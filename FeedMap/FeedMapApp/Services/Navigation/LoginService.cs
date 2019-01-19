using System;
namespace FeedMapApp.Services.Navigation
{
    public class LoginService
    {
        private AppDelegate _appDelegate;

        public LoginService(AppDelegate appDelegate)
        {
            _appDelegate = appDelegate;
        }

        public void UserEntryStart()
        {
            var loginViewController =
                _appDelegate.GetViewController(_appDelegate.MainStoryboard, "LoginViewController") as LoginViewController;
            loginViewController.OnLoginSuccess += LoginViewController_OnLoginSuccess;
            _appDelegate.SetRootViewController(loginViewController, true);
        }

        private void LoginViewController_OnLoginSuccess(object sender, EventArgs e)
        {
            LoginStart();
        }

        public void LoginStart()
        {
            var mapHomePageController = _appDelegate.GetViewController(_appDelegate.MainStoryboard, "MapHomePageController");
            _appDelegate.SetRootViewController(mapHomePageController, true);
        }

    }
}
