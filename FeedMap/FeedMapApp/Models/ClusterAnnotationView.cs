using System;
using CoreGraphics;
using Foundation;
using MapKit;
using UIKit;

namespace FeedMapApp.Models
{
    /// <summary>
    /// Implementation naprigaet.
    /// </summary>
    [Register("ClusterAnnotationView")]
    public class ClusterAnnotationView : MKAnnotationView
    {
        public override IMKAnnotation Annotation
        {
            get
            {
                return base.Annotation;
            }
            set
            {
                base.Annotation = value;
                GenView(value as MKClusterAnnotation);
            }
        }

        public void GenView(MKClusterAnnotation cluster)
        {
            AnnotationViewSize annotationSize = MapSettings.AnnotationSize;
            if (cluster != null)
            {
                var renderer = new UIGraphicsImageRenderer(new CGSize(annotationSize.Width,
                                                                      annotationSize.Height));
                var count = cluster.MemberAnnotations.Length;

                Image = renderer.CreateImage((context) => {
                    //circle
                    UIColor.FromRGB(230, 141, 119).SetFill();
                    UIBezierPath.FromOval(new CGRect(0,
                                                     0, 
                                                     annotationSize.Width,
                                                     annotationSize.Height)).Fill();

                    //text
                    var attributes = new UIStringAttributes()
                    {
                        ForegroundColor = UIColor.Black,
                        Font = UIFont.BoldSystemFontOfSize(20)
                    };
                    var text = new NSString($"{count}");
                    var size = text.GetSizeUsingAttributes(attributes);
                    var rect = new CGRect(20 - size.Width / 2, 20 - size.Height / 2, size.Width, size.Height);
                    text.DrawString(rect, attributes);
                });
            }
        }

        public ClusterAnnotationView() { }

        public ClusterAnnotationView(NSCoder coder) : base(coder)
        {
        }

        public ClusterAnnotationView(IntPtr handle) : base(handle)
        {
        }

        public ClusterAnnotationView(IMKAnnotation annotation, string reuseIdentifier) : base(annotation, reuseIdentifier)
        {
            DisplayPriority = MKFeatureDisplayPriority.DefaultHigh;
            CollisionMode = MKAnnotationViewCollisionMode.Circle;
            CenterOffset = new CoreGraphics.CGPoint(0, -10);
        }
    }
}
