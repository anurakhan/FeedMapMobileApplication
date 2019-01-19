using System;
using System.Collections.Generic;
using AutoMapper;
using FeedMapBLL.Domain;
using FeedMapBLL.Services.Abstract;
using FeedMapDAL;
using FeedMapDAL.Repository.Abstract;
using FeedMapDTO;

namespace FeedMapBLL.Services
{
    public class RestaurantService : IRestaurantService
    {
        private IRestaurantRepository _repo;
        private IMapper _mapper;

        public RestaurantService(RepositoryPayload repoPayload,
                                   IMapper mapper)
        {
            _repo = repoPayload.GetRestaurantRepository();
            _mapper = mapper;
        }

        public Restaurant GetRestaurant(int id)
        {
            RestaurantDTO restaurantDto = _repo.GetRestaurant(id);
            Restaurant restaurant = _mapper.Map<Restaurant>(restaurantDto);
            return restaurant;
        }

        public IEnumerable<Restaurant> GetRestaurants()
        {
            List<Restaurant> retLst = new List<Restaurant>();

            IEnumerable<RestaurantDTO> restaurantsDto = _repo.GetRestaurants();
            foreach (RestaurantDTO restaurantDto in restaurantsDto)
            {
                Restaurant restaurant = Mapper.Map<Restaurant>(restaurantDto);
                retLst.Add(restaurant);
            }

            return retLst;
        }

        public int PostRestaurant(Restaurant restaurant)
        {
            RestaurantDTO restaurantDto = _mapper.Map<RestaurantDTO>(restaurant);
            int id = _repo.Post(restaurantDto);
            return id;
        }
    }
}
