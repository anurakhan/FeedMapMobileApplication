using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FeedMapWebApiApp.Models;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using FeedMapDTO;
using FeedMapBLL.Services.Abstract;
using AutoMapper;
using FeedMapBLL.Domain;

namespace FeedMapWebApiApp.Controllers
{
    [Route("api/[controller]")]
    public class PhotosController : Controller
    {
        private IPhotoService _service;
        private IMapper _mapper;

        public PhotosController(IPhotoService photoService,
                               IMapper mapper)
        {
            _service = photoService;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {

            FoodMarkerPhoto photo = _service.GetPhotoById(id);

            if (photo == null) return NotFound();

            return Ok(
                new FoodMarkerPhotoClient
                {
                    ImageUrl = WebUtility.UrlEncode(photo.ImageUrl),
                    ImageRank = photo.ImageRank
                }
            );
        }

        [HttpGet]
        public ActionResult GetByFoodMarkerId([FromQuery(Name = "foodMarkerId")] int foodMarkerId)
        {
            IEnumerable<FoodMarkerPhoto> photos =
                _service.GetPhotosByFoodMarkerId(foodMarkerId);

            if (photos == null) return NotFound();

            List<FoodMarkerPhotoClient> lstFoodMarkerImageData =
                new List<FoodMarkerPhotoClient>();

            foreach (var photo in photos)
            {
                lstFoodMarkerImageData.Add(
                    new FoodMarkerPhotoClient
                    {
                        ImageUrl = WebUtility.UrlEncode(photo.ImageUrl),
                        ImageRank = photo.ImageRank
                    });
            }

            return Ok(
                lstFoodMarkerImageData
            );
        }
    }
}
