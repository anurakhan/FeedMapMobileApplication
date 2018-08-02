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
    [Register ("BottomSheetViewController")]
    partial class BottomSheetViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIScrollView BottomSheetScrollView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView ContentView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (BottomSheetScrollView != null) {
                BottomSheetScrollView.Dispose ();
                BottomSheetScrollView = null;
            }

            if (ContentView != null) {
                ContentView.Dispose ();
                ContentView = null;
            }
        }
    }
}