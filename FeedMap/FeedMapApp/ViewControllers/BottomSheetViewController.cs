using Foundation;
using System;
using UIKit;
using MapKit;
using CoreGraphics;
using CoreLocation;
using FeedMapApp.Models;
using Syncfusion.SfRating.iOS;
using FeedMapApp.Views.BottomSheetViews;
using System.Collections.Generic;

namespace FeedMapApp
{
    public partial class BottomSheetViewController : UIViewController
    {

        private CGAffineTransform m_PartialTransform;
        private CGAffineTransform m_FullTransform;

        private readonly CGPoint m_CGPointZero = new CGPoint(0, 0);
        private readonly nfloat m_ContentViewYInit = 80f;
        private readonly nfloat m_PartialView = UIScreen.MainScreen.Bounds.GetMaxY() - 60f;
        private readonly nfloat m_FullView = 20f;
        private readonly nfloat m_ViewPop = UIScreen.MainScreen.Bounds.Height - 60f;

        public int? m_FoodMarkerItem = null;

        private FoodMarker _foodMarker;
        private BottomSheetScrollViewBuilder _scrollViewBuilder;
        private BottomSheetContentViewBuilder _contentViewBuilder;
        private BottomSheetBackgroundView _bottomSheetBackgroundView;
        private List<IBottomSheetContainerView> _containerList;

        public BottomSheetViewController() : base("BottomSheetViewController", null)
        {
            
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            BottomSheetScrollView.ShowsVerticalScrollIndicator = false;

            View.UserInteractionEnabled = true;
            var dragGesture = new UIPanGestureRecognizer(DragBottomSheet);
            dragGesture.Delegate = new GestureDelegate(View, BottomSheetScrollView, m_PartialView, m_FullView);
            View.AddGestureRecognizer(dragGesture);

            _bottomSheetBackgroundView = new BottomSheetBackgroundView(View);
            _bottomSheetBackgroundView.Load();

            InitBottomSheetViewContainers();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewDidAppear(animated);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
        }

        public void InitBottomSheetViewContainers()
        {
            _containerList = new List<IBottomSheetContainerView>();
            _scrollViewBuilder =
                new BottomSheetScrollViewBuilder(BottomSheetScrollView);
            _scrollViewBuilder.Init();

            _contentViewBuilder =
                new BottomSheetContentViewBuilder(ContentView);

            IBottomSheetContainerView containerView = new BottomSheetTemplateContainerView(ContentView);
            containerView.Init();
            _containerList.Add(containerView);

            containerView = new BottomSheetFromContainerView(containerView);
            containerView.Init();
            _containerList.Add(containerView);

            containerView = new BottomSheetRestaurantContainerView(containerView);
            containerView.Init();
            _containerList.Add(containerView);

            containerView = new BottomSheetRatingContainerView(containerView);
            containerView.Init();
            _containerList.Add(containerView);

            containerView = new BottomSheetCategoryAndAddressContainerView(containerView);
            containerView.Init();
            _containerList.Add(containerView);

            containerView = new BottomSheetCommentContainerView(containerView);
            containerView.Init();
            _containerList.Add(containerView);

        }

        /// <summary>
        /// Method that gets invoked when drag gesture is performed.
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

                    _scrollViewBuilder.FoodNameControl.Transform = m_PartialTransform;

                    BottomSheetScrollView.Frame = new CGRect(0, 0,
                                                     View.Frame.Width, View.Frame.Height);
                    ContentView.Frame = new CGRect(0, m_ContentViewYInit,
                                                   ContentView.Frame.Width, ContentView.Frame.Height);
                }
                else
                {
                    View.Frame = new CGRect(0, m_FullView, View.Frame.Width, View.Frame.Height);

                    _scrollViewBuilder.FoodNameControl.Transform = m_FullTransform;

                    BottomSheetScrollView.Frame = new CGRect(0, 60,
                                                     View.Frame.Width, View.Frame.Height - 60);
                    ContentView.Frame = new CGRect(0, _scrollViewBuilder.FoodNameControl.Frame.Height + 15f,
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

        private void ShowBottomSheetFromBelow()
        {
            if (View.Frame.Y > m_ViewPop)
            {
                UIView.Animate(0.5, () =>
                {
                    var frame = View.Frame;
                    var sheetY = m_ViewPop;
                    View.Frame = new CGRect(0, sheetY, frame.Width, frame.Height);
                });
            }
        }

        private void SetUI()
        {
            _scrollViewBuilder.Width = View.Frame.Width;
            _scrollViewBuilder.Height = View.Frame.Height;
            _scrollViewBuilder.Load(_foodMarker.FoodName);
            m_PartialTransform = _scrollViewBuilder.FoodNameControl.Transform;
            m_FullTransform = CGAffineTransform.Scale(_scrollViewBuilder.FoodNameControl.Transform, 1.3f, 1.3f);

            _contentViewBuilder.Width = View.Frame.Width;
            _contentViewBuilder.Y = m_ContentViewYInit;
            _contentViewBuilder.X = 0;
            _contentViewBuilder.Height = 0;
            _contentViewBuilder.Load();

            foreach (IBottomSheetContainerView container in _containerList)
            {
                container.Load(_foodMarker);
            }

            _contentViewBuilder.Height = _containerList[_containerList.Count - 1].View.Frame.GetMaxY();
            _contentViewBuilder.Load();

            _scrollViewBuilder.SetContentSize(View.Frame.Width, ContentView.Frame.Height);
        }

        public void PopulateBottomSheetWithFoodMarkerData(FoodMarker foodMarker)
        {
            if (m_FoodMarkerItem.HasValue)
            {
                if (m_FoodMarkerItem == foodMarker.FoodMarkerId) return;
            }

            _foodMarker = foodMarker;

            SetUI();
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