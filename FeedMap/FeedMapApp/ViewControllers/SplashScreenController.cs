using FeedMapApp.Services;
using FeedMapApp.Services.Navigation;
using Foundation;
using System;
using System.Threading.Tasks;
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

		public override async void ViewDidAppear(bool animated)
		{
            base.ViewDidAppear(animated);

            var pingService = new TokenPingService();
            IsAuthenticated = await pingService.IsValidToken();

            LoginService loginService = new LoginService(_AppDelegate);

            if (!IsAuthenticated)
                loginService.UserEntryStart();
            else
                loginService.LoginStart();
		}
	}
}