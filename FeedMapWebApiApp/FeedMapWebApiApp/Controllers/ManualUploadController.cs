using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FeedMapWebApiApp.Models;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Web;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Auth;
using System.IO;

namespace FeedMapWebApiApp.Controllers
{
    public class ManualUploadController : Controller
    {
        private DataAccess m_DataAccess;
        private string key = "FeedMapAccessWebApp1";
        private string m_StorageConnectionString;

        public ManualUploadController(IConfiguration configuration)
        {
            m_DataAccess = new DataAccess(configuration);
            m_StorageConnectionString = configuration["AzureStorageConnectionString:FeedMapStorage"];
        }



        private bool IsAuth()
        {
            string claimKey = HttpContext.Request.Query["key"].ToString();
            if (String.IsNullOrEmpty(claimKey)) return false;
            if (claimKey != key) return false;
            return true;
        }

        public IActionResult CreateFoodMarker()
        {
            if (!IsAuth()) return View("_NotAllowed");
            return View();
        }

        public IActionResult CreateCategory()
        {
            if (!IsAuth()) return View("_NotAllowed");
            return View();
        }

        // GET: /<controller>/
        public IActionResult CreateRestaurant()
        {
            if (!IsAuth()) return View("_NotAllowed");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateFoodMarker(PostFoodMarker reqObj)
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
                    using (SqlTransaction tran = conn.BeginTransaction())
                    {
                        try
                        {
                            using (SqlCommand cmd = m_DataAccess.GetCommand("ADD_FOODMARKER", CommandType.StoredProcedure, conn, tran))
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
                            foreach (var file in Request.Form.Files)
                            {
                                int imageId;
                                ImageFileNameConverter conv = new ImageFileNameConverter();
                                string fileName = conv.Convert(file.FileName);
                                using (SqlCommand cmd = m_DataAccess.GetCommand("ADD_IMAGE", CommandType.StoredProcedure, conn, tran))
                                {
                                    cmd.Parameters.Add(m_DataAccess.BuildSqlParam("@fmid", SqlDbType.Int, retObj.ResponseObj));
                                    cmd.Parameters.Add(m_DataAccess.BuildSqlParam("@filename", SqlDbType.VarChar, fileName));
                                    cmd.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;

                                    cmd.ExecuteNonQuery();
                                    imageId = (int)cmd.Parameters["@id"].Value;
                                }
                                Stream stream = file.OpenReadStream();
                                AzureStorageHandler handler = new AzureStorageHandler(m_StorageConnectionString, "feedmapimages");
                                await handler.UploadFile(imageId.ToString() + "_" + fileName, file.OpenReadStream(), file.ContentType);
                            }
                            tran.Commit();
                        }
                        catch
                        {
                            tran.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                retObj.IsSuccess = false;
                retObj.Message = ex.Message;
                retObj.ResponseObj = null;
            }

            ViewData["RetObj"] = retObj;

            return View("_Success");
        }

        [HttpPost]
        public IActionResult CreateCategory(PostFoodCategories reqObj)
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
            catch (Exception ex)
            {
                retObj.IsSuccess = false;
                retObj.Message = ex.Message;
                retObj.ResponseObj = null;
            }

            ViewData["RetObj"] = retObj;

            return View("_Success");
        }

        [HttpPost]
        public IActionResult CreateRestaurant(PostRestaurant reqObj)
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
            catch (Exception ex)
            {
                retObj.IsSuccess = false;
                retObj.Message = ex.Message;
                retObj.ResponseObj = null;
            }

            ViewData["RetObj"] = retObj;

            return View("_Success");
        }
    }
}
