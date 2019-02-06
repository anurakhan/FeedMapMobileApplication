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
using FeedMapApp.Services.Navigation;
using Chafu;
using CoreFoundation;

namespace FeedMapApp
{
    //needs refactoring
    public partial class MapHomePageController : UIViewController
    {
        MKMapView MapView;
        private BottomSheetViewController BottomSheetVC { get; set; }
        private SideBarViewController SideBarVC { get; set; }
        private Dictionary<int, IMKAnnotation> _foodMarkerAnnotationDict;
        CLLocationManager LocationManager;

        public MapHomePageController(IntPtr handle) : base(handle)
        {
            LocationManager = new CLLocationManager();
            _foodMarkerAnnotationDict = new Dictionary<int, IMKAnnotation>();
        }

        public override async void ViewDidLoad()
        {
            base.ViewDidLoad();


            AddBottomSheetView();
            this.View.BringSubviewToFront(HomeButton);
            AddSideBarView();
            AddTapGestureToHomeButton();
            AddTapGestureToSideBarButton();


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
            var mapDel = new MapDelegate();
            mapDel.FoodMarkerSelected += OnFoodMarkerSelected;
            MapView.Delegate = mapDel;
            MapView.UserLocation.Title = "";
            RegisterToMapView();
        }

        private void RegisterToMapView()
        {
            MapView.Register(typeof(FoodMarkerAnnotationView), MKMapViewDefault.AnnotationViewReuseIdentifier);
            MapView.Register(typeof(ClusterAnnotationView), MKMapViewDefault.ClusterAnnotationViewReuseIdentifier);
        }

        private void OnFoodMarkerSelected(object sender, EventArgs e)
        {
            var annotation = (FoodMarkerAnnotation)sender;
            BottomSheetVC.PopulateBottomSheetWithFoodMarkerData(annotation.MarkerInfo);
        }


        private async Task AppendToFoodMarkerAnnotations()
        {
            FoodMarkerService foodMarkerService = new FoodMarkerService();
            foodMarkerService.OnFail += OnLogout;   


            IEnumerable<FoodMarker> foodMarkers = await foodMarkerService.GetAllFoodMarkerPositions();
            AnnotationService annotationService = new AnnotationService();

            foreach (FoodMarker marker in foodMarkers)
            {
                var imageMetas = await foodMarkerService.GetFoodMarkerPhotos(marker.FoodMarkerId);
                marker.FoodMarkerPhotos = imageMetas;
                FoodMarkerAnnotation annotation = annotationService.LoadAnnotations(marker);
                MapView.AddAnnotation(annotation);
                _foodMarkerAnnotationDict.Add(marker.FoodMarkerId, annotation);
            }
        }

        private void OnLogout(object sender, EventArgs eventArgs)
        {
            LogoutService logoutService = new LogoutService(
                UIApplication.SharedApplication.Delegate as AppDelegate);
            logoutService.LogoutStart();
        }

        private void AddSideBarView()
        {
            var sideBarVC = new SideBarViewController();

            sideBarVC.OnFoodMarkerCreationSuccess = NewFoodMarkerOnLoad;

            this.AddChildViewController(sideBarVC);
            this.View.InsertSubviewAbove(sideBarVC.View, HomeButton);
            sideBarVC.DidMoveToParentViewController(this);

            var sideBarVCHeight = View.Frame.Height;
            var sideBarVCWidth = View.Frame.Width;
            sideBarVC.View.Frame = new CGRect(-sideBarVCWidth, 0, sideBarVCWidth, sideBarVCHeight);

            SideBarVC = sideBarVC;
        }

        private void AddBottomSheetView()
        {
            var bottomSheetVC = new BottomSheetViewController();

            this.AddChildViewController(bottomSheetVC);
            this.View.InsertSubviewAbove(bottomSheetVC.View, SideBarButton);
            bottomSheetVC.DidMoveToParentViewController(this);

            var bottomSheetVCHeight = View.Frame.Height;
            var bottomSheetVCWidth = View.Frame.Width;
            bottomSheetVC.View.Frame = new CGRect(0, View.Frame.GetMaxY(), bottomSheetVCWidth, bottomSheetVCHeight);

            bottomSheetVC.AddOnDeleteEventHandler(new Func<int, Task>(FoodMarkerOnDelete));

            BottomSheetVC = bottomSheetVC;
        }

        private async Task FoodMarkerOnDelete(int foodMarkerId)
        {
            BottomSheetVC.HideBottomSheet(View.Frame.GetMaxY());

            FoodMarkerService foodMarkerService = new FoodMarkerService();
            bool isSuccess = await foodMarkerService.DeleteFoodMarker(foodMarkerId);

            if (!isSuccess)
            {
                UIImageView errorView = new UIImageView();
                errorView.Image = UIImage.FromBundle("ErrorScreen");
                double side = (View.Frame.Width * 2.0 / 3.0);
                errorView.Frame = new CoreGraphics.CGRect(0, 0, side, side);
                errorView.Center = View.Center;
                View.AddSubview(errorView);
                View.BringSubviewToFront(errorView);
                NSTimer.CreateScheduledTimer(3.0, delegate
                {
                    errorView.RemoveFromSuperview();
                });
                return;
            }
            if (_foodMarkerAnnotationDict.ContainsKey(foodMarkerId))
                MapView.RemoveAnnotation(_foodMarkerAnnotationDict[foodMarkerId]);
        }

        private void NewFoodMarkerOnLoad(object sender, EventArgs e)
        {
            FoodMarker newMarker = (FoodMarker)sender;
            AnnotationService annotationService = new AnnotationService();
            FoodMarkerAnnotation annotation = annotationService.LoadAnnotations(newMarker);
            _foodMarkerAnnotationDict.Add(newMarker.FoodMarkerId, annotation);
            MapView.AddAnnotation(annotation);
        }

        private void AddTapGestureToHomeButton()
        {
            var tapGesture = new UITapGestureRecognizer(TapHomeButton);
            HomeButton.AddGestureRecognizer(tapGesture);
        }

        private void AddTapGestureToSideBarButton()
        {
            var tapGesture = new UITapGestureRecognizer(TapSideBarButton);
            SideBarButton.AddGestureRecognizer(tapGesture);
        }

        private void TapHomeButton(UITapGestureRecognizer tapGesture)
        {
            BottomSheetVC.MoveSheetToBound(1);
        }

        private void TapSideBarButton(UITapGestureRecognizer tapGesture)
        {
            SideBarVC.RevealSideBar(0.5);
        }

        class MapDelegate : MKMapViewDelegate
        {
            public event EventHandler FoodMarkerSelected;
            private bool m_HasZoomedToUser = false;

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
                else if (annotation is MKClusterAnnotation)
                {
                    AnnotationService service = new AnnotationService();
                    annotationView = service.GetClusterAnnotationView(mapView, annotation);
                }
                else if (annotation != null)
                {
                    var unwrappedAnnotation = MKAnnotationWrapperExtensions.UnwrapClusterAnnotation(annotation);

                    return GetViewForAnnotation(mapView, unwrappedAnnotation);
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
                    //might be causing issues.
                    FoodMarkerSelected((object)annotation, new EventArgs());
                }
            }

            private static class MKAnnotationWrapperExtensions
            {
                public static MKClusterAnnotation UnwrapClusterAnnotation(IMKAnnotation annotation)
                {
                    if (annotation == null) return null;
                    return ObjCRuntime.Runtime.GetNSObject(annotation.Handle) as MKClusterAnnotation;
                }
            }
        }
    }
}