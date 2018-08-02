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
    public class FoodMarkerRepository : IFoodMarkerRepository
    {
        DataAccess m_DataAccess;

        public FoodMarkerRepository(IConfiguration configuration)
        {
            m_DataAccess = new DataAccess(configuration.GetConnectionString("FeedMapDataBase"));
        }

        public IEnumerable<FoodMarkerDTO> GetFoodMarkers()
        {
            string sql = " SELECT FM_ID, FM_FC_ID, FM_REST_ID, FM_NAME, ";
            sql += " FM_COMMENT, FM_RATING ";
            sql += " FROM FoodMarker ";
            DataTable retTbl = m_DataAccess.FillTable(sql);

            if (retTbl.Rows.Count == 0) return null;

            List<FoodMarkerDTO> retLst = new List<FoodMarkerDTO>();
            foreach (DataRow row in retTbl.Rows)
            {
                retLst.Add(new FoodMarkerDTO()
                {
                    Id = (int)row["FM_ID"],
                    FoodCategoryId = (int)row["FM_FC_ID"],
                    RestaurantId = (int)row["FM_REST_ID"],
                    Name = (string)row["FM_NAME"],
                    Comment = (string)row["FM_COMMENT"],
                    Rating = (int)row["FM_RATING"]
                });
            }

            return retLst;

        }

        public FoodMarkerDTO GetFoodMarker(int id)
        {
            List<SqlParameter> sqlParams = new List<SqlParameter>();
            sqlParams.Add("@id", SqlDbType.Int, (object)id);

            string sql = " SELECT FM_ID, FM_FC_ID, FM_REST_ID, FM_NAME, ";
            sql += " FM_COMMENT, FM_RATING ";
            sql += " FROM FoodMarker ";
            sql += " WHERE FM_ID = @id ";

            DataTable retTbl = m_DataAccess.FillTable(sql, sqlParams);

            if (retTbl.Rows.Count == 0) return null;

            FoodMarkerDTO ret = new FoodMarkerDTO()
            {
                Id = (int)retTbl.Rows[0]["FM_ID"],
                FoodCategoryId = (int)retTbl.Rows[0]["FM_FC_ID"],
                RestaurantId = (int)retTbl.Rows[0]["FM_REST_ID"],
                Name = (string)retTbl.Rows[0]["FM_NAME"],
                Comment = (string)retTbl.Rows[0]["FM_COMMENT"],
                Rating = (int)retTbl.Rows[0]["FM_RATING"]
            };

            return ret;
        }

        public int Post(FoodMarkerDTO foodMarker)
        {
            using (SqlConnection conn = new SqlConnection(m_DataAccess.ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = m_DataAccess.GetCommand("ADD_FOODMARKER", CommandType.StoredProcedure, conn))
                {
                    cmd.Parameters.Add(m_DataAccess.BuildSqlParam("@fcid", SqlDbType.Int, foodMarker.FoodCategoryId));
                    cmd.Parameters.Add(m_DataAccess.BuildSqlParam("@frid", SqlDbType.Int, foodMarker.RestaurantId));
                    cmd.Parameters.Add(m_DataAccess.BuildSqlParam("@name", SqlDbType.VarChar, foodMarker.Name));
                    cmd.Parameters.Add(m_DataAccess.BuildSqlParam("@comment", SqlDbType.VarChar, foodMarker.Comment));
                    cmd.Parameters.Add(m_DataAccess.BuildSqlParam("@rating", SqlDbType.Int, foodMarker.Rating));
                    cmd.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();
                    return (int)cmd.Parameters["@id"].Value;
                }
            }
        }

        public void Update(FoodMarkerDTO foodMarker, int id)
        {
            throw new NotImplementedException();
        }
    }
}
