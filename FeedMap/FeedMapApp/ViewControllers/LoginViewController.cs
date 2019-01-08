using Foundation;
using System;
using UIKit;
using System.Text.RegularExpressions;

namespace FeedMapApp
{
    public partial class LoginViewController : UIViewController
    {

        public event EventHandler OnLoginSuccess;

        public LoginViewController (IntPtr handle) : base (handle)
        {
        }

		public override void ViewDidLoad()
		{
            base.ViewDidLoad();

            var toolbar = new UIToolbar(new CoreGraphics.CGRect(0.0f, 0.0f, this.View.Frame.Size.Width, 44.0f));
            toolbar.Items = new UIBarButtonItem[]{
                new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
                new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate {
                    View.EndEditing(true);
                })
            };

            TxtUserName.InputAccessoryView = TxtPassword.InputAccessoryView = toolbar;

            var gestureRec = new UITapGestureRecognizer(() => {
                if (IsValidUserNamePassword(TxtUserName, TxtPassword))
                    LoginButtonPressed(TxtUserName, TxtPassword, LoginButton);
                else
                    LoginButton.Image = UIImage.FromBundle("LogInButtonError");
            });
            LoginButton.UserInteractionEnabled = true;
            LoginButton.AddGestureRecognizer(gestureRec);
		}

		public override void ViewWillAppear(bool animated)
		{
            base.ViewWillAppear(animated);
            LoginButton.Image = UIImage.FromBundle("LogInButton");
		}

		private void LoginButtonPressed(UITextField userNameField,
                                        UITextField passwordField,
                                        object sender)
        {
            //set current user procedure goes here

            if (OnLoginSuccess != null)
            {
                OnLoginSuccess(sender, new EventArgs());
            }
        }

        private bool IsValidUserNamePassword(UITextField userNameField,
                                             UITextField passwordField)
        {
            if (String.IsNullOrWhiteSpace(userNameField.Text)) return false;
            if (String.IsNullOrWhiteSpace(passwordField.Text)) return false;
            bool passwordValidator =
                Regex.IsMatch(passwordField.Text,
                              @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=[@$!%*?&]*)[A-Za-z\d@$!%*?&]{8,}$");
            bool userNameValidator = 
                Regex.IsMatch(userNameField.Text,
                              @"^[!-z]*$");

            if (!passwordValidator || !userNameValidator) return false;
            return true;
        }
	}
}