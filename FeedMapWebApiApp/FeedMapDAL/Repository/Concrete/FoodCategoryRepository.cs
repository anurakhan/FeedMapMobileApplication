using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using FeedMapDAL.Helper;
using FeedMapDAL.Repository.Abstract;
using FeedMapDTO;
using Microsoft.Extensions.Configuration;

namespace FeedMapDAL.Repository.Concrete
{
    public class FoodCategoryRepository : IFoodCategoryRepository
    {
        DataAccess m_DataAccess;

        public FoodCategoryRepository(IConfiguration configuration)
        {
            m_DataAccess = new DataAccess(configuration.GetConnectionString("FeedMapDataBase"));
        }

        public IEnumerable<FoodCategoryDTO> GetFoodCategories()
        {
            string sql = " SELECT FC_ID, FC_NAME ";
            sql += " FROM FoodCategories ";
            DataTable retTbl = m_DataAccess.FillTable(sql);

            if (retTbl.Rows.Count == 0) return null;

            List<FoodCategoryDTO> retLst = new List<FoodCategoryDTO>();
            foreach (DataRow row in retTbl.Rows)
            {
                retLst.Add(new FoodCategoryDTO()
                {
                    Id = (int)row["FC_ID"],
                    Name = (string)row["FC_NAME"]
                });
            }

            return retLst;
        }

        public FoodCategoryDTO GetFoodCategory(int id)
        {
            List<SqlParameter> sqlParams = new List<SqlParameter>();
            sqlParams.Add("@id", SqlDbType.Int, (object)id);

            string sql = " SELECT FC_ID, FC_NAME ";
            sql += " FROM FoodCategories ";
            sql += " WHERE FC_ID = @id ";

            DataTable retTbl = m_DataAccess.FillTable(sql, sqlParams);

            if (retTbl.Rows.Count == 0) return null;

            FoodCategoryDTO ret = new FoodCategoryDTO()
            {
                Id = (int)retTbl.Rows[0]["FC_ID"],
                Name = (string)retTbl.Rows[0]["FC_NAME"]
            };

            return ret;
        }

        public int Post(FoodCategoryDTO foodCategory)
        {
            using (SqlConnection conn = new SqlConnection(m_DataAccess.ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = m_DataAccess.GetCommand("ADD_CATEGORY", CommandType.StoredProcedure, conn))
                {
                    cmd.Parameters.Add(m_DataAccess.BuildSqlParam("@name", SqlDbType.VarChar, foodCategory.Name));
                    cmd.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();
                    return (int)cmd.Parameters["@id"].Value;
                }
            }
        }

        public void Update(FoodCategoryDTO foodCategory, int id)
        {
            throw new NotImplementedException();
        }
    }
}
