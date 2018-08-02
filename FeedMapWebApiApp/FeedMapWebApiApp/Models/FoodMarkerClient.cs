using System;
using System.ComponentModel.DataAnnotations;
namespace FeedMapWebApiApp.Models
{
    public class FoodMarkerClient
    {
        public int Id { get; set; }
        public int FoodCategoryId { get; set; }
        public int RestaurantId { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
    }
}

