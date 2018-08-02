using System;
namespace FeedMapApp
{
    public static class MapSettings
    {
        private static double _screenMapSpan = 5;
        private static nfloat _annotationViewCornerRadius = 20f;
        private static bool _annotationViewMasksToBound = true;
        private static AnnotationViewSize _annotationSize = new AnnotationViewSize(40,40);

        public static AnnotationViewSize AnnotationSize
        {
            get
            {
                return _annotationSize;
            }
        }

        public static double ScreenMapSpan 
        {
            get
            {
                return _screenMapSpan;
            }
        }

        public static nfloat AnnotationViewCornerRadius 
        {
            get
            {
                return _annotationViewCornerRadius;
            }
        }

        public static bool AnnotationViewMasksToBound 
        {
            get
            {
                return _annotationViewMasksToBound;
            }
        }
    }

    public class AnnotationViewSize
    {
        public AnnotationViewSize(float height, float width)
        {
            Height = height;
            Width = width;
        }

        public float Height { get; set; }
        public float Width { get; set; }
    }
}
