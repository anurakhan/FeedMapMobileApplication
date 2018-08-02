using System;
namespace FeedMapBLL.Domain
{
    public class FoodMarker
    {
        public int Id { get; set; }
        public int FoodCategoryId { get; set; }
        public int RestaurantId { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }

        public FoodMarker()
        {}
    }
}
