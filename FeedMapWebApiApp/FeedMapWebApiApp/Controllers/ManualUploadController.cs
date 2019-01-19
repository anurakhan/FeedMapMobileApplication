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
using FeedMapDAL;
using FeedMapDAL.Repository.Abstract;
using FeedMapBLL.Domain;
using AutoMapper;
using FeedMapDTO;

namespace FeedMapWebApiApp.Controllers
{
    /// <summary>
    /// This is Temporarily broken. Need to Replace this with Fontend Application
    /// that will also consume the web api.
    /// </summary>
    public class ManualUploadController : Controller
    {
        private string key = "FeedMapAccessWebApp1";
        private IFoodMarkerImageRepository _repoImageMeta;
        private IMediaFileRepository _repoImageFile;
        private IFoodMarkerRepository _repoFoodMarker;

        private IFoodCategoryRepository _repoFoodCategory;
        private IRestaurantRepository _repoRestaurant;

        public ManualUploadController(RepositoryPayload repoPayload)
        {
            _repoImageMeta = repoPayload.GetFoodMarkerImageRepository();
            _repoImageFile = repoPayload.GetFileRepository();
            _repoFoodMarker = repoPayload.GetFoodMarkerRepository();
            _repoFoodCategory = repoPayload.GetFoodCategoryRepository();
            _repoRestaurant = repoPayload.GetRestaurantRepository();
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
        public async Task<IActionResult> CreateFoodMarker(FoodMarkerClient reqObj)
        {
            if (reqObj == null)
            {
                BadRequest();
            }

            JSONRetObj<int?> retObj = new JSONRetObj<int?>();
            try
            {
                retObj.IsSuccess = true;
                FoodMarker foodMarker = Mapper.Map<FoodMarker>(reqObj);
                FoodMarkerDTO foodMarkerDto = Mapper.Map<FoodMarkerDTO>(foodMarker);
                int foodMarkerId = _repoFoodMarker.Post(foodMarkerDto);

                foreach (var file in Request.Form.Files)
                {
                    FoodMarkerImageData postImageMeta = new FoodMarkerImageData(foodMarkerId, file.FileName);

                    var postImageMetaDto = Mapper.Map<FoodMarkerImageDataDTO>(postImageMeta);

                    postImageMeta.Id = postImageMetaDto.Id = _repoImageMeta.Post(postImageMetaDto);

                    Stream stream = file.OpenReadStream();
                    await _repoImageFile.PostFile(postImageMetaDto, file.ContentType, stream);
                }

                retObj.ResponseObj = foodMarkerId;
            }
            catch (Exception ex)
            {
                retObj.IsSuccess = false;
                retObj.Message = ex.Message;
            }

            ViewData["RetObj"] = retObj;

            return View("_Success");
        }

        [HttpPost]
        public IActionResult CreateCategory(FoodCategoryClient reqObj)
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
                FoodCategory foodCategory = Mapper.Map<FoodCategory>(reqObj);
                FoodCategoryDTO foodCategoryDto = Mapper.Map<FoodCategoryDTO>(foodCategory);
                int foodMarkerId = _repoFoodCategory.Post(foodCategoryDto);
                retObj.ResponseObj = foodMarkerId;
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
        public IActionResult CreateRestaurant(RestaurantClient reqObj)
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
                Restaurant restaurant = Mapper.Map<Restaurant>(reqObj);
                RestaurantDTO restaurantDto = Mapper.Map<RestaurantDTO>(restaurant);
                int foodMarkerId = _repoRestaurant.Post(restaurantDto);
                retObj.ResponseObj = foodMarkerId;
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
