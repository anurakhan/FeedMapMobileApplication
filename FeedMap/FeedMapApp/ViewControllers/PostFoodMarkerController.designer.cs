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
    [Register ("PostFoodMarkerController")]
    partial class PostFoodMarkerController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView AddMediaBtn { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        FeedMapApp.PostFoodMarkerTextFieldView PostFoodMarkerCategory { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView PostFoodMarkerCloseBtn { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        FeedMapApp.PostFoodMarkerTextView PostFoodMarkerComment { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        FeedMapApp.PostFoodMarkerTextFieldView PostFoodMarkerName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView PostFoodMarkerOkButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        FeedMapApp.PostFoodMarkerTextFieldView PostFoodMarkerRating { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        FeedMapApp.PostFoodMarkerTextFieldView PostFoodMarkerRestaurant { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView SeeMediaBtn { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (AddMediaBtn != null) {
                AddMediaBtn.Dispose ();
                AddMediaBtn = null;
            }

            if (PostFoodMarkerCategory != null) {
                PostFoodMarkerCategory.Dispose ();
                PostFoodMarkerCategory = null;
            }

            if (PostFoodMarkerCloseBtn != null) {
                PostFoodMarkerCloseBtn.Dispose ();
                PostFoodMarkerCloseBtn = null;
            }

            if (PostFoodMarkerComment != null) {
                PostFoodMarkerComment.Dispose ();
                PostFoodMarkerComment = null;
            }

            if (PostFoodMarkerName != null) {
                PostFoodMarkerName.Dispose ();
                PostFoodMarkerName = null;
            }

            if (PostFoodMarkerOkButton != null) {
                PostFoodMarkerOkButton.Dispose ();
                PostFoodMarkerOkButton = null;
            }

            if (PostFoodMarkerRating != null) {
                PostFoodMarkerRating.Dispose ();
                PostFoodMarkerRating = null;
            }

            if (PostFoodMarkerRestaurant != null) {
                PostFoodMarkerRestaurant.Dispose ();
                PostFoodMarkerRestaurant = null;
            }

            if (SeeMediaBtn != null) {
                SeeMediaBtn.Dispose ();
                SeeMediaBtn = null;
            }
        }
    }
}