using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;
using XamDogsOut.Helpers;

namespace XamDogsOut.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPage : ContentPage
    {
        private Geocoder geocoder;
        private CancellationTokenSource cts;

        public MapPage()
        {
            InitializeComponent();
            geocoder = new Geocoder();

            var assembly = typeof(MapPage);
            addRequestButton.Source = ImageSource.FromResource("XamDogsOut.Assets.Images.paw_s.png", assembly);
        }

        protected async override void OnAppearing()
        {      
            base.OnAppearing();

            await GetDeviceLocationAsync();

            

        }

        private async Task GetDeviceLocationAsync()
        {
            var status = await GeoHelpers.CheckAndRequestPermisionsForLocation();

            if (status == PermissionStatus.Granted)
            {

                try
                {

                    var location = await GeoHelpers.GetDeviceLocation();
                    

                    if (location != null)
                    {
                        var position = new Position(location.Latitude, location.Longitude);

                        var mapSpan = new MapSpan(position, 0.01, 0.01);
                        map.IsShowingUser = true;

                        map.MoveToRegion(mapSpan); 
                    }
                }
                catch (FeatureNotSupportedException fnsEx)
                {
                    // Handle not supported on device exception
                    await App.Current.MainPage.DisplayAlert("Error", fnsEx.Message, "Ok");
                }
                catch (FeatureNotEnabledException fneEx)
                {
                    await App.Current.MainPage.DisplayAlert("Error", fneEx.Message, "Ok");
                }
                catch (PermissionException pEx)
                {
                    await App.Current.MainPage.DisplayAlert("Error", pEx.Message, "Ok");
                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");             
                }   
            }
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage.Navigation.PushAsync(new EditDogPage());
        }
    }
}