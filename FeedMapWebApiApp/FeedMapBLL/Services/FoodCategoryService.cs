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
    public class FoodCategoryService : IFoodCategoryService
    {
        private IFoodCategoryRepository _repo;
        private IMapper _mapper;

        public FoodCategoryService(RepositoryPayload repoPayload,
                                   IMapper mapper)
        {
            _repo = repoPayload.GetFoodCategoryRepository();
            _mapper = mapper;
        }

        public IEnumerable<FoodCategory> GetFoodCategories()
        {
            List<FoodCategory> retLst = new List<FoodCategory>();

            IEnumerable<FoodCategoryDTO> foodCategoriesDto = _repo.GetFoodCategories();
            foreach (FoodCategoryDTO foodCategoryDto in foodCategoriesDto)
            {
                FoodCategory foodCategory = _mapper.Map<FoodCategory>(foodCategoryDto);
                retLst.Add(foodCategory);
            }

            return retLst;
        }

        public FoodCategory GetFoodCategory(int id)
        {
            FoodCategoryDTO foodCategoryDto = _repo.GetFoodCategory(id);
            FoodCategory foodCategory = _mapper.Map<FoodCategory>(foodCategoryDto);
            return foodCategory;
        }

        public int PostFoodCategory(FoodCategory foodCategory)
        {
            FoodCategoryDTO foodCategoryDto = Mapper.Map<FoodCategoryDTO>(foodCategory);
            int id = _repo.Post(foodCategoryDto);
            return id;
        }
    }
}
