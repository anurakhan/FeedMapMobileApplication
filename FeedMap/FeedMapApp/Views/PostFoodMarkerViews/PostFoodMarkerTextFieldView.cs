using Foundation;
using System;
using UIKit;

namespace FeedMapApp
{
    public partial class PostFoodMarkerTextFieldView : UITextField
    {
        public PostFoodMarkerTextFieldView (IntPtr handle) : base (handle)
        {
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            this.Layer.BorderColor = UIColor.Yellow.CGColor;
            this.Layer.BorderWidth = 1f;

            this.AttributedPlaceholder = new NSAttributedString(Placeholder, 
                                                                null,
                                                                UIColor.Yellow);
        }
    }
}