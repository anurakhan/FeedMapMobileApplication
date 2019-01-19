using System;
namespace FeedMapApp.Services.Navigation
{
    public class LogoutService
    {
        private AppDelegate _appDelegate;

        public LogoutService(AppDelegate appDelegate)
        {
            _appDelegate = appDelegate;
        }

        public void LogoutStart()
        {
            var appDelegate = _appDelegate;

            var mainStoryboard = appDelegate.MainStoryboard;

            var loginViewController =
                appDelegate.GetViewController(appDelegate.MainStoryboard, "LoginViewController") as LoginViewController;

            loginViewController.OnLoginSuccess += (s, e) =>
            {
                var mapHomePageController = appDelegate.GetViewController(mainStoryboard, "MapHomePageController");
                appDelegate.SetRootViewController(mapHomePageController, true);
            };

            appDelegate.SetRootViewController(loginViewController, true);
        }
    }
}
