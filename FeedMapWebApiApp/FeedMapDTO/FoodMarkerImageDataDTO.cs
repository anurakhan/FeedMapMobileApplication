﻿using System;
namespace FeedMapDTO
{
    public class FoodMarkerImageDataDTO
    {
        public int Id { get; set; }
        public int FoodMarkerId { get; set; }
        public int? ImageRank { get; set; }
        public string FileName { get; set; }
        public string ClientFileName { get; set; }
    }
}