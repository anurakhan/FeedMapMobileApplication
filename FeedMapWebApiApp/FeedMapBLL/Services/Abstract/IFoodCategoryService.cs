using System;
using System.Collections.Generic;
using FeedMapBLL.Domain;

namespace FeedMapBLL.Services.Abstract
{
    public interface IFoodCategoryService
    {
        IEnumerable<FoodCategory> GetFoodCategories();

        FoodCategory GetFoodCategory(int id);

        int PostFoodCategory(FoodCategory foodCategory);
    }
}
