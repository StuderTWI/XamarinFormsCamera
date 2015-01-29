using System;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using System.Globalization;
using System.Reflection;
using Camera_Test.Media;
#if __IOS__
using UIKit;
#endif


namespace Camera_Test
{
    public class HomeScreen : ContentPage
    {
        PhotoListData pld;
        public HomeScreen()
        {
            pld = new PhotoListData();

            Title = "Camera Test";

            Button clickMe = new Button
            {
                Text = "Click Me!"
            };

            StackLayout mainContainer = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = Color.White,
                Spacing = 0,
                Children = { clickMe }
            };

            Content = mainContainer;
            clickMe.Clicked += clickMe_Clicked;
        }

        async void clickMe_Clicked(object sender, EventArgs e)
        {
            await doPhotoAction(pld);
        }

        async private Task doPhotoAction(PhotoListData pld)
        {
#if __ANDROID__
            var action = await DisplayActionSheet("Select Source", "Cancel", null, "Camera", "Photo Library");
            if (action == "Camera")
            {
                doCameraPhoto(pld);
            }
            else if (action == "Photo Library")
            {
                doPhotoLibrary(pld);
            }
#else
            if (App.isIOS8) // for iOS 8.0
            {
                var avAlert = UIAlertController.Create("Select Source", "", UIAlertControllerStyle.ActionSheet);
                avAlert.AddAction(UIAlertAction.Create("Camera", UIAlertActionStyle.Default, async (UIAlertAction obj) => doCameraPhoto(pld)));
                avAlert.AddAction(UIAlertAction.Create("Photo Library", UIAlertActionStyle.Default, async (UIAlertAction obj) => doPhotoLibrary(pld)));
                avAlert.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Default, null));
                App.rootController.ShowViewController(avAlert, null);
            }
            else
            {
                var action = await DisplayActionSheet("Select Source", "Cancel", null, "Camera", "Photo Library");
                if (action == "Camera")
                {
                    doCameraPhoto(pld);
                }
                else if (action == "Photo Library")
                {
                    doPhotoLibrary(pld);
                }
                //doPhotoLibrary(pld);
            }
#endif
        }

        async void doCameraPhoto(PhotoListData pld)
        {
#if __ANDROID__
            MediaPicker picker = new MediaPicker(Forms.Context);
#else
            MediaPicker picker = new MediaPicker();
#endif

            if (picker.IsCameraAvailable == false)
            {
                var page = new ContentPage();
                var result = page.DisplayAlert("Warning", "Camera is not available", "OK");

                return;
            }
            else
            {
                try
                {
                    var resultfile = await picker.TakePhoto(null);
#if __ANDROID__
                    showDrawingView(pld);
#else
                    showDrawingView(pld);
#endif
                }
                catch (Exception ex)
                {
                }
            }
        }

        async void doPhotoLibrary(PhotoListData pld)
        {
#if __ANDROID__
            MediaPicker picker = new MediaPicker(Forms.Context);
#else
            MediaPicker picker = new MediaPicker();
#endif

            if (picker.IsPhotoGalleryAvailable == false)
            {
                var page = new ContentPage();
                var result = page.DisplayAlert("Warning", "Photo is not available", "OK");

                return;
            }
            else
            {
                try
                {
                    var resultfile = await picker.PickPhoto();
#if __ANDROID__
                    //showDrawingView(pld);
#else
                    //showDrawingView(pld);
#endif
                }
                catch (Exception e)
                {
                }
            }
        }

        async void showDrawingView(PhotoListData pld)
        {
            //var pv = new DrawingPhotoView();
            //pv.PhotoList = pld;
            //await Navigation.PushAsync(pv);
            //pld = pv.PhotoList;
        }
    }

    public class PhotoListData : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        List<string> _photoList;
        int _count;
        string _countItem;
        string _firstPhotoPath;

        // Init Function
        public PhotoListData()
        {
            _photoList = new List<string>();
            _count = 0;
            _countItem = "ADD PHOTO";
            _firstPhotoPath = "takePhoto_icon.png";
        }

        public void AddPhotoItem(string photoPath, bool _comesFromSubmit)
        {
            if (_count == 0)
                FirstPhotoPath = photoPath;

            _photoList.Add(photoPath);
            _count = _photoList.Count;
            CountItemString = _comesFromSubmit ? String.Format("ADD MORE PHOTOS ({0} added)", _count) : String.Format("VIEW PHOTOS ({0})", _count);
        }

        // Data Fields that Change
        public string FirstPhotoPath
        {
            set
            {
                _firstPhotoPath = value;

                if (PropertyChanged != null)
                {
                    PropertyChanged(this,
                        new PropertyChangedEventArgs("FirstPhotoPath"));
                }
            }

            get
            {
                return _firstPhotoPath;
            }
        }

        public string CountItemString
        {
            set
            {
                _countItem = value;

                if (PropertyChanged != null)
                {
                    PropertyChanged(this,
                        new PropertyChangedEventArgs("CountItemString"));
                }
            }

            get
            {
                return _countItem;
            }
        }

        public List<string> PhotoList
        {
            set
            {
                _photoList = value;

                if (PropertyChanged != null)
                {
                    PropertyChanged(this,
                        new PropertyChangedEventArgs("PhotoList"));
                }
            }

            get
            {
                return _photoList;
            }
        }
    }

}
