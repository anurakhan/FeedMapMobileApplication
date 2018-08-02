using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FeedMapWebApiApp.Models;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using FeedMapDAL;
using FeedMapDAL.Repository.Abstract;
using FeedMapDTO;
using FeedMapBLL.Domain;
using AutoMapper;

namespace FeedMapWebApiApp.Controllers
{
    [Route("api/[controller]")]
    public class FullFoodAndGeoDataController : Controller
    {
        private ICompleteFoodDataRepository _repo;

        public FullFoodAndGeoDataController(RepositoryPayload repoPayload)
        {
            _repo = repoPayload.GetCompleteFoodDataRepository();
        }

        // GET: api/FullFoodAndGeoData
        [HttpGet]
        public IEnumerable<FullFoodAndGeoDataClient> Get()
        {
            List<FullFoodAndGeoDataClient> retLst = new List<FullFoodAndGeoDataClient>();

            IEnumerable<CompleteFoodDataDTO> completeFoodDatasDto = _repo.GetCompleteFoodDatas();
            foreach (CompleteFoodDataDTO completeFoodDataDto in completeFoodDatasDto)
            {
                retLst.Add(Mapper.Map<FullFoodAndGeoDataClient>(completeFoodDataDto));    
            }
            return retLst;
        }

        // GET api/FullFoodAndGeoData/5
        [HttpGet("{id}")]
        public FullFoodAndGeoDataClient Get(int id)
        {
            CompleteFoodDataDTO completeFoodDataDto = _repo.GetCompleteFoodData(id);
            return Mapper.Map<FullFoodAndGeoDataClient>(completeFoodDataDto);
        }
    }
}
