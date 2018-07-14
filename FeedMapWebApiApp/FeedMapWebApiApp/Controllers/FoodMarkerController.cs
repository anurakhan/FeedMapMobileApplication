using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FeedMapWebApiApp.Models;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace FeedMapWebApiApp.Controllers
{
    [Route("api/[controller]")]
    public class FoodMarkerController : Controller
    {
        private DataAccess m_DataAccess;

        public FoodMarkerController(IConfiguration configuration)
        {
            m_DataAccess = new DataAccess(configuration);
        }

        // GET: api/FoodMarker
        [HttpGet]
        public JSONRetObj<IEnumerable<FoodMarker>> Get()
        {
            JSONRetObj<IEnumerable<FoodMarker>> retObj = new JSONRetObj<IEnumerable<FoodMarker>>();
            string sql = " SELECT FM_ID, FM_FC_ID, FM_REST_ID, FM_NAME, ";
            sql += " FM_COMMENT, FM_RATING ";
            sql += " FROM FoodMarker ";
            DataTable retTbl = m_DataAccess.FillTable(sql);

            if (retTbl.Rows.Count == 0)
            {
                retObj.IsSuccess = false;
                retObj.Message = "Empty Response Obj";
                retObj.ResponseObj = null;

                return retObj;
            }

            List<FoodMarker> retLst = new List<FoodMarker>();
            foreach (DataRow row in retTbl.Rows)
            {
                retLst.Add(new FoodMarker()
                {
                    Id = (int)row["FM_ID"],
                    FoodCategoryId = (int)row["FM_FC_ID"],
                    RestaurantId = (int)row["FM_REST_ID"],
                    Name = (string)row["FM_NAME"],
                    Comment = (string)row["FM_COMMENT"],
                    Rating = (int)row["FM_RATING"]
                });
            }

            retObj.IsSuccess = true;
            retObj.Message = "";
            retObj.ResponseObj = retLst;

            return retObj;
        }

        // GET api/FoodMarker/5
        [HttpGet("{id}")]
        public JSONRetObj<FoodMarker> Get(int id)
        {
            JSONRetObj<FoodMarker> retObj = new JSONRetObj<FoodMarker>();

            List<SqlParameter> sqlParams = new List<SqlParameter>();
            sqlParams.Add("@id", SqlDbType.Int, (object)id);

            string sql = " SELECT FM_ID, FM_FC_ID, FM_REST_ID, FM_NAME, ";
            sql += " FM_COMMENT, FM_RATING ";
            sql += " FROM FoodMarker ";
            sql += " WHERE FM_ID = @id ";

            DataTable retTbl = m_DataAccess.FillTable(sql, sqlParams);

            if (retTbl.Rows.Count == 0)
            {
                retObj.IsSuccess = false;
                retObj.Message = "Empty Response Obj";
                retObj.ResponseObj = null;

                return retObj;
            }

            FoodMarker ret = new FoodMarker()
            {
                Id = (int)retTbl.Rows[0]["FM_ID"],
                FoodCategoryId = (int)retTbl.Rows[0]["FM_FC_ID"],
                RestaurantId = (int)retTbl.Rows[0]["FM_REST_ID"],
                Name = (string)retTbl.Rows[0]["FM_NAME"],
                Comment = (string)retTbl.Rows[0]["FM_COMMENT"],
                Rating = (int)retTbl.Rows[0]["FM_RATING"]
            };

            retObj.IsSuccess = true;
            retObj.Message = "";
            retObj.ResponseObj = ret;

            return retObj;
        }

        // POST api/FoodMarker
        [HttpPost]
        public JSONRetObj<int?> Post([FromBody]PostFoodMarker reqObj)
        {
            if (reqObj == null)
            {
                BadRequest();
            }

            JSONRetObj<int?> retObj = new JSONRetObj<int?>();
            try
            {
                retObj.IsSuccess = true;
                retObj.Message = "";
                using (SqlConnection conn = new SqlConnection(m_DataAccess.ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = m_DataAccess.GetCommand("ADD_FOODMARKER", CommandType.StoredProcedure, conn))
                    {
                        cmd.Parameters.Add(m_DataAccess.BuildSqlParam("@fcid", SqlDbType.Int, reqObj.FoodCategoryId));
                        cmd.Parameters.Add(m_DataAccess.BuildSqlParam("@frid", SqlDbType.Int, reqObj.RestaurantId));
                        cmd.Parameters.Add(m_DataAccess.BuildSqlParam("@name", SqlDbType.VarChar, reqObj.Name));
                        cmd.Parameters.Add(m_DataAccess.BuildSqlParam("@comment", SqlDbType.VarChar, reqObj.Comment));
                        cmd.Parameters.Add(m_DataAccess.BuildSqlParam("@rating", SqlDbType.Int, reqObj.Rating));
                        cmd.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;

                        cmd.ExecuteNonQuery();
                        retObj.ResponseObj = (int)cmd.Parameters["@id"].Value;
                    }
                }
            }
            catch
            {
                retObj.IsSuccess = false;
                retObj.Message = "Error During querying";
                retObj.ResponseObj = null;
            }

            return retObj;
        }

    }
}
