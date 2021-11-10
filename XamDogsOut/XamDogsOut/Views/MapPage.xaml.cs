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
        }

        protected async override void OnAppearing()
        {      
            base.OnAppearing();

            await GetDeviceLocationAsync();

            

        }

        private async Task GetDeviceLocationAsync()
        {
            var status = await CheckAndRequestPermisionsForLocation();

            if (status == PermissionStatus.Granted)
            {

                try
                {
                    var request = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));
                    cts = new CancellationTokenSource();
                    var location = await Geolocation.GetLocationAsync(request, cts.Token);

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

        private async Task<PermissionStatus> CheckAndRequestPermisionsForLocation()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

            if (status == PermissionStatus.Granted)
                return status;

            status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            return status;

        }
    }
}