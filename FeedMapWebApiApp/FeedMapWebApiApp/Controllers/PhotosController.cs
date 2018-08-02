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
        public async Task<ActionResult> Get(int id)
        {
            FoodMarkerImageDTO imageMeta = _repoImageMeta.GetFoodMarkerImage(id);

            if (imageMeta == null) return NotFound();

            using (Stream stream = new MemoryStream())
            {
                string contentType = await _repoImageFile.GetFile(imageMeta, stream);
                byte[] buffer = ((MemoryStream)stream).ToArray();
                return File(buffer, contentType, imageMeta.FileName);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetByFoodMarkerId([FromQuery(Name = "foodMarkerId")] int foodMarkerId)
        {
            FoodMarkerImageDTO topImageMeta = _repoImageMeta.GetTopFoodMarkerImageByFoodMarkerId(foodMarkerId);

            if (topImageMeta == null) return NotFound();

            using (Stream stream = new MemoryStream())
            {
                string contentType = await _repoImageFile.GetFile(topImageMeta, stream);
                byte[] buffer = ((MemoryStream)stream).ToArray();
                return File(buffer, contentType, topImageMeta.FileName);
            }
        }
    }
}
