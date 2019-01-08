// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace FeedMapApp
{
    [Register ("SignupViewController")]
    partial class SignupViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton CancelButtonSignup { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView SignupButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField TxtPasswordConfirm { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField TxtPasswordSignup { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField TxtUserNameSignup { get; set; }

        [Action ("CancelButtonSignup_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void CancelButtonSignup_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (CancelButtonSignup != null) {
                CancelButtonSignup.Dispose ();
                CancelButtonSignup = null;
            }

            if (SignupButton != null) {
                SignupButton.Dispose ();
                SignupButton = null;
            }

            if (TxtPasswordConfirm != null) {
                TxtPasswordConfirm.Dispose ();
                TxtPasswordConfirm = null;
            }

            if (TxtPasswordSignup != null) {
                TxtPasswordSignup.Dispose ();
                TxtPasswordSignup = null;
            }

            if (TxtUserNameSignup != null) {
                TxtUserNameSignup.Dispose ();
                TxtUserNameSignup = null;
            }
        }
    }
}