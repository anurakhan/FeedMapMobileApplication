using System;
namespace FeedMapApp.Models
{
    public class FoodMarkerValidator
    {
        private FoodMarker _foodMarker;
        public FoodMarkerValidator(FoodMarker foodMarker)
        {
            _foodMarker = foodMarker;
        }

        public bool Validate()
        {
            if (String.IsNullOrWhiteSpace(_foodMarker.FoodName)) return false;

            if (String.IsNullOrWhiteSpace(_foodMarker.CategoryName)) return false;

            if (String.IsNullOrWhiteSpace(_foodMarker.RestaurantAddress)) return false;

            if (String.IsNullOrWhiteSpace(_foodMarker.RestaurantName)) return false;

            if (String.IsNullOrWhiteSpace(_foodMarker.RestaurantPosition)) return false;

            if (_foodMarker.Rating > 5 || _foodMarker.Rating < 1) return false;

            return true;
        }
    }
}
