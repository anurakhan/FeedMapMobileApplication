using FeedMapApp.Models;
using FeedMapApp.Services;
using Foundation;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UIKit;

namespace FeedMapApp
{
    public partial class SignupViewController : UIViewController
    {
        public SignupViewController (IntPtr handle) : base (handle)
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

            TxtUserNameSignup.InputAccessoryView = 
                TxtPasswordSignup.InputAccessoryView =
                    TxtPasswordConfirm.InputAccessoryView = toolbar;

            var gestureRec = new UITapGestureRecognizer(async () => {
                if (!IsValidUserNamePassword(TxtUserNameSignup,
                                            TxtPasswordSignup,
                                            TxtPasswordConfirm) ||
                    !(await SignUpButtonPressed(TxtUserNameSignup, TxtPasswordSignup)))
                {
                    SignupButton.Image = UIImage.FromBundle("SignUpButtonError");
                }
            });
            SignupButton.UserInteractionEnabled = true;
            SignupButton.AddGestureRecognizer(gestureRec);
		}

        private bool IsValidUserNamePassword(UITextField userNameField,
                                             UITextField passwordField,
                                             UITextField confirmPasswordField)
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
            if (passwordField.Text != confirmPasswordField.Text) return false;

            return true;
        }

        private async Task<bool> SignUpButtonPressed(UITextField userNameField,
                                         UITextField passwordField)
        {
            //Creating User Procedure Goes here
            UserAuthService authService = new UserAuthService(new UserData(userNameField.Text,
                                                                           passwordField.Text));
            bool isSuccess = await authService.SignUp();
            if (!isSuccess) return false;

            DismissViewController(true, null);
            return true;
        }

		partial void CancelButtonSignup_TouchUpInside(UIButton sender)
        {
            DismissViewController(true, null);
        }
    }
}