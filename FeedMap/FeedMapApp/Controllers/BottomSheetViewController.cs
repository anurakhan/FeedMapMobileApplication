using Foundation;
using System;
using UIKit;
using MapKit;
using CoreGraphics;
using CoreLocation;
using FeedMapApp.Models;
using Syncfusion.SfRating.iOS;

namespace FeedMapApp
{
    public partial class BottomSheetViewController : UIViewController
    {
        public BottomSheetViewController() : base("BottomSheetViewController", null)
        {
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        private readonly CGPoint m_CGPointZero = new CGPoint(0, 0);
        private CGAffineTransform m_PartialTransform;
        private CGAffineTransform m_FullTransform;
        private nfloat m_ContentViewYInit = 80f;
        public readonly nfloat m_PartialView = UIScreen.MainScreen.Bounds.GetMaxY() - 60f;
        public readonly nfloat m_FullView = 20f;
        public int? m_FoodMarkerItem = null;

        #region FoodMarkerControl properties
        UILabel m_FoodName;
        UILabel m_FromText;
        UILabel m_RestaurantName;
        UILabel m_RestaurantAddress;
        UILabel m_Comment;
        UILabel m_CategoryName;
        SfRating m_Rating;
        #endregion

        #region UIBoxViews
        UIView m_RatingBox;
        UIView m_CategoryAndAddressBox;
        UIView m_CommentBox;
        #endregion

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            BottomSheetScrollView.ShowsVerticalScrollIndicator = false;

            View.UserInteractionEnabled = true;
            var dragGesture = new UIPanGestureRecognizer(DragBottomSheet);
            dragGesture.Delegate = new GestureDelegate(View, BottomSheetScrollView, m_PartialView, m_FullView);
            View.AddGestureRecognizer(dragGesture);

            PrepareBackground();

            InitUIForBottomSheet();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewDidAppear(animated);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
        }


        /// <summary>
        /// Prepares Background of the View, by adding dropshadow, rounded corners, and blurred effect.
        /// </summary>
        private void PrepareBackground()
        {
            View.Layer.ShadowColor = UIColor.Black.CGColor;
            View.Layer.ShadowOffset = new CGSize(0, -3);
            View.Layer.ShadowRadius = 3f;
            View.Layer.ShadowOpacity = 0.2f;
            View.Layer.MasksToBounds = false;

            var borderView = new UIView();
            borderView.Layer.CornerRadius = 30f;
            borderView.Frame = UIScreen.MainScreen.Bounds;
            borderView.BackgroundColor = UIColor.FromRGB(208, 96, 79).ColorWithAlpha(0.7f);
            borderView.Layer.MasksToBounds = true;
            View.AddSubview(borderView);
            View.SendSubviewToBack(borderView);

            var blurEffect = UIBlurEffect.FromStyle(UIBlurEffectStyle.Regular);
            var visualEffect = new UIVisualEffectView(blurEffect);
            visualEffect.Frame = UIScreen.MainScreen.Bounds;
            borderView.AddSubview(visualEffect);

        }

