using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FeedMapWebApiApp.Models;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using FeedMapDAL;
using FeedMapDAL.Repository.Abstract;
using FeedMapDTO;
using FeedMapBLL.Domain;
using AutoMapper;

namespace FeedMapWebApiApp.Controllers
{
    [Route("api/[controller]")]
    public class FoodCategoriesController : Controller
    {
        private IFoodCategoryRepository _repo;

        public FoodCategoriesController(RepositoryPayload repoPayload)
        {
            _repo = repoPayload.GetFoodCategoryRepository();
        }


        // GET: api/FoodCategories
        [HttpGet]
        public IEnumerable<FoodCategoryClient> Get()
        {
            List<FoodCategoryClient> retLst = new List<FoodCategoryClient>();

            IEnumerable<FoodCategoryDTO> foodCategoriesDto = _repo.GetFoodCategories();
            foreach (FoodCategoryDTO foodCategoryDto in foodCategoriesDto)
            {
                FoodCategory foodCategory = Mapper.Map<FoodCategory>(foodCategoryDto);
                FoodCategoryClient foodCategoryRet = Mapper.Map<FoodCategoryClient>(foodCategory);
                retLst.Add(foodCategoryRet);
            }

            return retLst;
        }

        // GET api/FoodCategories/5
        [HttpGet("{id}")]
        public FoodCategoryClient Get(int id)
        {
            FoodCategoryDTO foodCategoryDto = _repo.GetFoodCategory(id);
            FoodCategory foodCategory = Mapper.Map<FoodCategory>(foodCategoryDto);
            return Mapper.Map<FoodCategoryClient>(foodCategory);
        }

        // POST api/FoodCategories
        [HttpPost]
        public int Post([FromBody]FoodCategoryClient reqObj)
        {
            if (reqObj == null)
            {
                BadRequest();
            }

            FoodCategory foodCategory = Mapper.Map<FoodCategory>(reqObj);
            FoodCategoryDTO foodCategoryDto = Mapper.Map<FoodCategoryDTO>(foodCategory);
            int id = _repo.Post(foodCategoryDto);

            return id;
        }

    }
}
