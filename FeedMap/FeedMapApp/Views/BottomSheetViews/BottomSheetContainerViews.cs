using System;
using System.Collections.Generic;
using CoreGraphics;
using CoreImage;
using FeedMapApp.Models;
using Syncfusion.SfRating.iOS;
using UIKit;

namespace FeedMapApp.Views.BottomSheetViews
{
    public interface IBottomSheetContainerView
    {
        UIView ContainingView { get; set; }
        UIView View { get; set; }
        void Load(FoodMarker marker);
        void Init();
    }

    public class BottomSheetTemplateContainerView : IBottomSheetContainerView
    {
        public UIView ContainingView { get; set; }
        public UIView View { get; set; }

        public BottomSheetTemplateContainerView(UIView containingView)
        {
            ContainingView = containingView;
        }

        public void Init()
        {
            View = new UIView();   
        }

        public void Load(FoodMarker marker)
        {
            View.Frame = new CGRect(0, 0, ContainingView.Frame.Width, 0);
        }
    }

    public class BottomSheetFromContainerView : IBottomSheetContainerView
    {
        private IBottomSheetContainerView _parentContainer;
        private UILabel _fromText;

        public UIView ContainingView { get; set; }
        public UIView View { get; set; }

        public BottomSheetFromContainerView(IBottomSheetContainerView parentContainer)
        {
            _parentContainer = parentContainer;
            ContainingView = parentContainer.ContainingView;
        }

        public void Init()
        {
            _fromText = new UILabel();
            View = new UIView();
            View.AddSubview(_fromText);
            ContainingView.AddSubview(View);
        }

        public void Load(FoodMarker marker)
        {
            _fromText.Text = "from";
            _fromText.TextColor = new UIColor(ControlProps.CIColors.Black);
            _fromText.Font = UIFont.FromName("Helvetica-BoldOblique", 12f);
            _fromText.TextAlignment = UITextAlignment.Center;
            _fromText.LineBreakMode = UILineBreakMode.WordWrap;
            _fromText.Lines = 0;
            _fromText.Frame = new CGRect(
                0,
                0,
                ControlProps.Width,
                0
            );
            LabelSizeCalculator.SetCenterAndHeightBasedOnText(ContainingView, _fromText);

            View.Frame = new CGRect(
                0,
                _parentContainer.View.Frame.GetMaxY(),
                ContainingView.Frame.Width,
                _fromText.Frame.Height + 10f
            );
        }
    }

    public class BottomSheetRestaurantContainerView : IBottomSheetContainerView
    {
        private IBottomSheetContainerView _parentContainer;
        private UILabel _restaurantName;

        public UIView ContainingView { get; set; }
        public UIView View { get; set; }

        public BottomSheetRestaurantContainerView(IBottomSheetContainerView parentContainter)
        {
            _parentContainer = parentContainter;
            ContainingView = _parentContainer.ContainingView;
        }

        public void Init()
        {
            _restaurantName = new UILabel();
            View = new UIView();
            View.AddSubview(_restaurantName);
            ContainingView.AddSubview(View);
        }

        public void Load(FoodMarker marker)
        {
            _restaurantName.Text = marker.RestaurantName;
            _restaurantName.TextColor = new UIColor(ControlProps.CIColors.Black);
            _restaurantName.Font = UIFont.FromName("Menlo-BoldItalic", 16f);
            _restaurantName.TextAlignment = UITextAlignment.Center;
            _restaurantName.LineBreakMode = UILineBreakMode.WordWrap;
            _restaurantName.Lines = 0;
            _restaurantName.Frame = new CGRect(
                0,
                0,
                ControlProps.Width,
                0
            );
            LabelSizeCalculator.SetCenterAndHeightBasedOnText(ContainingView, _restaurantName);

            View.Frame = new CGRect(
                0,
                _parentContainer.View.Frame.GetMaxY(),
                ContainingView.Frame.Width,
                _restaurantName.Frame.Height + 10f
            );
        }
    }

    public class BottomSheetRatingContainerView : IBottomSheetContainerView
    {
        private IBottomSheetContainerView _parentContainer;
        private SfRating _rating;

        public UIView ContainingView { get; set; }
        public UIView View { get; set; }

        public BottomSheetRatingContainerView(IBottomSheetContainerView parentContainter)
        {
            _parentContainer = parentContainter;
            ContainingView = _parentContainer.ContainingView;
        }

        public void Init()
        {
            _rating = new SfRating();
            View = new UIView();
            View.AddSubview(_rating);
            ContainingView.AddSubview(View);
        }

