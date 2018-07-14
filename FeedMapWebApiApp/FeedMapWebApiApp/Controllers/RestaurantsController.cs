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
    public class RestaurantsController : Controller
    {
        private DataAccess m_DataAccess;

        public RestaurantsController(IConfiguration configuration)
        {
            m_DataAccess = new DataAccess(configuration);
        }

        // GET: api/Restaurants
        [HttpGet]
        public JSONRetObj<IEnumerable<Restaurants>> Get()
        {
            JSONRetObj<IEnumerable<Restaurants>> retObj = new JSONRetObj<IEnumerable<Restaurants>>();
            string sql = " SELECT REST_ID, REST_NAME, (REST_POSITION.ToString()) AS 'REST_POSITION', REST_ADDRESS ";
            sql += " FROM Restaurants ";
            DataTable retTbl = m_DataAccess.FillTable(sql);

            if (retTbl.Rows.Count == 0)
            {
                retObj.IsSuccess = false;
                retObj.Message = "Empty Response Obj";
                retObj.ResponseObj = null;

                return retObj;
            }

            List<Restaurants> retLst = new List<Restaurants>();
            foreach (DataRow row in retTbl.Rows)
            {
                retLst.Add(new Restaurants() {
                    Id = (int)row["REST_ID"],
                    Name = (string)row["REST_NAME"],
                    Position = (string)row["REST_POSITION"],
                    Address = (string)row["REST_ADDRESS"]
                });
            }

            retObj.IsSuccess = true;
            retObj.Message = "";
            retObj.ResponseObj = retLst;

            return retObj;

        }

        // GET api/Restaurants/5
        [HttpGet("{id}")]
        public JSONRetObj<Restaurants> Get(int id)
        {
            JSONRetObj<Restaurants> retObj = new JSONRetObj<Restaurants>();

            List<SqlParameter> sqlParams = new List<SqlParameter>();
            sqlParams.Add("@id", SqlDbType.Int, (object)id);

            string sql = " SELECT REST_ID, REST_NAME, (REST_POSITION.ToString()) AS 'REST_POSITION', REST_ADDRESS ";
            sql += " FROM Restaurants ";
            sql += " WHERE REST_ID = @id ";

            DataTable retTbl = m_DataAccess.FillTable(sql, sqlParams);

            if (retTbl.Rows.Count == 0)
            {
                retObj.IsSuccess = false;
                retObj.Message = "Empty Response Obj";
                retObj.ResponseObj = null;

                return retObj;
            }

            Restaurants ret = new Restaurants()
            {
                Id = (int)retTbl.Rows[0]["REST_ID"],
                Name = (string)retTbl.Rows[0]["REST_NAME"],
                Position = (string)retTbl.Rows[0]["REST_POSITION"],
                Address = (string)retTbl.Rows[0]["REST_ADDRESS"]
            };

            retObj.IsSuccess = true;
            retObj.Message = "";
            retObj.ResponseObj = ret;

            return retObj;
        }

        // POST api/Restaurants
        [HttpPost]
        public JSONRetObj<int?> Post([FromBody]PostRestaurant reqObj)
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
                    using (SqlCommand cmd = m_DataAccess.GetCommand("ADD_RESTAURANT", CommandType.StoredProcedure, conn))
                    {
                        cmd.Parameters.Add(m_DataAccess.BuildSqlParam("@name", SqlDbType.VarChar, reqObj.Name));
                        cmd.Parameters.Add(m_DataAccess.BuildSqlParam("@position", SqlDbType.VarChar, reqObj.Position));
                        cmd.Parameters.Add(m_DataAccess.BuildSqlParam("@address", SqlDbType.VarChar, reqObj.Address));
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
