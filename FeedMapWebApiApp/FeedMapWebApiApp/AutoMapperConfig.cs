using System;
using AutoMapper;
using FeedMapDTO;
using FeedMapBLL.Domain;
using FeedMapWebApiApp.Models;

namespace FeedMapWebApiApp
{
    public class AutoMapperConfig
    {
        MapperConfiguration _config;

        public AutoMapperConfig()
        {
            _config = new MapperConfiguration(cfg =>
            {
                //DTOs to Domain object mapping
                cfg.CreateMap<FoodCategory, FoodCategoryDTO>().ReverseMap();
                cfg.CreateMap<FoodMarker, FoodMarkerDTO>().ReverseMap();
                cfg.CreateMap<FoodMarkerImageData, FoodMarkerImageDataDTO>().ReverseMap();
                cfg.CreateMap<Restaurant, RestaurantDTO>().ReverseMap();
                cfg.CreateMap<User, UserDTO>().ReverseMap();
                cfg.CreateMap<FullFoodAndGeoData, CompleteFoodDataDTO>().ReverseMap();
                cfg.CreateMap<User, UserDTO>();
                cfg.CreateMap<UserDTO, User>()
                .ConstructUsing(s => new User(s.Id,
                                              s.UserName,
                                              s.PasswordHash,
                                              s.PasswordSalt));

                //Domain to WepApi Gateway object mapping
                cfg.CreateMap<FullFoodAndGeoDataClient, FullFoodAndGeoData>().ReverseMap();
                cfg.CreateMap<FoodCategoryClient, FoodCategory>().ReverseMap();
                cfg.CreateMap<FoodMarkerClient, FoodMarker>().ReverseMap();
                cfg.CreateMap<RestaurantClient, Restaurant>().ReverseMap();
                cfg.CreateMap<UserDataClient, User>()
                .ConstructUsing(s => new User(s.UserName, s.HashedPassword));
                cfg.CreateMap<User, UserDataClient>();

            });
        }

        public IMapper GetMapperInstance()
        {
            return _config.CreateMapper();
        }
    }
}
