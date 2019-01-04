using Foundation;
using System;
using UIKit;
using MapKit;
using CoreGraphics;
using CoreLocation;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace FeedMapApp.Models
{
    public class FoodMarkerAnnotation : MKAnnotation
    {
        string title;
        CLLocationCoordinate2D coord;
        FoodMarker m_MarkerInfo;

        public FoodMarkerAnnotation(FoodMarker marker)
        {
            m_MarkerInfo = marker;
            title = marker.FoodName;
            string wktcoord = marker.RestaurantPosition;
            var groups = Regex.Match(wktcoord, @"POINT\s*\(\s*(.+)\s+(.+)\)").Groups;
            double lat = Convert.ToDouble(groups[2].Value);
            double lon = Convert.ToDouble(groups[1].Value);
            coord = new CLLocationCoordinate2D(lat, lon);
        }

        public override string Title
        {
            get
            {
                return title;
            }
        }

        public override CLLocationCoordinate2D Coordinate
        {
            get
            {
                return coord;
            }
        }

        public FoodMarker MarkerInfo { get { return m_MarkerInfo; } }

        public string imgFileName { get; set; }
    }
}
