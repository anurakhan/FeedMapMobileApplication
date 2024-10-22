﻿using System;
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
    public class FullFoodAndGeoDataController : Controller
    {
        private IFoodMarkerService _service;
        private IMapper _mapper;

        public FullFoodAndGeoDataController(IFoodMarkerService service,
                                            IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        // GET: api/FullFoodAndGeoData
        [HttpGet]
        public IEnumerable<FullFoodAndGeoDataClient> Get()
        {
            List<FullFoodAndGeoDataClient> retLst = new List<FullFoodAndGeoDataClient>();

            int id = (int)RouteData.Values["UserId"];

            var compeleteFoodDatas = _service.GetCompleteFoodDataByUserId(id);
            if (compeleteFoodDatas != null)
            {
                foreach (var foodData in compeleteFoodDatas)
                {
                    retLst.Add(_mapper.Map<FullFoodAndGeoDataClient>(foodData));
                }
            }
            return retLst;
        }

        // GET api/FullFoodAndGeoData/5
        [HttpGet("{id}")]
        public FullFoodAndGeoDataClient Get(int id)
        {
            return _mapper.Map<FullFoodAndGeoDataClient>( 
                _service.GetCompleteFoodDataById(id));
        }

        [HttpPost]
        public PostedFoodMarkerRetObj Post([FromBody]FullFoodAndGeoDataClient reqObj)
        {
            if (reqObj == null)
            {
                BadRequest();
            }

            int id = (int)RouteData.Values["UserId"];
            User user = new User(){Id = id};

            FullFoodAndGeoData model = _mapper.Map<FullFoodAndGeoData>(reqObj);
            return new PostedFoodMarkerRetObj { Id = _service.PostFullFoodAndGeoData(model, user) };
        }
    }
}
