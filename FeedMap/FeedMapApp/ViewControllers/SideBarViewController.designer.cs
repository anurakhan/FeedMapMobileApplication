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
    [Register ("SideBarViewController")]
    partial class SideBarViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView BackButtonImg { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView LogOutButtonImg { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (BackButtonImg != null) {
                BackButtonImg.Dispose ();
                BackButtonImg = null;
            }

            if (LogOutButtonImg != null) {
                LogOutButtonImg.Dispose ();
                LogOutButtonImg = null;
            }
        }
    }
}