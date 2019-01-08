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
using FeedMapDAL;
using FeedMapDAL.Repository.Abstract;
using FeedMapDTO;

namespace FeedMapWebApiApp.Controllers
{
    [Route("api/[controller]")]
    public class PhotosController : Controller
    {
        private IFoodMarkerImageRepository _repoImageMeta;
        private IMediaFileRepository _repoImageFile;

        public PhotosController(RepositoryPayload repoPayload)
        {
            _repoImageMeta = repoPayload.GetFoodMarkerImageRepository();
            _repoImageFile = repoPayload.GetFileRepository();
        }

        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            FoodMarkerImageDTO imageMeta = _repoImageMeta.GetFoodMarkerImage(id);

            if (imageMeta == null) return NotFound();

            return Ok(
                new FoodMarkerImageDataClient { ImageUrl = WebUtility.UrlEncode(_repoImageFile.GetFileUrl(imageMeta)) }
            );
        }

        [HttpGet]
        public ActionResult GetByFoodMarkerId([FromQuery(Name = "foodMarkerId")] int foodMarkerId)
        {
            var imageMetas = _repoImageMeta.GetFoodMarkerImageByFoodMarkerId(foodMarkerId);

            if (imageMetas == null || !imageMetas.Any()) return NotFound();

            List<FoodMarkerImageDataClient> lstFoodMarkerImageData =
                new List<FoodMarkerImageDataClient>();

            foreach (var imageMeta in imageMetas)
            {
                lstFoodMarkerImageData.Add(
                    new FoodMarkerImageDataClient
                    {
                        ImageUrl = WebUtility.UrlEncode(_repoImageFile.GetFileUrl(imageMeta)),
                        imageRank = (imageMeta.ImageRank.HasValue ?
                                 imageMeta.ImageRank.Value : 2)
                    });
            }

            return Ok(
                lstFoodMarkerImageData
            );
        }
    }
}
