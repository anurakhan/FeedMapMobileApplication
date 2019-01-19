using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FeedMapWebApiApp.Models;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using FeedMapDTO;
using FeedMapBLL.Domain;
using AutoMapper;
using FeedMapBLL.Services.Abstract;

namespace FeedMapWebApiApp.Controllers
{
    [Route("api/[controller]")]
    public class FoodCategoriesController : Controller
    {
        private IFoodCategoryService _service;
        private IMapper _mapper;

        public FoodCategoriesController(IFoodCategoryService service,
                                        IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }


        // GET: api/FoodCategories
        [HttpGet]
        public IEnumerable<FoodCategoryClient> Get()
        {
            List<FoodCategoryClient> retLst = new List<FoodCategoryClient>();

            var categories = _service.GetFoodCategories();

            foreach (var cat in categories)
            {
                FoodCategoryClient foodCategoryRet = _mapper.Map<FoodCategoryClient>(cat);
                retLst.Add(foodCategoryRet);
            }

            return retLst;
        }

        // GET api/FoodCategories/5
        [HttpGet("{id}")]
        public FoodCategoryClient Get(int id)
        {
            var category = _service.GetFoodCategory(id);
            return _mapper.Map<FoodCategoryClient>(category);
        }

        // POST api/FoodCategories
        [HttpPost]
        public int Post([FromBody]FoodCategoryClient reqObj)
        {
            if (reqObj == null)
            {
                BadRequest();
            }

            FoodCategory foodCategory = _mapper.Map<FoodCategory>(reqObj);
            return _service.PostFoodCategory(foodCategory);
        }

    }
}
