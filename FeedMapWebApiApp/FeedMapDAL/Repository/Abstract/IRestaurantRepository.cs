using System;
using System.Collections.Generic;
using FeedMapDTO;

namespace FeedMapDAL.Repository.Abstract
{
    public interface IRestaurantRepository
    {
        IEnumerable<RestaurantDTO> GetRestaurants();
        RestaurantDTO GetRestaurant(int id);
        int Post(RestaurantDTO restaurant);
        void Update(RestaurantDTO restaurant, int id);
    }
}
