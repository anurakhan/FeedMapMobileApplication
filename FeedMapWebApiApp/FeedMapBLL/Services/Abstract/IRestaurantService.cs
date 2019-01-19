using System;
using System.Collections.Generic;
using FeedMapBLL.Domain;

namespace FeedMapBLL.Services.Abstract
{
    public interface IRestaurantService
    {
        IEnumerable<Restaurant> GetRestaurants();

        Restaurant GetRestaurant(int id);

        int PostRestaurant(Restaurant restaurant);
    }
}
