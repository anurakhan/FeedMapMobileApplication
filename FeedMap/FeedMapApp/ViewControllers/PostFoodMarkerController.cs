using System;
using System.Threading.Tasks;
using Chafu;
using CoreFoundation;
using CoreLocation;
using FeedMapApp.Helpers.DirectoryHelpers;
using FeedMapApp.Models;
using FeedMapApp.Services;
using Foundation;
using UIKit;
using Xamarin.iOS.DGActivityIndicatorViewBinding;

namespace FeedMapApp
{
    public partial class PostFoodMarkerController : UIViewController
    {
        private readonly double _animationDuration = 0.3;
        private ChafuViewController _chafuViewController;
        private AlbumViewController _albumViewController;
        private MediaPickerService _mediaPickerService;
        private readonly nfloat _maxDimensionCap = 600;
        private DGActivityIndicatorView _activityIndicatorView;

        CLLocationManager _locationManager;

        public event EventHandler OnSaveSuccess;

        public PostFoodMarkerController() : base("PostFoodMarkerController", null)
        {
            Configuration.TintColor = UIColor.Yellow;
            _chafuViewController = new ChafuViewController { HasVideo = false };
            _albumViewController = new AlbumViewController 
            {
                LazyDataSource = (view, size, mediaTypes) =>
                    new LocalFilesDataSource(view, size, mediaTypes) {
                    ImagesPath = (new FoodMarkerPendingImageDirectory()).GetDir() },
                LazyDelegate = (view, source) => new LocalFilesDelegate(view, (LocalFilesDataSource)source)
            };
            _locationManager = new CLLocationManager();
            _mediaPickerService = new MediaPickerService();
        }

        public override async void ViewDidLoad()
        {
            base.ViewDidLoad();

            InitDoneButton();

            AddGestureRecognizer(PostFoodMarkerCloseBtn, CloseButtonPressed);
            AddGestureRecognizer(AddMediaBtn, AddMediaBtnPressed);
            AddGestureRecognizer(SeeMediaBtn, SeeMediaBtnPressed);
            AddGestureRecognizer(PostFoodMarkerOkButton, OkButtonPressed);

            PostFoodMarkerComment.Started += MoveViewToTop;
            PostFoodMarkerComment.Ended += MoveViewToBottom;

            UIPickerView pickerView = new UIPickerView();
            var model = new CategoryModel(PostFoodMarkerCategory);
            await model.InitCat();
            pickerView.Model = model;
            PostFoodMarkerCategory.InputView = pickerView;

            SetMediaPickerEventHandlers();

            _activityIndicatorView = 
                new DGActivityIndicatorView(DGActivityIndicatorAnimationType.BallBeat,
                                            UIColor.Green);
            _activityIndicatorView.Center = new CoreGraphics.CGPoint(View.Center.X, PostFoodMarkerOkButton.Center.Y);
            View.AddSubview(_activityIndicatorView);
        }

		public override void ViewDidAppear(bool animated)
		{
            base.ViewDidAppear(animated);
            PostFoodMarkerOkButton.Image = UIImage.FromBundle("CheckButton");
		}

		private void SetMediaPickerEventHandlers()
        {
            _mediaPickerService.ClearPending();
            _chafuViewController.ImageSelected += (sender, image) =>
            {
                nfloat h = image.Size.Height;
                nfloat w = image.Size.Width;
                if (_maxDimensionCap < h)
                {
                    w *= (_maxDimensionCap / h);
                    h = _maxDimensionCap;
                }

                if (_maxDimensionCap < w)
                {
                    h *= (_maxDimensionCap / w);
                    w = _maxDimensionCap;
                }

                image = image.Scale(new CoreGraphics.CGSize(Convert.ToDouble(w),
                                                    Convert.ToDouble(h)));
                _mediaPickerService.SaveMediaToPending(image);
            };

            _chafuViewController.Closed += (sender, e) =>
            {

            };
        }

        private void InitDoneButton()
        {
            var toolbar = new UIToolbar(new CoreGraphics.CGRect(0.0f, 0.0f, this.View.Frame.Size.Width, 44.0f));
            toolbar.Items = new UIBarButtonItem[]{
                new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
                new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate {
                    View.EndEditing(true);
                })
            };

            PostFoodMarkerName.InputAccessoryView =
            PostFoodMarkerRestaurant.InputAccessoryView =
            PostFoodMarkerRating.InputAccessoryView =
            PostFoodMarkerComment.InputAccessoryView =
            PostFoodMarkerCategory.InputAccessoryView = toolbar;
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }

