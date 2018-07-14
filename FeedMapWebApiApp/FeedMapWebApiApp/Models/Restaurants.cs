using System;
namespace FeedMapWebApiApp.Models
{
    public class Restaurants
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string Address { get; set; }
    }

    public class PostRestaurant
    {
        public string Name { get; set; }
        public string Position { get; set; }
        public string Address { get; set; }
    }
}

