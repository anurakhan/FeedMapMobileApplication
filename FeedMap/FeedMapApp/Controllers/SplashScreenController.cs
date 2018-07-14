using Foundation;
using System;
using UIKit;

namespace FeedMapApp
{
    public partial class SplashScreenController : UIViewController
    {
        public SplashScreenController (IntPtr handle) : base (handle)
        {
        }

		public override void ViewDidAppear(bool animated)
		{
            base.ViewDidAppear(animated);

            PerformSegue("SplashScreenToHomeSegue", this);
		}
	}
}