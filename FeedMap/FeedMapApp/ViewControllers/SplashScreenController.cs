using Foundation;
using System;
using UIKit;

namespace FeedMapApp
{
    public partial class SplashScreenController : UIViewController
    {
        private AppDelegate _AppDelegate
        {
            get { return UIApplication.SharedApplication.Delegate as AppDelegate; }
        }

        private bool IsAuthenticated { get; set; } = false;

        public SplashScreenController (IntPtr handle) : base (handle)
        {
        }

		public override void ViewDidAppear(bool animated)
		{
            base.ViewDidAppear(animated);

            if (!IsAuthenticated)
            {
                var loginViewController =
                    _AppDelegate.GetViewController(_AppDelegate.MainStoryboard, "LoginViewController") as LoginViewController;
                loginViewController.OnLoginSuccess += LoginViewController_OnLoginSuccess;
                _AppDelegate.SetRootViewController(loginViewController, true);
            }
            else
            {
                var mapHomePageController = _AppDelegate.GetViewController(_AppDelegate.MainStoryboard, "MapHomePageController");
                _AppDelegate.SetRootViewController(mapHomePageController, true);
            }
		}

        void LoginViewController_OnLoginSuccess(object sender, EventArgs e)
        {
            var mapHomePageController = _AppDelegate.GetViewController(_AppDelegate.MainStoryboard, "MapHomePageController");
            _AppDelegate.SetRootViewController(mapHomePageController, true);

        }
	}
}