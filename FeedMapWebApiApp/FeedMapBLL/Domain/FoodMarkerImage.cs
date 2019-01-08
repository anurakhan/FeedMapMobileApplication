using System;
using FeedMapBLL.Helpers;

namespace FeedMapBLL.Domain
{
    public class FoodMarkerImage
    {
        public int Id { get; set; }
        public int FoodMarkerId { get; set; }
        public string FileName { get; set; }
        public int? ImageRank { get; set; }
        public string ClientFileName { get; set; }

        public FoodMarkerImage(int _foodMarkerId, string _fileName)
        {
            FoodMarkerId = _foodMarkerId;
            ImageFileNameConverter conv = new ImageFileNameConverter();
            ClientFileName = conv.ClientFileNameConvert(_fileName);
            FileName = conv.FileNameGenerate();
        }
    }
}
