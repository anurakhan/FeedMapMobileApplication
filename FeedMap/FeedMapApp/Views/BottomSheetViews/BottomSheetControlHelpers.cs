using System;
using CoreGraphics;
using CoreImage;
using UIKit;

namespace FeedMapApp.Views.BottomSheetViews
{
    public static class ControlProps
    {
        public static readonly double Width = 270;

        public static class UIColors
        {
            public static readonly UIColor Black = UIColor.FromRGB(0.14f, 0.14f, 0.14f);
            public static readonly UIColor Gray = UIColor.FromRGB(0.28f, 0.28f, 0.28f);
            public static readonly UIColor Pink = UIColor.FromRGB(252, 183, 254);
            public static readonly UIColor Brown = UIColor.FromRGB(255, 229, 204);
            public static readonly UIColor Yellow = UIColor.FromRGB(255, 255, 204);
            public static readonly UIColor White = UIColor.FromRGB(255, 255, 255);
        }
    }

    public static class LabelSizeCalculator
    {
        public static void SetCenterAndHeightBasedOnText(UIView containingView, UILabel label)
        {
            var size = new CGSize(0, 0);
            if (!string.IsNullOrEmpty(label.Text))
                size = label.Text.StringSize(label.Font,
                    constrainedToSize: new CGSize(label.Frame.Width, 900f),
                    lineBreakMode: UILineBreakMode.WordWrap);
            var labelFrame = label.Frame;
            labelFrame.Size = new CGSize(label.Frame.Width, size.Height);
            label.Frame = labelFrame;
            label.Center = new CGPoint(containingView.Center.X, label.Center.Y);
        }
    }
}