        private void AddGestureRecognizer(UIView obj, Action action)
        {
            var gestureRec = new UITapGestureRecognizer(action);
            obj.AddGestureRecognizer(gestureRec);
            obj.UserInteractionEnabled = true;
        }

        private void MoveViewToTop(object sender, EventArgs eventArgs)
        {
            UIView.Animate(_animationDuration, () =>
            {
                View.Frame = new CoreGraphics.CGRect(
                    View.Frame.X,
                    View.Frame.Y - (PostFoodMarkerComment.Frame.Y -
                        PostFoodMarkerName.Frame.Y),
                    View.Frame.Width,
                    View.Frame.Height
                );
            });
        }

        private void MoveViewToBottom(object sender, EventArgs eventArgs)
        {
            UIView.Animate(_animationDuration, () =>
            {
                View.Frame = new CoreGraphics.CGRect(
                    View.Frame.X,
                    0f, 
                    View.Frame.Width,
                    View.Frame.Height
                );
            });
        }

        private void CloseButtonPressed()
        {
            this.DismissViewController(true, null);
        }

        private void AddMediaBtnPressed()
        {
            PresentModalViewController(_chafuViewController, true);
        }

        private void SeeMediaBtnPressed()
        {
            PresentModalViewController(_albumViewController, true);
        }

        private async void OkButtonPressed()
        {
            PostFoodMarkerOkButton.Hidden = true;
            PostFoodMarkerOkButton.Image = UIImage.FromBundle("CheckButtonPressed");
            _activityIndicatorView.StartAnimating();
            try
            {
                var images = _mediaPickerService.GetMediaFromPending();
                if (images == null || images.Count == 0)
                {
                    NotCorrect();
                    return;
                }
                var foodMarker = new FoodMarker()
                {
                    FoodName = PostFoodMarkerName.Text,
                    CategoryName = PostFoodMarkerCategory.Text,
                    Comment = PostFoodMarkerComment.Text,
                    RestaurantName = PostFoodMarkerRestaurant.Text
                };

                string rating = PostFoodMarkerRating.Text;
                foodMarker.Rating = (String.IsNullOrWhiteSpace(rating)? -1 : Convert.ToInt32(rating));


                double lat = _locationManager.Location.Coordinate.Latitude;
                double lng = _locationManager.Location.Coordinate.Longitude;
                FoodMarkerService service = new FoodMarkerService();

                bool isSaved = await service.SaveFoodMarker(foodMarker, lat, lng);
                if (!isSaved)
                {
                    NotCorrect();
                    return;
                }

                if (images != null && images.Count != 0)
                {
                    var imageMetas = await service.SavePhotos(foodMarker.FoodMarkerId, _mediaPickerService.GetMediaFromPending());
                    foodMarker.FoodMarkerPhotos = imageMetas;
                    _mediaPickerService.ClearPending();
                }

                if (OnSaveSuccess != null)
                {
                    OnSaveSuccess((object)foodMarker, new EventArgs());
                }
                CloseButtonPressed();
            }
            catch
            {
                _activityIndicatorView.StopAnimating();
                UIImageView errorView = new UIImageView();
                errorView.Image = UIImage.FromBundle("ErrorScreen");
                double side = (View.Frame.Width * 2.0 / 3.0);
                errorView.Frame = new CoreGraphics.CGRect(0, 0, side, side);
                errorView.Center = View.Center;
                View.AddSubview(errorView);
                View.BringSubviewToFront(errorView);
                NSTimer.CreateScheduledTimer(3.0, delegate 
                {
                    errorView.RemoveFromSuperview();
                });
                PostFoodMarkerOkButton.Hidden = false;
            }
        }

        private void NotCorrect()
        {
            _activityIndicatorView.StopAnimating();
            UIImageView validationScreen = new UIImageView();
            validationScreen.Image = UIImage.FromBundle("ValidationScreen");
            double side = (View.Frame.Width * 2.0 / 3.0);
            validationScreen.Frame = new CoreGraphics.CGRect(0, 0, side, side);
            validationScreen.Center = View.Center;
            View.AddSubview(validationScreen);
            View.BringSubviewToFront(validationScreen);
            NSTimer.CreateScheduledTimer(3.0, delegate
            {
                validationScreen.RemoveFromSuperview();
            });
            PostFoodMarkerOkButton.Image = UIImage.FromBundle("CheckButton");
            PostFoodMarkerOkButton.Hidden = false;
        }
    }
}

