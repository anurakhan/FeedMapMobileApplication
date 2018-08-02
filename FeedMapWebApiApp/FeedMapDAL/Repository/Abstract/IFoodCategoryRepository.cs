using System;
using System.Collections.Generic;
using FeedMapDTO;

namespace FeedMapDAL.Repository.Abstract
{
    public interface IFoodCategoryRepository
    {
        IEnumerable<FoodCategoryDTO> GetFoodCategories();
        FoodCategoryDTO GetFoodCategory(int id);
        int Post(FoodCategoryDTO foodCategory);
        void Update(FoodCategoryDTO foodCategory, int id);
    }
}
