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
    public class RestaurantRepository : IRestaurantRepository
    {
        DataAccess m_DataAccess;

        public RestaurantRepository(IConfiguration configuration)
        {
            m_DataAccess = new DataAccess(configuration.GetConnectionString("FeedMapDataBase"));
        }

        public IEnumerable<RestaurantDTO> GetRestaurants()
        {
            string sql = " SELECT REST_ID, REST_NAME, (REST_POSITION.ToString()) AS 'REST_POSITION', REST_ADDRESS ";
            sql += " FROM Restaurants ";
            DataTable retTbl = m_DataAccess.FillTable(sql);

            if (retTbl.Rows.Count == 0) return null;

            List<RestaurantDTO> retLst = new List<RestaurantDTO>();
            foreach (DataRow row in retTbl.Rows)
            {
                retLst.Add(new RestaurantDTO()
                {
                    Id = (int)row["REST_ID"],
                    Name = (string)row["REST_NAME"],
                    Position = (string)row["REST_POSITION"],
                    Address = (string)row["REST_ADDRESS"]
                });
            }

            return retLst;
        }

        public RestaurantDTO GetRestaurant(int id)
        {
            List<SqlParameter> sqlParams = new List<SqlParameter>();
            sqlParams.Add("@id", SqlDbType.Int, (object)id);

            string sql = " SELECT REST_ID, REST_NAME, (REST_POSITION.ToString()) AS 'REST_POSITION', REST_ADDRESS ";
            sql += " FROM Restaurants ";
            sql += " WHERE REST_ID = @id ";

            DataTable retTbl = m_DataAccess.FillTable(sql, sqlParams);

            if (retTbl.Rows.Count == 0) return null;

            RestaurantDTO ret = new RestaurantDTO()
            {
                Id = (int)retTbl.Rows[0]["REST_ID"],
                Name = (string)retTbl.Rows[0]["REST_NAME"],
                Position = (string)retTbl.Rows[0]["REST_POSITION"],
                Address = (string)retTbl.Rows[0]["REST_ADDRESS"]
            };

            return ret;
        }


        public int Post(RestaurantDTO restaurant)
        {
            using (SqlConnection conn = new SqlConnection(m_DataAccess.ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = m_DataAccess.GetCommand("ADD_RESTAURANT", CommandType.StoredProcedure, conn))
                {
                    cmd.Parameters.Add(m_DataAccess.BuildSqlParam("@name", SqlDbType.VarChar, restaurant.Name));
                    cmd.Parameters.Add(m_DataAccess.BuildSqlParam("@position", SqlDbType.VarChar, restaurant.Position));
                    cmd.Parameters.Add(m_DataAccess.BuildSqlParam("@address", SqlDbType.VarChar, restaurant.Address));
                    cmd.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();
                    return (int)cmd.Parameters["@id"].Value;
                }
            }
        }

        public void Update(RestaurantDTO restaurant, int id)
        {
            throw new NotImplementedException();
        }
    }
}
