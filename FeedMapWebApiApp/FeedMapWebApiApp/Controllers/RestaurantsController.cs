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
    public class RestaurantsController : Controller
    {
        private IRestaurantService _service;
        private IMapper _mapper;

        public RestaurantsController(IRestaurantService service,
                                    IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        // GET: api/Restaurants
        [HttpGet]
        public IEnumerable<RestaurantClient> Get()
        {
            IEnumerable<Restaurant> restaurants = _service.GetRestaurants();
            List<RestaurantClient> retLst = new List<RestaurantClient>();

            foreach (var restaurant in restaurants)
            {
                RestaurantClient restaurantRet = _mapper.Map<RestaurantClient>(restaurant);
                retLst.Add(restaurantRet);
            }

            return retLst;
        }

        // GET api/Restaurants/5
        [HttpGet("{id}")]
        public RestaurantClient Get(int id)
        {
            var restaurant = _service.GetRestaurant(id);
            return _mapper.Map<RestaurantClient>(restaurant);
        }

        // POST api/Restaurants
        [HttpPost]
        public int Post([FromBody]RestaurantClient reqObj)
        {
            if (reqObj == null)
            {
                BadRequest();
            }

            Restaurant restaurant = _mapper.Map<Restaurant>(reqObj);
            int id = _service.PostRestaurant(restaurant);
            return id;
        }
    }
}