        private void InitUIForBottomSheet()
        {
            m_FoodName = new UILabel();
            m_FoodName.TextColor = new UIColor(CoreImage.CIColor.FromRgb(0.14f, 0.14f, 0.14f));
            m_FoodName.Font = UIFont.FromName("Menlo-BoldItalic", 14f);//Title2
            m_FoodName.TextAlignment = UITextAlignment.Center;
            m_FoodName.LineBreakMode = UILineBreakMode.WordWrap;
            m_FoodName.Lines = 0;

            BottomSheetScrollView.AddSubview(m_FoodName);

            m_FromText = new UILabel();
            m_FromText.Text = "from";
            m_FromText.TextColor = new UIColor(CoreImage.CIColor.FromRgb(0.14f, 0.14f, 0.14f));
            m_FromText.Font = UIFont.FromName("Helvetica-BoldOblique", 12f);
            m_FromText.TextAlignment = UITextAlignment.Center;
            m_FromText.LineBreakMode = UILineBreakMode.WordWrap;
            m_FromText.Lines = 0;

            ContentView.AddSubview(m_FromText);

            m_RestaurantName = new UILabel();
            m_RestaurantName.TextColor = new UIColor(CoreImage.CIColor.FromRgb(0.14f, 0.14f, 0.14f));
            m_RestaurantName.Font = UIFont.FromName("Menlo-BoldItalic", 16f);
            m_RestaurantName.TextAlignment = UITextAlignment.Center;
            m_RestaurantName.LineBreakMode = UILineBreakMode.WordWrap;
            m_RestaurantName.Lines = 0;

            ContentView.AddSubview(m_RestaurantName);

            m_RatingBox = new UIView();
            m_RatingBox.BackgroundColor = UIColor.FromRGB(252, 183, 254);
            ContentView.AddSubview(m_RatingBox);

            m_Rating = new SfRating();
            m_Rating.ItemCount = 5;
            m_Rating.Precision = SFRatingPrecision.Standard;
            m_Rating.TooltipPlacement = SFRatingTooltipPlacement.None;
            m_Rating.RatingSettings.UnRatedStroke = UIColor.FromRGB(255, 255, 255);
            m_Rating.Readonly = true;
            m_RatingBox.AddSubview(m_Rating);

            m_CategoryAndAddressBox = new UIView();
            m_CategoryAndAddressBox.BackgroundColor = UIColor.FromRGB(255, 229, 204);
            ContentView.AddSubview(m_CategoryAndAddressBox);
                                 
            m_CategoryName = new UILabel();
            m_CategoryName.TextColor = new UIColor(CoreImage.CIColor.FromRgb(0.28f, 0.28f, 0.28f));
            m_CategoryName.Font = UIFont.FromName("Helvetica", 14f);
            m_CategoryName.TextAlignment = UITextAlignment.Center;
            m_CategoryName.LineBreakMode = UILineBreakMode.WordWrap;
            m_CategoryName.Lines = 0;

            m_CategoryAndAddressBox.AddSubview(m_CategoryName);

            m_RestaurantAddress = new UILabel();
            m_RestaurantAddress.TextColor = new UIColor(CoreImage.CIColor.FromRgb(0.28f, 0.28f, 0.28f));
            m_RestaurantAddress.Font = UIFont.FromName("Helvetica", 14f);
            m_RestaurantAddress.TextAlignment = UITextAlignment.Center;
            m_RestaurantAddress.LineBreakMode = UILineBreakMode.WordWrap;
            m_RestaurantAddress.Lines = 0;

            m_CategoryAndAddressBox.AddSubview(m_RestaurantAddress);

            m_CommentBox = new UIView();
            m_CommentBox.BackgroundColor = UIColor.FromRGB(255, 255, 204);
            ContentView.AddSubview(m_CommentBox);

            m_Comment = new UILabel();
            m_Comment.TextColor = new UIColor(CoreImage.CIColor.FromRgb(0.14f, 0.14f, 0.14f));
            m_Comment.Font = UIFont.PreferredBody;
            m_Comment.TextAlignment = UITextAlignment.Center;
            m_Comment.LineBreakMode = UILineBreakMode.WordWrap;
            m_Comment.Lines = 0;

            m_CommentBox.AddSubview(m_Comment);

        }

        /// <summary>
        /// Method that the gets invoked when drag gesture is performed.
        /// </summary>
        /// <param name="dragGesture">Drag gesture.</param>
        public void DragBottomSheet(UIPanGestureRecognizer dragGesture)
        {
            var translation = dragGesture.TranslationInView(View);
            var velocity = dragGesture.VelocityInView(View);
            var bottomSheetY = View.Frame.GetMinY();


            if (bottomSheetY + translation.Y <= m_PartialView && bottomSheetY + translation.Y >= m_FullView)
            {
                View.Frame = new CGRect(0, bottomSheetY + translation.Y, View.Frame.Width, View.Frame.Height);
                dragGesture.SetTranslation(m_CGPointZero, View);
            }

            if (dragGesture.State == UIGestureRecognizerState.Ended)
            {
                MoveSheetToBound(velocity.Y);
            }
        }

        public void MoveSheetToBound(nfloat velocityY)
        {
            var bottomSheetY = View.Frame.GetMinY();
            var duration = velocityY < 0 ? (double)((bottomSheetY - m_FullView) / -velocityY)
                : (double)((m_PartialView - bottomSheetY) / velocityY);
            duration = duration > 1.3 ? 1 : duration;


            UIView.Animate(duration, () =>
            {
                if (velocityY > 0)
                {
                    View.Frame = new CGRect(0, m_PartialView, View.Frame.Width, View.Frame.Height);
                    m_FoodName.Transform = m_PartialTransform;
                    BottomSheetScrollView.Frame = new CGRect(0, 0,
                                                     View.Frame.Width, View.Frame.Height);
                    ContentView.Frame = new CGRect(0, m_ContentViewYInit,
                                                   ContentView.Frame.Width, ContentView.Frame.Height);
                }
                else
                {
                    View.Frame = new CGRect(0, m_FullView, View.Frame.Width, View.Frame.Height);
                    m_FoodName.Transform = m_FullTransform;
                    BottomSheetScrollView.Frame = new CGRect(0, 60,
                                                     View.Frame.Width, View.Frame.Height - 60);
                    ContentView.Frame = new CGRect(0, m_FoodName.Frame.Height + 15f,
                                                   ContentView.Frame.Width, ContentView.Frame.Height);
                }
            }, completion: () =>
            {
                if (velocityY < 0)
                {
                    this.BottomSheetScrollView.ScrollEnabled = true;
                }
            });
        }

