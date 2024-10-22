﻿using System;
using System.Collections.Generic;

namespace FeedMapApp.Models
{
    public class FoodMarker
    {
        public int FoodMarkerId { get; set; }
        public string FoodName { get; set; }
        public string RestaurantName { get; set; }
        public string RestaurantPosition { get; set; }
        public string RestaurantAddress { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
        public string CategoryName { get; set; }
        public IEnumerable<FoodMarkerImageMeta> FoodMarkerPhotos { get; set; }
    }
}
