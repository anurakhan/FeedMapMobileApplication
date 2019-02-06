using Foundation;
using System;
using UIKit;

namespace FeedMapApp
{
    public partial class PostFoodMarkerPickerView : UIPickerView
    {
        public PostFoodMarkerPickerView (IntPtr handle) : base (handle)
        {
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            this.Layer.BorderColor = UIColor.Yellow.CGColor;
            this.Layer.BorderWidth = 1f;
        }
    }
}