        private void SetUIBounds()
        {
            m_FoodName.Frame = new CGRect(0, 10, 270, 0);
            SetCenterAndStringSize(m_FoodName);
            m_PartialTransform = m_FoodName.Transform;
            m_FullTransform = CGAffineTransform.Scale(m_FoodName.Transform, 1.3f, 1.3f);
            //m_FullTransform = CGAffineTransform.Translate(m_FullTransform, 0f, 45f);

            //m_FromText.Frame = new CGRect(0, Math.Max((m_FoodName.Frame.GetMaxY() * 1.3f) + 60f, 100f), 270, 0);
            //m_FromText.Frame = new CGRect(0, (m_FoodName.Frame.GetMaxY() * 1.3f) + 10f, 270, 0);
            m_FromText.Frame = new CGRect(0, 0f, 270, 0);
            SetCenterAndStringSize(m_FromText);


            m_RestaurantName.Frame = new CGRect(0, m_FromText.Frame.GetMaxY() + 10f, 270, 0);
            SetCenterAndStringSize(m_RestaurantName);

            m_RatingBox.Frame = new CGRect(0, m_RestaurantName.Frame.GetMaxY() + 10f,
                                           View.Frame.Width, 25);

            m_Rating.Frame = new CGRect(0, 5f, 92, 20);
            m_Rating.Center = new CGPoint(View.Center.X, m_Rating.Center.Y);
            m_Rating.ItemSize = 15;


            m_CategoryName.Frame = new CGRect(0, 5f, 270, 0);
            SetCenterAndStringSize(m_CategoryName);

            m_RestaurantAddress.Frame = new CGRect(0, m_CategoryName.Frame.GetMaxY() + 10f, 270, 0);
            SetCenterAndStringSize(m_RestaurantAddress);

            m_CategoryAndAddressBox.Frame = new CGRect(0, m_RatingBox.Frame.GetMaxY(),
                                                       View.Frame.Width,
                                                       m_CategoryName.Frame.Height + m_RestaurantAddress.Frame.Height + 20f);


            m_Comment.Frame = new CGRect(0, 5f, 270, 0);
            SetCenterAndStringSize(m_Comment);

            m_CommentBox.Frame = new CGRect(0, m_CategoryAndAddressBox.Frame.GetMaxY(),
                                                       View.Frame.Width, m_Comment.Frame.Height + 10f);

            ContentView.Frame = new CGRect(0, m_ContentViewYInit,
                                           View.Frame.Width,
                                           m_CommentBox.Frame.GetMaxY());

            BottomSheetScrollView.Frame = new CGRect(0, 0, 
                                                     View.Frame.Width, View.Frame.Height);
            
            BottomSheetScrollView.ContentSize = new CGSize(View.Frame.Width, ContentView.Frame.Height + 80f);

        }

        private void SetCenterAndStringSize(UILabel label)
        {
            var size = new CGSize(0, 0);
            if (!string.IsNullOrEmpty(label.Text))
                size = label.Text.StringSize(label.Font,
                    constrainedToSize: new CGSize(label.Frame.Width, 900f),
                    lineBreakMode: UILineBreakMode.WordWrap);
            var labelFrame = label.Frame;
            labelFrame.Size = new CGSize(label.Frame.Width, size.Height);
            label.Frame = labelFrame;
            label.Center = new CGPoint(View.Center.X, label.Center.Y);
        }

        private void ShowBottomSheetFromBelow() 
        {
            if (View.Frame.Y > UIScreen.MainScreen.Bounds.Height - 60f)
            {
                UIView.Animate(0.5, () =>
                {
                    var frame = View.Frame;
                    var sheetY = UIScreen.MainScreen.Bounds.Height - 60f;
                    View.Frame = new CGRect(0, sheetY, frame.Width, frame.Height);
                });
            }
        }

        public void PopulateBottomSheetWithFoodMarkerData(FoodMarker foodMarker)
        {
            if (m_FoodMarkerItem.HasValue)
            {
                if (m_FoodMarkerItem == foodMarker.FoodMarkerId) return;
            }

            m_FoodName.Text = foodMarker.FoodName;
            m_RestaurantName.Text = foodMarker.RestaurantName;
            m_Rating.Value = (nfloat)foodMarker.Rating;
            m_CategoryName.Text = foodMarker.CategoryName;
            m_RestaurantAddress.Text = foodMarker.RestaurantAddress;
            m_Comment.Text = foodMarker.Comment;

            SetUIBounds();
            ShowBottomSheetFromBelow();
        }
    }

    public class GestureDelegate : UIGestureRecognizerDelegate
    {
        UIView m_View;
        UIScrollView m_ScrollView;
        readonly nfloat m_PartialView;
        readonly nfloat m_FullView;

        public GestureDelegate(UIView view, UIScrollView scrollView, nfloat partialView, nfloat fullView)
        {
            m_View = view;
            m_ScrollView = scrollView;
            m_PartialView = partialView;
            m_FullView = fullView;
        }

        public override bool ShouldRecognizeSimultaneously(UIGestureRecognizer gestureRecognizer, UIGestureRecognizer otherGestureRecognizer)
        {
            var gesture = (UIPanGestureRecognizer)gestureRecognizer;
            var direction = gesture.VelocityInView(m_View).Y;

            var y = m_View.Frame.GetMinY();
            if ((y == m_FullView && m_ScrollView.ContentOffset.Y == 0 && direction > 0) || (y == m_PartialView))
            {
                m_ScrollView.ScrollEnabled = false;
            }
            else
            {
                m_ScrollView.ScrollEnabled = true;
            }
            return false;
        }
    }
}