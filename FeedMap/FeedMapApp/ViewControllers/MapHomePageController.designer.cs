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
    [Register ("MapHomePageController")]
    partial class MapHomePageController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        FeedMapApp.HomeButtonController HomeButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        FeedMapApp.SideBarButtonController SideBarButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (HomeButton != null) {
                HomeButton.Dispose ();
                HomeButton = null;
            }

            if (SideBarButton != null) {
                SideBarButton.Dispose ();
                SideBarButton = null;
            }
        }
    }
}