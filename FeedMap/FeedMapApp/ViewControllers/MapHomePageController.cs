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
using FeedMapApp.Helpers;
using FeedMapApp.Services;
using Chafu;

namespace FeedMapApp
{
    public partial class MapHomePageController : UIViewController
    {
        MKMapView MapView;
        private BottomSheetViewController BottomSheetVC { get; set; }
        CLLocationManager LocationManager;
        public MapHomePageController(IntPtr handle) : base(handle)
        {
            LocationManager = new CLLocationManager();
        }

        public override async void ViewDidLoad()
        {
            base.ViewDidLoad();

            AddBottomSheetView();
            AddTapGestureToHomeButton();

            InitMap();

            await AppendToFoodMarkerAnnotations();
        }

        private void InitMap()
        {
            MapView = new MKMapView(View.Bounds);

            MapView.AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;
            MapView.ShowsUserLocation = true;
            LocationManager.RequestWhenInUseAuthorization();

            View.AddSubview(MapView);

            View.SendSubviewToBack(MapView);
            MapView.Delegate = new MapDelegate(BottomSheetVC);
        }

        private async Task AppendToFoodMarkerAnnotations()
        {
            RestService service = new RestService();
            IEnumerable<FoodMarker> foodMarkers = await service.GetAllFoodMarkerPosits();
            AnnotationService annotationService = new AnnotationService();

            foreach (FoodMarker marker in foodMarkers)
            {
                var imageMetas = await service.GetFoodMarkerPhotos(marker.FoodMarkerId);
                marker.FoodMarkerPhotos = imageMetas;
                FoodMarkerAnnotation annotation = annotationService.LoadAnnotations(marker);
                MapView.AddAnnotation(annotation);
            }
        }

        private void AddBottomSheetView()
        {
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


        class MapDelegate : MKMapViewDelegate
        {
            private BottomSheetViewController m_BottomSheet;
            private bool m_HasZoomedToUser = false;

            public MapDelegate(BottomSheetViewController bottomSheet)
            {
                m_BottomSheet = bottomSheet;
            }


            public override MKAnnotationView GetViewForAnnotation(MKMapView mapView, IMKAnnotation annotation)
            {
                MKAnnotationView annotationView = null;

                if (annotation is MKUserLocation)
                    return null;

                if (annotation is FoodMarkerAnnotation)
                {
                    AnnotationService service = new AnnotationService();
                    annotationView = service.GetAnnotationView(mapView, annotation);
                }

                return annotationView;
            }

            public override void DidUpdateUserLocation(MKMapView mapView, MKUserLocation userLocation)
            {
                if (userLocation != null && !m_HasZoomedToUser)
                {
                    CLLocationCoordinate2D coords = mapView.UserLocation.Coordinate;
                    MKCoordinateSpan span = new MKCoordinateSpan(GeoConverter.MilesToLatitudeDegrees(MapSettings.ScreenMapSpan),
                                                                 GeoConverter.MilesToLongitudeDegrees(MapSettings.ScreenMapSpan, coords.Latitude));
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