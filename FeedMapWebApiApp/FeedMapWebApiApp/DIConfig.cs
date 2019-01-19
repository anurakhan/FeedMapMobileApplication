using System;
using FeedMapBLL.Services;
using FeedMapBLL.Services.Abstract;
using FeedMapDAL;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FeedMapWebApiApp
{
    public class DIConfig
    {
        private IConfiguration _configuration { get; }
        private AutoMapperConfig _mapperConfig;

        public DIConfig(IConfiguration  configuration,
                        AutoMapperConfig mapperConfig)
        {
            _configuration = configuration;
            _mapperConfig = mapperConfig;
        }
        /// <summary>
        /// Replace with scanning Dependency Injection.
        /// </summary>
        /// <param name="services">Services.</param>
        public void ImplDI(IServiceCollection services)
        {
            //Enable AutoMapper with AutoMapperCongif Obj for configuration.
            var mapConfig = _mapperConfig;

            //Add TokenAuth Filter Globaly for all actions.
            services.AddMvc(options => options.Filters.Add(typeof(TokenAuthorizeFilter)));

            services.AddSingleton(mapConfig.GetMapperInstance());

            //Dependency Inject configuration obj to repositories.
            services.AddTransient<RepositoryPayload>(provider => new RepositoryPayload(_configuration));

            //Dependency Inject Token Manager Singleton.
            services.AddSingleton(TokenManagerSingleton.GetInstance(
                Convert.ToInt32(_configuration["TokenExpiration:Mins"])));

            //Dependency Inject Business Services.
            services.AddTransient<IFoodCategoryService, FoodCategoryService>();
            services.AddTransient<IFoodMarkerService, FoodMarkerService>();
            services.AddTransient<IPhotoService, PhotoService>();
            services.AddTransient<IRestaurantService, RestaurantService>();
            services.AddTransient<IUserService, UserService>();
        }
    }
}
