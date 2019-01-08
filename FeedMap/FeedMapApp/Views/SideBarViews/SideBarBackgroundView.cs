using System;
using CoreGraphics;
using UIKit;

namespace FeedMapApp.Views.SideBarViews
{
    public class SideBarBackgroundView
    {

        UIView _view;

        public SideBarBackgroundView(UIView view)
        {
            _view = view;
        }

        public void Load()
        {
            _view.Layer.ShadowColor = UIColor.Black.CGColor;
            _view.Layer.ShadowOffset = new CGSize(0, -3);
            _view.Layer.ShadowRadius = 3f;
            _view.Layer.ShadowOpacity = 0.2f;
            _view.Layer.MasksToBounds = false;

            var borderView = new UIView();
            borderView.Frame = UIScreen.MainScreen.Bounds;
            borderView.BackgroundColor = UIColor.FromRGB(208, 96, 79).ColorWithAlpha(0.7f);
            borderView.Layer.MasksToBounds = true;
            _view.AddSubview(borderView);
            _view.SendSubviewToBack(borderView);

            var blurEffect = UIBlurEffect.FromStyle(UIBlurEffectStyle.Regular);
            var visualEffect = new UIVisualEffectView(blurEffect);
            visualEffect.Frame = UIScreen.MainScreen.Bounds;
            borderView.AddSubview(visualEffect);
        }
    }
}
