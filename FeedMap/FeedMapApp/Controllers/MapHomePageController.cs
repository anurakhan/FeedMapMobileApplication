using Foundation;
using System;
using System.IO;
using UIKit;
using MapKit;
using CoreGraphics;
using CoreLocation;
using FeedMapApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FeedMapApp
{
    public partial class MapHomePageController : UIViewController
    {
        MKMapView MapView;
        CLLocationManager LocationManager = new CLLocationManager();
        private static readonly double ViewSpanInMiles = 5;
        BottomSheetViewController BottomSheetVC { get; set; }

        public MapHomePageController(IntPtr handle) : base(handle)
        {
            
        }
        public override async void ViewDidLoad()
        {
            base.ViewDidLoad();

            MapView = new MKMapView(View.Bounds);

            MapView.AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;
            MapView.ShowsUserLocation = true;
            LocationManager.RequestWhenInUseAuthorization();

            View.AddSubview(MapView);

            View.SendSubviewToBack(MapView);

            AddBottomSheetView();
            AddTapGestureToHomeButton();

            MapView.Delegate = new MapDelegate(BottomSheetVC);

            await AppendToFoodMarkerAnnotations();
		}

        private async Task AppendToFoodMarkerAnnotations()
        {
            RestService service = new RestService();
            IEnumerable<FoodMarker> foodMarkers = await service.GetAllFoodMarkerPosits();

            foreach (FoodMarker marker in foodMarkers)
            {
                FoodMarkerAnnotation annotation = new FoodMarkerAnnotation(marker);
                annotation.imgFileName = marker.FoodMarkerId.ToString() + "_Main_Image";

                UIImage uiImage;
                using (Stream stream = await service.GetFoodMarkerPhotos(marker.FoodMarkerId))
                {
                    using (var nsData = NSData.FromStream(stream))
                    {
                        uiImage = UIImage.LoadFromData(nsData);
                    }
                }
                uiImage = uiImage.Scale(new CGSize(40, 40));

                DirectoryAccess dirAccessHelper = new DirectoryAccess(temp:true);

                using (NSData data = uiImage.AsPNG())
                {
                    byte[] buffer = new byte[data.Length];
                    System.Runtime.InteropServices.Marshal.Copy(data.Bytes, buffer, 0, Convert.ToInt32(data.Length));
                    dirAccessHelper.UploadFile(buffer, annotation.imgFileName);
                }

                MapView.AddAnnotation(annotation);
            }
        }

		private void AddBottomSheetView() {
            var bottomSheetVC = new BottomSheetViewController();

            this.AddChildViewController(bottomSheetVC);
            this.View.InsertSubviewBelow(bottomSheetVC.View, HomeButton);
            bottomSheetVC.DidMoveToParentViewController(this);

            var bottomSheetVCHeight = View.Frame.Height;
            var bottomSheetVCWidth = View.Frame.Width;
            bottomSheetVC.View.Frame = new CGRect(0, View.Frame.GetMaxY(), bottomSheetVCWidth, bottomSheetVCHeight);

            BottomSheetVC = bottomSheetVC;
        }

        private void AddTapGestureToHomeButton()
        {
            var tapGesture = new UITapGestureRecognizer(TapHomeButton);
            HomeButton.AddGestureRecognizer(tapGesture);
        }

        public void TapHomeButton(UITapGestureRecognizer tapGesture)
        {
            BottomSheetVC.MoveSheetToBound(1); 
        }

        /// <summary>
        /// Converts miles to latitude degrees
        /// </summary>
        private static double MilesToLatitudeDegrees(double miles)
        {
            double earthRadius = 3960.0;
            double radiansToDegrees = 180.0 / Math.PI;
            return (miles / earthRadius) * radiansToDegrees;
        }

        /// <summary>
        /// Converts miles to longitudinal degrees at a specified latitude
        /// </summary>
        private static double MilesToLongitudeDegrees(double miles, double atLatitude)
        {
            double earthRadius = 3960.0;
            double degreesToRadians = Math.PI / 180.0;
            double radiansToDegrees = 180.0 / Math.PI;

            // derive the earth's radius at that point in latitude
            double radiusAtLatitude = earthRadius * Math.Cos(atLatitude * degreesToRadians);
            return (miles / radiusAtLatitude) * radiansToDegrees;
        }


        class MapDelegate : MKMapViewDelegate
        {
            BottomSheetViewController m_BottomSheet;
            bool m_HasZoomedToUser = false;

            public MapDelegate(BottomSheetViewController bottomSheet)
            {
                m_BottomSheet = bottomSheet;
            }


            string annotationId = "FoodMarkerAnnotation";
            public override MKAnnotationView GetViewForAnnotation(MKMapView mapView, IMKAnnotation annotation)
            {
                MKAnnotationView annotationView = null;

                if (annotation is MKUserLocation)
                    return null;

                if (annotation is FoodMarkerAnnotation)
                {

                    // show conference annotation
                    annotationView = mapView.DequeueReusableAnnotation(annotationId);

                    if (annotationView == null)
                        annotationView = new MKAnnotationView(annotation, annotationId);

                    UIImage img;

                    DirectoryAccess dirAccessHelper = new DirectoryAccess(temp: true);
                    byte[] buffer = dirAccessHelper.GetFile(((FoodMarkerAnnotation)annotation).imgFileName);
                    using (NSData data = NSData.FromArray(buffer))
                    {
                        img = UIImage.LoadFromData(data);  
                    }
                    annotationView.Image = img;
                    annotationView.Layer.CornerRadius = 20f;
                    annotationView.Layer.MasksToBounds = true;
                }

                return annotationView;
            }

            public override void DidUpdateUserLocation(MKMapView mapView, MKUserLocation userLocation)
            {
                if (userLocation != null && !m_HasZoomedToUser)
                {
                    CLLocationCoordinate2D coords = mapView.UserLocation.Coordinate;
                    MKCoordinateSpan span = new MKCoordinateSpan(MilesToLatitudeDegrees(ViewSpanInMiles), MilesToLongitudeDegrees(ViewSpanInMiles, coords.Latitude));
                    mapView.Region = new MKCoordinateRegion(coords, span);
                    m_HasZoomedToUser = true;
                }
            }

			public override void DidSelectAnnotationView(MKMapView mapView, MKAnnotationView view)
			{
                if (view.Annotation is FoodMarkerAnnotation)
                {
                    var annotation = (FoodMarkerAnnotation)view.Annotation;
                    m_BottomSheet.PopulateBottomSheetWithFoodMarkerData(annotation.MarkerInfo);
                }
			}
		}
	}
}