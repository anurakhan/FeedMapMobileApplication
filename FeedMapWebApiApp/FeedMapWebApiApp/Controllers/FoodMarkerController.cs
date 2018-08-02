using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FeedMapWebApiApp.Models;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using FeedMapDAL.Repository.Abstract;
using FeedMapDAL;
using FeedMapDTO;
using FeedMapBLL.Domain;
using AutoMapper;

namespace FeedMapWebApiApp.Controllers
{
    [Route("api/[controller]")]
    public class FoodMarkerController : Controller
    {
        private IFoodMarkerRepository _repo;

        public FoodMarkerController(RepositoryPayload repoPayload)
        {
            _repo = repoPayload.GetFoodMarkerRepository();
        }

        // GET: api/FoodMarker
        [HttpGet]
        public IEnumerable<FoodMarkerClient> Get()
        {
            List<FoodMarkerClient> retLst = new List<FoodMarkerClient>();
            IEnumerable<FoodMarkerDTO> foodMarkersDto = _repo.GetFoodMarkers();
            foreach (FoodMarkerDTO foodMarkerDto in foodMarkersDto)
            {
                FoodMarker foodMarker = Mapper.Map<FoodMarker>(foodMarkerDto);
                FoodMarkerClient foodMarkerRet = Mapper.Map<FoodMarkerClient>(foodMarker);
                retLst.Add(foodMarkerRet);
            }

            return retLst;
        }

        // GET api/FoodMarker/5
        [HttpGet("{id}")]
        public FoodMarkerClient Get(int id)
        {
            FoodMarkerDTO foodMarkerDto = _repo.GetFoodMarker(id);
            FoodMarker foodMarker = Mapper.Map<FoodMarker>(foodMarkerDto);
            return Mapper.Map<FoodMarkerClient>(foodMarker);
        }

        // POST api/FoodMarker
        [HttpPost]
        public int Post([FromBody]FoodMarkerClient reqObj)
        {
            if (reqObj == null)
            {
                BadRequest();
            }

            FoodMarker foodMarker = Mapper.Map<FoodMarker>(reqObj);
            FoodMarkerDTO foodMarkerDTO = Mapper.Map<FoodMarkerDTO>(foodMarker);
            int id = _repo.Post(foodMarkerDTO);

            return id;
        }

    }
}
