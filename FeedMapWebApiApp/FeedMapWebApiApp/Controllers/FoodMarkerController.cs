using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FeedMapWebApiApp.Models;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using FeedMapDTO;
using FeedMapBLL.Domain;
using AutoMapper;
using FeedMapBLL.Services.Abstract;

namespace FeedMapWebApiApp.Controllers
{
    [Route("api/[controller]")]
    public class FoodMarkerController : Controller
    {
        private IFoodMarkerService _service;
        private IMapper _mapper;

        public FoodMarkerController(IFoodMarkerService service,
                                    IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        // GET: api/FoodMarker
        [HttpGet]
        public IEnumerable<FoodMarkerClient> Get()
        {
            var markers = _service.GetFoodMarkers();

            List<FoodMarkerClient> retLst = new List<FoodMarkerClient>();
            foreach (var marker in markers)
            {
                FoodMarkerClient foodMarkerRet = _mapper.Map<FoodMarkerClient>(marker);
                retLst.Add(foodMarkerRet);
            }

            return retLst;
        }

        // GET api/FoodMarker/5
        [HttpGet("{id}")]
        public FoodMarkerClient Get(int id)
        {
            FoodMarker foodMarker = _service.GetFoodMarker(id);
            return _mapper.Map<FoodMarkerClient>(foodMarker);
        }

        // POST api/FoodMarker
        [HttpPost]
        public int Post([FromBody]FoodMarkerClient reqObj)
        {
            if (reqObj == null)
            {
                BadRequest();
            }

            FoodMarker foodMarker = _mapper.Map<FoodMarker>(reqObj);
            return _service.PostFoodMarker(foodMarker);
        }

    }
}
