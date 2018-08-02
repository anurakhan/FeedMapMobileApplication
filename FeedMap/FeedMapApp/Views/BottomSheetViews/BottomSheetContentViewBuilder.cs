using System;
using CoreGraphics;
using UIKit;

namespace FeedMapApp.Views.BottomSheetViews
{
    public class BottomSheetContentViewBuilder
    {
        private UIView _contentView;
        public nfloat Width { get; set; }
        public nfloat Height { get; set; }
        public nfloat X { get; set; }
        public nfloat Y { get; set; }

        public BottomSheetContentViewBuilder(UIView contentView)
        {
            _contentView = contentView;
        }

        public void Load()
        {
            _contentView.Frame = new CGRect(X, Y, Width, Height);
        }
    }
}
