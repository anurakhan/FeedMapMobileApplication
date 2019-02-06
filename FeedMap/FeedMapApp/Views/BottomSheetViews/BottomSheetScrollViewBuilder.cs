using System;
using CoreGraphics;
using FeedMapApp.Models;
using FeedMapApp.Views.BottomSheetViews;
using Syncfusion.SfRating.iOS;
using UIKit;

namespace FeedMapApp.Views.BottomSheetViews
{
    public class BottomSheetScrollViewBuilder
    {
        private UIScrollView _scrollView;
        public nfloat Width { get; set; }
        public nfloat Height { get; set; }
        private readonly nfloat _overhead = 80f;

        public UILabel FoodNameControl { get; set; }

        public BottomSheetScrollViewBuilder(UIScrollView scrollView)
        {
            _scrollView = scrollView;
        }

        public void Init()
        {
            FoodNameControl = new UILabel();
            _scrollView.AddSubview(FoodNameControl);
        }

        public void Load(string text)
        {
            _scrollView.Frame = new CGRect(0, 0, Width, Height);

            FoodNameControl.Text = text;
            FoodNameControl.TextColor = ControlProps.UIColors.Black;
            FoodNameControl.Font = UIFont.FromName("Menlo-BoldItalic", 14f);
            FoodNameControl.TextAlignment = UITextAlignment.Center;
            FoodNameControl.LineBreakMode = UILineBreakMode.WordWrap;
            FoodNameControl.Lines = 0;

            FoodNameControl.Frame = new CGRect(0, 10, 270, 0);
            LabelSizeCalculator.SetCenterAndHeightBasedOnText(_scrollView, FoodNameControl);
        }

        public void SetContentSize(nfloat width, nfloat height)
        {
            _scrollView.ContentSize = new CGSize(width, height + _overhead);
        }

    }
}
