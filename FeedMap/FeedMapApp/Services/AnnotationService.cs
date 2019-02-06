using System;
using System.IO;
using System.Threading.Tasks;
using CoreGraphics;
using FeedMapApp.Helpers;
using FeedMapApp.Models;
using Foundation;
using MapKit;
using UIKit;
using System.Linq;
using FeedMapApp.Helpers.DirectoryHelpers;

namespace FeedMapApp.Services
{
    public class AnnotationService
    {
        private DirectoryAccess _directoryAccess;

        public AnnotationService()
        {
            IDirectory directory = new TempDirectory();
            _directoryAccess = new DirectoryAccess(directory);
        }

        public FoodMarkerAnnotation LoadAnnotations(FoodMarker marker)
        {
            FoodMarkerAnnotation annotation = new FoodMarkerAnnotation(marker);

            annotation.imgFileName = marker.FoodMarkerId.ToString();

            UIImage uiImage;
            if (marker.FoodMarkerPhotos != null
                && marker.FoodMarkerPhotos.Where(p => p.ImageRank == 1).Any())
            {
                var iconImage = marker.FoodMarkerPhotos.Where(p => p.ImageRank == 1).First();

                using (var url = new NSUrl(iconImage.ImageUrl))
                {
                    using (var data = NSData.FromUrl(url))
                    {
                        if (data != null)
                            uiImage = UIImage.LoadFromData(data);
                        else
                            uiImage = UIImage.FromBundle("DataBox");
                    }

                }
            } else uiImage = UIImage.FromBundle("DataBox");

            uiImage = uiImage.Scale(new CGSize(MapSettings.AnnotationSize.Width,
                                               MapSettings.AnnotationSize.Height));

            using (NSData data = uiImage.AsPNG())
            {
                byte[] buffer = new byte[data.Length];
                System.Runtime.InteropServices.Marshal.Copy(data.Bytes, buffer, 0, Convert.ToInt32(data.Length));
                _directoryAccess.UploadFile(buffer, annotation.imgFileName);
            }

            return annotation;
        }

        public MKAnnotationView GetAnnotationView(MKMapView mapView, IMKAnnotation annotation)
        {
            FoodMarkerAnnotation marker = annotation as FoodMarkerAnnotation;

            var view = mapView.DequeueReusableAnnotation(MKMapViewDefault.AnnotationViewReuseIdentifier) as FoodMarkerAnnotationView;
            if (view == null)
            {
                view = new FoodMarkerAnnotationView(marker,
                                                    MKMapViewDefault.AnnotationViewReuseIdentifier);
            }
            view.GenView(marker, _directoryAccess);

            return view;
        }

        public MKAnnotationView GetClusterAnnotationView(MKMapView mapView, IMKAnnotation annotation)
        {
            var cluster = annotation as MKClusterAnnotation;

            var view = mapView.DequeueReusableAnnotation(MKMapViewDefault.ClusterAnnotationViewReuseIdentifier) as ClusterAnnotationView;
            if (view == null)
            {
                view = new ClusterAnnotationView(cluster, MKMapViewDefault.ClusterAnnotationViewReuseIdentifier);
            }
            return view;
        }

    }
}
