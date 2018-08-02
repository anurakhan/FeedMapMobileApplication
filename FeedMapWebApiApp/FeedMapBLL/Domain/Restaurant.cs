using System;
namespace FeedMapBLL.Domain
{
    public class Restaurant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string Address { get; set; }

        public Restaurant()
        {
        }
    }
}
