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
    public class FoodCategoriesController : Controller
    {
        private DataAccess m_DataAccess;

        public FoodCategoriesController(IConfiguration configuration)
        {
            m_DataAccess = new DataAccess(configuration);
        }


        // GET: api/FoodCategories
        [HttpGet]
        public JSONRetObj<IEnumerable<FoodCategories>> Get()
        {
            JSONRetObj<IEnumerable<FoodCategories>> retObj = new JSONRetObj<IEnumerable<FoodCategories>>();
            string sql = " SELECT FC_ID, FC_NAME ";
            sql += " FROM FoodCategories ";
            DataTable retTbl = m_DataAccess.FillTable(sql);

            if (retTbl.Rows.Count == 0)
            {
                retObj.IsSuccess = false;
                retObj.Message = "Empty Response Obj";
                retObj.ResponseObj = null;

                return retObj;
            }

            List<FoodCategories> retLst = new List<FoodCategories>();
            foreach (DataRow row in retTbl.Rows)
            {
                retLst.Add(new FoodCategories()
                {
                    Id = (int)row["FC_ID"],
                    Name = (string)row["FC_NAME"]
                });
            }

            retObj.IsSuccess = true;
            retObj.Message = "";
            retObj.ResponseObj = retLst;

            return retObj;
        }

        // GET api/FoodCategories/5
        [HttpGet("{id}")]
        public JSONRetObj<FoodCategories> Get(int id)
        {
            JSONRetObj<FoodCategories> retObj = new JSONRetObj<FoodCategories>();

            List<SqlParameter> sqlParams = new List<SqlParameter>();
            sqlParams.Add("@id", SqlDbType.Int, (object)id);

            string sql = " SELECT FC_ID, FC_NAME ";
            sql += " FROM FoodCategories ";
            sql += " WHERE FC_ID = @id ";

            DataTable retTbl = m_DataAccess.FillTable(sql, sqlParams);

            if (retTbl.Rows.Count == 0)
            {
                retObj.IsSuccess = false;
                retObj.Message = "Empty Response Obj";
                retObj.ResponseObj = null;

                return retObj;
            }

            FoodCategories ret = new FoodCategories()
            {
                Id = (int)retTbl.Rows[0]["FC_ID"],
                Name = (string)retTbl.Rows[0]["FC_NAME"]
            };

            retObj.IsSuccess = true;
            retObj.Message = "";
            retObj.ResponseObj = ret;

            return retObj;
        }

        // POST api/FoodCategories
        [HttpPost]
        public JSONRetObj<int?> Post([FromBody]PostFoodCategories reqObj)
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
                    using (SqlCommand cmd = m_DataAccess.GetCommand("ADD_CATEGORY", CommandType.StoredProcedure, conn))
                    {
                        cmd.Parameters.Add(m_DataAccess.BuildSqlParam("@name", SqlDbType.VarChar, reqObj.Name));
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
