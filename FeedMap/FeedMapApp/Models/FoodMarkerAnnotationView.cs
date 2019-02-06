using System;
using Foundation;
using MapKit;
using UIKit;

namespace FeedMapApp.Models
{
    /// <summary>
    /// Implementation naprigaet.
    /// </summary>
    [Register("FoodMarkerAnnotationView")]
    public class FoodMarkerAnnotationView : MKAnnotationView
    {
        public FoodMarkerAnnotationView()
        {
        }

        public FoodMarkerAnnotationView(NSCoder coder) : base(coder)
        {
        }

        public FoodMarkerAnnotationView(IntPtr handle) : base(handle)
        {
        }

        public FoodMarkerAnnotationView(IMKAnnotation annotation, string identifier)
            : base(annotation, identifier)
        {
        }

        public override IMKAnnotation Annotation
        {
            get
            {
                return base.Annotation;
            }
            set
            {
                base.Annotation = value;
            }
        }

        public void GenView(FoodMarkerAnnotation foodMarkerAnnotation, DirectoryAccess directoryAccess)
        {
            ClusteringIdentifier = "FoodMarker";

            UIImage img;

            byte[] buffer = directoryAccess.GetFile(foodMarkerAnnotation.imgFileName);
            using (NSData data = NSData.FromArray(buffer))
            {
                img = UIImage.LoadFromData(data);
            }
            Image = img;

            Layer.CornerRadius = MapSettings.AnnotationViewCornerRadius;
            Layer.MasksToBounds = MapSettings.AnnotationViewMasksToBound;
        }
    }
}
