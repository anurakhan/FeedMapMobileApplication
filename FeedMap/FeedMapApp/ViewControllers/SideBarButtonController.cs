using CoreGraphics;
using Foundation;
using System;
using UIKit;

namespace FeedMapApp
{
    public partial class SideBarButtonController : UIImageView
    {
        public SideBarButtonController (IntPtr handle) : base (handle)
        {
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            //this.Layer.ZPosition = 1;
            this.Layer.ShadowColor = UIColor.Black.CGColor;
            this.Layer.ShadowOffset = new CGSize(0, 2);
            this.Layer.ShadowRadius = 2f;
            this.Layer.ShadowOpacity = 0.3f;
            this.Layer.MasksToBounds = false;
        }
    }
}