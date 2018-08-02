using System;
using AutoMapper;
using FeedMapDTO;
using FeedMapBLL.Domain;
using FeedMapWebApiApp.Models;

namespace FeedMapWebApiApp
{
    public class AutoMapperConfig
    {
        public void Initialize()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<FoodCategory, FoodCategoryDTO>();
                cfg.CreateMap<FoodMarker, FoodMarkerDTO>();
                cfg.CreateMap<FoodMarkerImage, FoodMarkerImageDTO>();
                cfg.CreateMap<Restaurant, RestaurantDTO>();

                cfg.CreateMap<FoodCategoryClient, FoodCategory>();
                cfg.CreateMap<FoodMarkerClient, FoodMarker>();
                cfg.CreateMap<RestaurantClient, Restaurant>();

                cfg.CreateMap<CompleteFoodDataDTO, FullFoodAndGeoDataClient>();
            });
        }
    }
}
