using Foundation;
using System;
using UIKit;

namespace FeedMapApp
{
    public partial class PostFoodMarkerTextView : UITextView
    {
        public PostFoodMarkerTextView (IntPtr handle) : base (handle)
        {
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            this.Layer.BorderColor = UIColor.Yellow.CGColor;
            this.Layer.BorderWidth = 1f;

            this.TextContainerInset = new UIEdgeInsets(top: 8, left: 8, bottom: 8, right: 8);
        }
    }
}