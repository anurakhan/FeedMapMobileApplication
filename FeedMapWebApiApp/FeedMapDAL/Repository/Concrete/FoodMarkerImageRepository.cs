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
    public class FoodMarkerImageRepository : IFoodMarkerImageRepository
    {
        DataAccess m_DataAccess;

        public FoodMarkerImageRepository(IConfiguration configuration)
        {
            m_DataAccess = new DataAccess(configuration.GetConnectionString("FeedMapDataBase"));
        }

        public IEnumerable<FoodMarkerImageDataDTO> GetFoodMarkerImages()
        {
            string sql = " SELECT FMP_ID, FMP_FM_ID, FMP_FILE_NAME, FMP_CLIENT_FILE_NAME, ";
            sql += " FMP_FMPR_ID FROM FoodMarkerPhotos ";
            DataTable retTbl = m_DataAccess.FillTable(sql);

            if (retTbl.Rows.Count == 0) return null;

            List<FoodMarkerImageDataDTO> retLst = new List<FoodMarkerImageDataDTO>();
            foreach (DataRow row in retTbl.Rows)
            {
                retLst.Add(new FoodMarkerImageDataDTO()
                {
                    Id = (int)row["FMP_ID"],
                    FoodMarkerId = (int)row["FMP_FM_ID"],
                    FileName = (string)row["FMP_FILE_NAME"],
                    ClientFileName = (string)row["FMP_CLIENT_FILE_NAME"],
                    ImageRank = (int?)row["FMP_FMPR_ID"]
                });
            }
            return retLst;
        }

        public FoodMarkerImageDataDTO GetFoodMarkerImage(int id)
        {
            List<SqlParameter> sqlParams = new List<SqlParameter>();
            sqlParams.Add("@id", SqlDbType.Int, (object)id);

            string sql = " SELECT FMP_FM_ID, FMP_FILE_NAME, FMP_CLIENT_FILE_NAME, ";
            sql += " FMP_FMPR_ID FROM FoodMarkerPhotos ";
            sql += " WHERE FMP_ID = @id ";

            DataTable tbl = m_DataAccess.FillTable(sql, sqlParams);

            if (tbl.Rows.Count == 0) return null;

            return new FoodMarkerImageDataDTO
            {
                Id = id,
                FoodMarkerId = (int)tbl.Rows[0]["FMP_FM_ID"],
                FileName = (string)tbl.Rows[0]["FMP_FILE_NAME"],
                ClientFileName = (string)tbl.Rows[0]["FMP_CLIENT_FILE_NAME"],
                ImageRank = (int?)tbl.Rows[0]["FMP_FMPR_ID"]
            };
        }

        public IEnumerable<FoodMarkerImageDataDTO> GetFoodMarkerImageByFoodMarkerId(int id)
        {
            List<SqlParameter> sqlParams = new List<SqlParameter>();
            sqlParams.Add("@foodMarkerId", SqlDbType.Int, (object)id);

            string sql = " SELECT FMP_ID, FMP_FM_ID, FMP_FILE_NAME, FMP_CLIENT_FILE_NAME, ";
            sql += " FMP_FMPR_ID FROM FoodMarkerPhotos ";
            sql += " WHERE FMP_FM_ID = @foodMarkerId ";
            DataTable retTbl = m_DataAccess.FillTable(sql, sqlParams);

            if (retTbl.Rows.Count == 0) return null;

            List<FoodMarkerImageDataDTO> retLst = new List<FoodMarkerImageDataDTO>();
            foreach (DataRow row in retTbl.Rows)
            {
                retLst.Add(new FoodMarkerImageDataDTO()
                {
                    Id = (int)row["FMP_ID"],
                    FoodMarkerId = (int)row["FMP_FM_ID"],
                    FileName = (string)row["FMP_FILE_NAME"],
                    ClientFileName = (string)row["FMP_CLIENT_FILE_NAME"],
                    ImageRank = (int?)row["FMP_FMPR_ID"]
                });
            }
            return retLst;
        }

        public FoodMarkerImageDataDTO GetTopFoodMarkerImageByFoodMarkerId(int id)
        {
            List<SqlParameter> sqlParams = new List<SqlParameter>();
            sqlParams.Add("@foodMarkerId", SqlDbType.Int, (object)id);

            string sql = " SELECT TOP(1) FMP_ID, FMP_FM_ID, FMP_FILE_NAME, FMP_CLIENT_FILE_NAME, ";
            sql += " FMP_FMPR_ID FROM FoodMarkerPhotos ";
            sql += " WHERE FMP_FM_ID = @foodMarkerId ";
            DataTable tbl = m_DataAccess.FillTable(sql, sqlParams);

            if (tbl.Rows.Count == 0) return null;

            return new FoodMarkerImageDataDTO
            {
                Id = (int)tbl.Rows[0]["FMP_ID"],
                FoodMarkerId = (int)tbl.Rows[0]["FMP_FM_ID"],
                FileName = (string)tbl.Rows[0]["FMP_FILE_NAME"],
                ClientFileName = (string)tbl.Rows[0]["FMP_CLIENT_FILE_NAME"],
                ImageRank = (int?)tbl.Rows[0]["FMP_FMPR_ID"]
            };
        }

        public int Post(FoodMarkerImageDataDTO foodMarkerImg)
        {
            using (SqlConnection conn = new SqlConnection(m_DataAccess.ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = m_DataAccess.GetCommand("ADD_IMAGE", CommandType.StoredProcedure, conn))
                {
                    cmd.Parameters.Add(m_DataAccess.BuildSqlParam("@fmid", SqlDbType.Int, foodMarkerImg.FoodMarkerId));
                    cmd.Parameters.Add(m_DataAccess.BuildSqlParam("@filename", SqlDbType.VarChar, foodMarkerImg.FileName));
                    cmd.Parameters.Add(m_DataAccess.BuildSqlParam("@clientfilename", SqlDbType.VarChar, foodMarkerImg.ClientFileName));
                    cmd.Parameters.Add(m_DataAccess.BuildSqlParam("@rankId", SqlDbType.Int, foodMarkerImg.ImageRank));
                    cmd.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();
                    return (int)cmd.Parameters["@id"].Value;
                }
            }
        }

        public void Update(FoodMarkerImageDataDTO foodMarkerImg, int id)
        {
            throw new NotImplementedException();
        }

        public void DeleteByFoodMarker(int id)
        {
            List<SqlParameter> sqlParams = new List<SqlParameter>();
            sqlParams.Add("@id", SqlDbType.Int, (object)id);

            string sql = " DELETE ";
            sql += " FROM FoodMarkerPhotos ";
            sql += " WHERE FMP_FM_ID = @id ";

            m_DataAccess.ExecuteNonQuery(sql, sqlParams);
        }
    }
}
