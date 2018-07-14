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
    public class FullFoodAndGeoDataController : Controller
    {
        private DataAccess m_DataAccess;

        public FullFoodAndGeoDataController(IConfiguration configuration)
        {
            m_DataAccess = new DataAccess(configuration);
        }

        // GET: api/FullFoodAndGeoData
        [HttpGet]
        public JSONRetObj<IEnumerable<FullFoodAndGeoData>> Get()
        {
            JSONRetObj<IEnumerable<FullFoodAndGeoData>> retObj = new JSONRetObj<IEnumerable<FullFoodAndGeoData>>();

            string sql = " SELECT FM_ID, FM_NAME, REST_NAME, (REST_POSITION.ToString()) AS 'REST_POSITION', ";
            sql += " REST_ADDRESS, FM_COMMENT, FM_RATING, FC_NAME ";
            sql += " FROM FoodMarker ";
            sql += " INNER JOIN Restaurants on FM_REST_ID = REST_ID ";
            sql += " INNER JOIN FoodCategories on FM_FC_ID = FC_ID ";
            DataTable retTbl = m_DataAccess.FillTable(sql);

            if (retTbl.Rows.Count == 0) 
            {
                retObj.IsSuccess = false;
                retObj.Message = "Empty Response Obj";
                retObj.ResponseObj = null;

                return retObj;
            }

            List<FullFoodAndGeoData> retLst = new List<FullFoodAndGeoData>();
            foreach (DataRow row in retTbl.Rows)
            {
                retLst.Add(new FullFoodAndGeoData()
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

            retObj.IsSuccess = true;
            retObj.Message = "";
            retObj.ResponseObj = retLst;

            return retObj;

        }

        // GET api/FullFoodAndGeoData/5
        [HttpGet("{id}")]
        public JSONRetObj<FullFoodAndGeoData> Get(int id)
        {
            JSONRetObj<FullFoodAndGeoData> retObj = new JSONRetObj<FullFoodAndGeoData>();

            List<SqlParameter> sqlParams = new List<SqlParameter>();
            sqlParams.Add("@id", SqlDbType.Int, (object)id);

            string sql = " SELECT FM_ID, FM_NAME, REST_NAME, (REST_POSITION.ToString()) AS 'REST_POSITION', ";
            sql += " REST_ADDRESS, FM_COMMENT, FM_RATING, FC_NAME ";
            sql += " FROM FoodMarker ";
            sql += " INNER JOIN Restaurants on FM_REST_ID = REST_ID ";
            sql += " INNER JOIN FoodCategories on FM_FC_ID = FC_ID ";
            sql += " WHERE FM_ID = @id ";
                
            DataTable retTbl = m_DataAccess.FillTable(sql, sqlParams);

            if (retTbl.Rows.Count == 0)
            {
                retObj.IsSuccess = false;
                retObj.Message = "Empty Response Obj";
                retObj.ResponseObj = null;

                return retObj;
            }

            FullFoodAndGeoData ret = new FullFoodAndGeoData()
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

            retObj.IsSuccess = true;
            retObj.Message = "";
            retObj.ResponseObj = ret;

            return retObj;
        }
    }
}
