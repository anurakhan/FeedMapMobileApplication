﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using FeedMapDAL.Helper;
using FeedMapDAL.Repository.Abstract;
using FeedMapDTO;
using Microsoft.Extensions.Configuration;

namespace FeedMapDAL.Repository.Concrete
{
    public class CompleteFoodDataRepository : ICompleteFoodDataRepository
    {
        DataAccess m_DataAccess;

        public CompleteFoodDataRepository(IConfiguration configuration)
        {
            m_DataAccess = new DataAccess(configuration.GetConnectionString("FeedMapDataBase"));
        }

        public IEnumerable<CompleteFoodDataDTO> GetCompleteFoodDatas()
        {
            string sql = " SELECT FM_ID, FM_NAME, REST_NAME, (REST_POSITION.ToString()) AS 'REST_POSITION', ";
            sql += " REST_ADDRESS, FM_COMMENT, FM_RATING, FC_NAME ";
            sql += " FROM FoodMarker ";
            sql += " INNER JOIN Restaurants on FM_REST_ID = REST_ID ";
            sql += " INNER JOIN FoodCategories on FM_FC_ID = FC_ID ";
            DataTable retTbl = m_DataAccess.FillTable(sql);

            if (retTbl.Rows.Count == 0) return null;

            List<CompleteFoodDataDTO> retLst = new List<CompleteFoodDataDTO>();
            foreach (DataRow row in retTbl.Rows)
            {
                retLst.Add(new CompleteFoodDataDTO()
                {
                    FoodMarkerId = (int)row["FM_ID"],
                    FoodName = (string)row["FM_NAME"],
                    RestaurantName = (string)row["REST_NAME"],
                    RestaurantPosition = (string)row["REST_POSITION"],
                    RestaurantAddress = (string)row["REST_ADDRESS"],
                    Comment = (string)row["FM_COMMENT"],
                    Rating = (int)row["FM_RATING"],
                    CategoryName = (string)row["FC_NAME"]
                });
            }

            return retLst;
        }

        public IEnumerable<CompleteFoodDataDTO> GetCompleteFoodDatasByUserId(int id)
        {
            List<SqlParameter> sqlParams = new List<SqlParameter>();
            sqlParams.Add("@id", SqlDbType.Int, (object)id);

            string sql = " SELECT FM_ID, FM_NAME, REST_NAME, (REST_POSITION.ToString()) AS 'REST_POSITION', ";
            sql += " REST_ADDRESS, FM_COMMENT, FM_RATING, FC_NAME ";
            sql += " FROM FoodMarker ";
            sql += " INNER JOIN Restaurants on FM_REST_ID = REST_ID ";
            sql += " INNER JOIN FoodCategories on FM_FC_ID = FC_ID ";
            sql += " WHERE FM_USER_ID = @id ";
            DataTable retTbl = m_DataAccess.FillTable(sql, sqlParams);

            if (retTbl.Rows.Count == 0) return null;

            List<CompleteFoodDataDTO> retLst = new List<CompleteFoodDataDTO>();
            foreach (DataRow row in retTbl.Rows)
            {
                retLst.Add(new CompleteFoodDataDTO()
                {
                    FoodMarkerId = (int)row["FM_ID"],
                    FoodName = (string)row["FM_NAME"],
                    RestaurantName = (string)row["REST_NAME"],
                    RestaurantPosition = (string)row["REST_POSITION"],
                    RestaurantAddress = (string)row["REST_ADDRESS"],
                    Comment = (string)row["FM_COMMENT"],
                    Rating = (int)row["FM_RATING"],
                    CategoryName = (string)row["FC_NAME"]
                });
            }

            return retLst;
        }

        public CompleteFoodDataDTO GetCompleteFoodData(int id)
        {
            List<SqlParameter> sqlParams = new List<SqlParameter>();
            sqlParams.Add("@id", SqlDbType.Int, (object)id);

            string sql = " SELECT FM_ID, FM_NAME, REST_NAME, (REST_POSITION.ToString()) AS 'REST_POSITION', ";
            sql += " REST_ADDRESS, FM_COMMENT, FM_RATING, FC_NAME ";
            sql += " FROM FoodMarker ";
            sql += " INNER JOIN Restaurants on FM_REST_ID = REST_ID ";
            sql += " INNER JOIN FoodCategories on FM_FC_ID = FC_ID ";
            sql += " WHERE FM_ID = @id ";

            DataTable retTbl = m_DataAccess.FillTable(sql, sqlParams);

            if (retTbl.Rows.Count == 0) return null;

            CompleteFoodDataDTO ret = new CompleteFoodDataDTO()
            {
                FoodMarkerId = (int)retTbl.Rows[0]["FM_ID"],
                FoodName = (string)retTbl.Rows[0]["FM_NAME"],
                RestaurantName = (string)retTbl.Rows[0]["REST_NAME"],
                RestaurantPosition = (string)retTbl.Rows[0]["REST_POSITION"],
                RestaurantAddress = (string)retTbl.Rows[0]["REST_ADDRESS"],
                Comment = (string)retTbl.Rows[0]["FM_COMMENT"],
                Rating = (int)retTbl.Rows[0]["FM_RATING"],
                CategoryName = (string)retTbl.Rows[0]["FC_NAME"]
            };

            return ret;   
        }

        public int Post(CompleteFoodDataDTO completeFoodDatas, UserDTO user)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(m_DataAccess.ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = m_DataAccess.GetCommand("ADD_FOODMARKER_FROM_MOBILECLIENT",
                                                                    CommandType.StoredProcedure, conn))
                    {
                        cmd.Parameters.Add(m_DataAccess.BuildSqlParam("@foodCategoryName", SqlDbType.VarChar, completeFoodDatas.CategoryName));
                        cmd.Parameters.Add(m_DataAccess.BuildSqlParam("@foodRestaurantName", SqlDbType.VarChar, completeFoodDatas.RestaurantName));
                        cmd.Parameters.Add(m_DataAccess.BuildSqlParam("@foodRestaurantAddress", SqlDbType.VarChar, completeFoodDatas.RestaurantAddress));
                        cmd.Parameters.Add(m_DataAccess.BuildSqlParam("@position", SqlDbType.VarChar, completeFoodDatas.RestaurantPosition));
                        cmd.Parameters.Add(m_DataAccess.BuildSqlParam("@name", SqlDbType.VarChar, completeFoodDatas.FoodName));
                        cmd.Parameters.Add(m_DataAccess.BuildSqlParam("@comment", SqlDbType.VarChar, completeFoodDatas.Comment));
                        cmd.Parameters.Add(m_DataAccess.BuildSqlParam("@rating", SqlDbType.Int, completeFoodDatas.Rating));
                        cmd.Parameters.Add(m_DataAccess.BuildSqlParam("@userId", SqlDbType.Int, user.Id));

                        cmd.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;

                        cmd.ExecuteNonQuery();
                        return (int)cmd.Parameters["@id"].Value;
                    }
                }
            }
            catch(Exception ex)
            {
                
            }
            return -1;
        }

        public void Update(CompleteFoodDataDTO completeFoodDatas, int id)
        {
            throw new NotImplementedException();
        }
    }
}
