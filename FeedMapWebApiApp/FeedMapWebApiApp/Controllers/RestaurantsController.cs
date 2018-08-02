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
    public class RestaurantsController : Controller
    {
        private IRestaurantRepository _repo;

        public RestaurantsController(RepositoryPayload repoPayload)
        {
            _repo = repoPayload.GetRestaurantRepository();
        }

        // GET: api/Restaurants
        [HttpGet]
        public IEnumerable<RestaurantClient> Get()
        {
            List<RestaurantClient> retLst = new List<RestaurantClient>();

            IEnumerable<RestaurantDTO> restaurantsDto = _repo.GetRestaurants();
            foreach (RestaurantDTO restaurantDto in restaurantsDto)
            {
                Restaurant restaurant = Mapper.Map<Restaurant>(restaurantDto);
                RestaurantClient restaurantRet = Mapper.Map<RestaurantClient>(restaurant);
                retLst.Add(restaurantRet);
            }

            return retLst;
        }

        // GET api/Restaurants/5
        [HttpGet("{id}")]
        public RestaurantClient Get(int id)
        {
            RestaurantDTO restaurantDto = _repo.GetRestaurant(id);
            Restaurant restaurant = Mapper.Map<Restaurant>(restaurantDto);
            return Mapper.Map<RestaurantClient>(restaurant);
        }

        // POST api/Restaurants
        [HttpPost]
        public int Post([FromBody]RestaurantClient reqObj)
        {
            if (reqObj == null)
            {
                BadRequest();
            }

            Restaurant restaurant = Mapper.Map<Restaurant>(reqObj);
            RestaurantDTO restaurantDto = Mapper.Map<RestaurantDTO>(restaurant);
            int id = _repo.Post(restaurantDto);

            return id;
        }
    }
}
