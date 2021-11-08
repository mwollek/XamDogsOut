using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
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
        private readonly IGeolocator locator;
        private CancellationTokenSource cts;

        public MapPage()
        {
            InitializeComponent();
            locator = CrossGeolocator.Current;
        }

        protected async override void OnAppearing()
        {      
            base.OnAppearing();

            await GetDeviceLocationAsync();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            locator.StopListeningAsync();
        }

        private async Task GetDeviceLocationAsync()
        {
            var status = await CheckAndRequestPermisionsForLocation();

            if (status == PermissionStatus.Granted)
            {
                var location = await locator.GetPositionAsync();


                locator.PositionChanged += Locator_PositionChanged;
                await locator.StartListeningAsync(new TimeSpan(0, 0, 5), 0.01);

                map.IsShowingUser = true;
                label.Text = $"{location.Latitude} | {location.Longitude}";


                CenterMap(location.Latitude, location.Longitude);
            }
        }

        private void Locator_PositionChanged(object sender, Plugin.Geolocator.Abstractions.PositionEventArgs e)
        {
            CenterMap(e.Position.Latitude, e.Position.Longitude);
            label.Text = $"{e.Position.Latitude} | {e.Position.Longitude}";
        }

        private void CenterMap(double latitude, double longitude)
        {
            Xamarin.Forms.Maps.Position center = new Xamarin.Forms.Maps.Position(latitude, longitude);
            MapSpan mapSpan = new MapSpan(center, 0.01, 0.01);

            map.MoveToRegion(mapSpan);

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