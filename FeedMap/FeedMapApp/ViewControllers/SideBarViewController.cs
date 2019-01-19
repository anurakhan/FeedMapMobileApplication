using System;
using CoreGraphics;
using FeedMapApp.Services;
using FeedMapApp.Services.Navigation;
using FeedMapApp.Views.SideBarViews;
using UIKit;

namespace FeedMapApp
{
    public partial class SideBarViewController : UIViewController
    {
        private readonly nfloat m_ViewRevealedX = -1 * UIScreen.MainScreen.Bounds.Width / 3;
        private readonly nfloat m_ViewHiddenX = -1 * UIScreen.MainScreen.Bounds.Width;
        private readonly double _animationDuration = 0.5;

        public SideBarViewController() : base("SideBarViewController", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var _bottomSheetBackgroundView = new SideBarBackgroundView(View);
            _bottomSheetBackgroundView.Load();

            AddGestureRecognizer(BackButtonImg, BackButtonPressed);
            AddGestureRecognizer(LogOutButtonImg, LogOutButtonPressed);
        }

        private void AddGestureRecognizer(UIView obj, Action action)
        {
            var gestureRec = new UITapGestureRecognizer(action);
            obj.AddGestureRecognizer(gestureRec);
            obj.UserInteractionEnabled = true;
        }

        private void BackButtonPressed()
        {
            DoTransitionToX(m_ViewHiddenX, _animationDuration);
        }

        private void LogOutButtonPressed()
        {
            var tokenPersistanceService = new TokenPersistanceService();
            tokenPersistanceService.RemoveToken(WebApiCred.KeyChainTokenKey);

            LogoutService logOutService = new LogoutService(
                UIApplication.SharedApplication.Delegate as AppDelegate);

            logOutService.LogoutStart();
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        public void RevealSideBar(double duration)
        {
            if (View.Frame.X < m_ViewRevealedX)
            {
                DoTransitionToX(m_ViewRevealedX, duration);
            }
        }

        private void DoTransitionToX(nfloat x, double dur)
        {
            UIView.Animate(dur, () =>
            {
                var frame = View.Frame;
                View.Frame = new CGRect(x, frame.Y, frame.Width, frame.Height);
            });
        }

    }
}