        public void Load(FoodMarker marker)
        {
            _rating.ItemCount = 5;
            _rating.Precision = SFRatingPrecision.Standard;
            _rating.TooltipPlacement = SFRatingTooltipPlacement.None;
            _rating.RatingSettings.UnRatedStroke = ControlProps.UIColors.White;
            _rating.Readonly = true;
            _rating.Frame = new CGRect(0, 5f, 92, 20);
            _rating.Center = new CGPoint(ContainingView.Center.X, _rating.Center.Y);
            _rating.ItemSize = 15;
            _rating.Value = (nfloat)marker.Rating;

            View.BackgroundColor = ControlProps.UIColors.Pink;
            View.Frame = new CGRect(
                0,
                _parentContainer.View.Frame.GetMaxY(),
                ContainingView.Frame.Width,
                25
            );
        }
    }

    public class BottomSheetCategoryAndAddressContainerView : IBottomSheetContainerView
    {
        private IBottomSheetContainerView _parentContainer;
        private UILabel _categoryName;
        private UILabel _restaurantAddress;

        public UIView ContainingView { get; set; }
        public UIView View { get; set; }

        public BottomSheetCategoryAndAddressContainerView(IBottomSheetContainerView parentContainter)
        {
            _parentContainer = parentContainter;
            ContainingView = _parentContainer.ContainingView;
        }

        public void Init()
        {
            _categoryName = new UILabel();
            _restaurantAddress = new UILabel();
            View = new UIView();
            View.AddSubview(_categoryName);
            View.AddSubview(_restaurantAddress);
            ContainingView.AddSubview(View);
        }

        public void Load(FoodMarker marker)
        {
            _categoryName.Text = marker.CategoryName;
            _categoryName.TextColor = new UIColor(ControlProps.CIColors.Gray);
            _categoryName.Font = UIFont.FromName("Helvetica", 14f);
            _categoryName.TextAlignment = UITextAlignment.Center;
            _categoryName.LineBreakMode = UILineBreakMode.WordWrap;
            _categoryName.Lines = 0;
            _categoryName.Frame = new CGRect(0, 5f, ControlProps.Width, 0);
            LabelSizeCalculator.SetCenterAndHeightBasedOnText(ContainingView, _categoryName);

            _restaurantAddress.Text = marker.RestaurantAddress;
            _restaurantAddress.TextColor = new UIColor(ControlProps.CIColors.Gray);
            _restaurantAddress.Font = UIFont.FromName("Helvetica", 14f);
            _restaurantAddress.TextAlignment = UITextAlignment.Center;
            _restaurantAddress.LineBreakMode = UILineBreakMode.WordWrap;
            _restaurantAddress.Lines = 0;
            _restaurantAddress.Frame = new CGRect(0, _categoryName.Frame.GetMaxY() + 10f, ControlProps.Width, 0);
            LabelSizeCalculator.SetCenterAndHeightBasedOnText(ContainingView, _restaurantAddress);

            View.BackgroundColor = ControlProps.UIColors.Brown;
            View.Frame = new CGRect(
                0,
                _parentContainer.View.Frame.GetMaxY(),
                ContainingView.Frame.Width,
                _categoryName.Frame.Height + _restaurantAddress.Frame.Height + 20f
            );
        }
    }

    public class BottomSheetCommentContainerView : IBottomSheetContainerView
    {
        private IBottomSheetContainerView _parentContainer;
        private UILabel _comment;

        public UIView ContainingView { get; set; }
        public UIView View { get; set; }

        public BottomSheetCommentContainerView(IBottomSheetContainerView parentContainter)
        {
            _parentContainer = parentContainter;
            ContainingView = _parentContainer.ContainingView;
        }

        public void Init()
        {
            _comment = new UILabel();
            View = new UIView();
            View.AddSubview(_comment);
            ContainingView.AddSubview(View);
        }

        public void Load(FoodMarker marker)
        {
            _comment.Text = marker.Comment;
            _comment.TextColor = new UIColor(ControlProps.CIColors.Black);
            _comment.Font = UIFont.PreferredBody;
            _comment.TextAlignment = UITextAlignment.Center;
            _comment.LineBreakMode = UILineBreakMode.WordWrap;
            _comment.Lines = 0;
            _comment.Frame = new CGRect(0, 5f, ControlProps.Width, 0);
            LabelSizeCalculator.SetCenterAndHeightBasedOnText(ContainingView, _comment);

            View.BackgroundColor = ControlProps.UIColors.Yellow;
            View.Frame = new CGRect(
                0,
                _parentContainer.View.Frame.GetMaxY(),
                ContainingView.Frame.Width,
                _comment.Frame.Height + 10f
            );
        }
    }
}
