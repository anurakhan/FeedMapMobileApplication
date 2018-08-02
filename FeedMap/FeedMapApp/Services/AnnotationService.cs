using System;
using System.IO;
using System.Threading.Tasks;
using CoreGraphics;
using FeedMapApp.Helpers;
using FeedMapApp.Models;
using Foundation;
using MapKit;
using UIKit;

namespace FeedMapApp.Services
{
    public class AnnotationService
    {
        private string _annotationId = "FoodMarkerAnnotation";

        public async Task<FoodMarkerAnnotation> LoadAnnotations(FoodMarker marker)
        {
            RestService service = new RestService();

            FoodMarkerAnnotation annotation = new FoodMarkerAnnotation(marker);

            annotation.imgFileName = marker.FoodMarkerId.ToString();

            UIImage uiImage;
            using (Stream stream = await service.GetFoodMarkerPhotos(marker.FoodMarkerId))
            {
                using (var nsData = NSData.FromStream(stream))
                {
                    uiImage = UIImage.LoadFromData(nsData);
                }
            }
            uiImage = uiImage.Scale(new CGSize(MapSettings.AnnotationSize.Width, 
                                               MapSettings.AnnotationSize.Height));

            DirectoryAccess dirAccessHelper = new DirectoryAccess(temp: true);

            using (NSData data = uiImage.AsPNG())
            {
                byte[] buffer = new byte[data.Length];
                System.Runtime.InteropServices.Marshal.Copy(data.Bytes, buffer, 0, Convert.ToInt32(data.Length));
                dirAccessHelper.UploadFile(buffer, annotation.imgFileName);
            }

            return annotation;
        }

        public MKAnnotationView GetAnnotationView(MKMapView mapView, IMKAnnotation annotation)
        {
            MKAnnotationView annotationView = mapView.DequeueReusableAnnotation(_annotationId);

            if (annotationView == null)
                annotationView = new MKAnnotationView(annotation, _annotationId);

            UIImage img;

            DirectoryAccess dirAccessHelper = new DirectoryAccess(temp: true);
            byte[] buffer = dirAccessHelper.GetFile(((FoodMarkerAnnotation)annotation).imgFileName);
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
