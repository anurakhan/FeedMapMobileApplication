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
        private string _annotationId = "FoodMarkerAnnotation";
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

            var iconImage = marker.FoodMarkerPhotos.Where(p => p.ImageRank == 1).First();

            using (var url = new NSUrl(iconImage.ImageUrl))
            {
                using (var data = NSData.FromUrl(url))
                {
                    uiImage = UIImage.LoadFromData(data);
                }

            }

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
            MKAnnotationView annotationView = mapView.DequeueReusableAnnotation(_annotationId);

            if (annotationView == null)
                annotationView = new MKAnnotationView(annotation, _annotationId);

            UIImage img;

            byte[] buffer = _directoryAccess.GetFile(((FoodMarkerAnnotation)annotation).imgFileName);
            using (NSData data = NSData.FromArray(buffer))
            {
                img = UIImage.LoadFromData(data);
            }
            annotationView.Image = img;
            annotationView.Layer.CornerRadius = MapSettings.AnnotationViewCornerRadius;
            annotationView.Layer.MasksToBounds = MapSettings.AnnotationViewMasksToBound;

            return annotationView;
        }
    }
}
