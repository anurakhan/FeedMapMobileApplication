using System;
using FeedMapDAL.Repository.Abstract;
using FeedMapDAL.Repository.Concrete;
using Microsoft.Extensions.Configuration;

namespace FeedMapDAL
{
    public class RepositoryPayload
    {
        private IConfiguration _configuration;

        public RepositoryPayload(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IRestaurantRepository GetRestaurantRepository() => new RestaurantRepository(_configuration);
        public IFoodMarkerRepository GetFoodMarkerRepository() => new FoodMarkerRepository(_configuration);
        public IFoodCategoryRepository GetFoodCategoryRepository() => new FoodCategoryRepository(_configuration);
        public IFoodMarkerImageRepository GetFoodMarkerImageRepository() => new FoodMarkerImageRepository(_configuration);
        public ICompleteFoodDataRepository GetCompleteFoodDataRepository() => new CompleteFoodDataRepository(_configuration);
        public IMediaFileRepository GetFileRepository() => new AzureFileRepository(_configuration);
    }
}
