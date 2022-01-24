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
using XamDogsOut.Models;
using XamDogsOut.Services;
using XamDogsOut.Services.FirestoreData;

namespace XamDogsOut.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPage : ContentPage
    {
        private Geocoder geocoder;
        private CancellationTokenSource cts;
        private IDataProvider<Dog> _dogService;
        private IDataProvider<Profile> _profileService;
        private IDataProvider<Request> _requestService;
        private IDataProvider<AcceptedRequest> _acceptedRequestService;




        public MapPage()
        {
            InitializeComponent();
            geocoder = new Geocoder();
            _dogService = DependencyService.Get<IDataProvider<Dog>>();
            _profileService = DependencyService.Get<IDataProvider<Profile>>();
            _requestService = DependencyService.Get<IDataProvider<Request>>();
            _acceptedRequestService = DependencyService.Get<IDataProvider<AcceptedRequest>>();


            map.CustomPins = new List<CustomPin>();

            var assembly = typeof(MapPage);
            addRequestButton.Source = ImageSource.FromResource("XamDogsOut.Assets.Images.paw_s.png", assembly);
        }

        protected async override void OnAppearing()
        {      
            base.OnAppearing();

            await GetDeviceLocationAsync();
            await GetStandByRequestOnMap();
            await DisplayRequestCountMarker();
            

        }

        private async Task DisplayRequestCountMarker()
        {
            var userId = Auth.GetCurrentUserId();
            bool userHasRequestPosted = (await (_requestService as RequestTable).GetByUserId(userId)) != null;
            if (userHasRequestPosted)
            {
                var acceptedRequests = await _acceptedRequestService.GetItemsAsync();
                var acceptedUserRequestsCount = acceptedRequests.Where(x => x.SenderId == userId).Count();

                if (acceptedUserRequestsCount > 0)
                {
                    requestCountButton.IsVisible = true;
                    requestCountButton.Text = acceptedUserRequestsCount.ToString();
                }
                else
                {
                    requestCountButton.IsVisible = false;
                }
            }
            else
            {
                requestCountButton.IsVisible = false;
            }
        }

        private async Task GetStandByRequestOnMap()
        {
            var requests = (await _requestService.GetItemsAsync()).Where(x => x.Status == RequestStatuses.StandBy).ToList();

            foreach (var request in requests)
            {
                var pin = await PreparePin(request);
                map.Pins.Add(pin);
                map.CustomPins.Add(pin);
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            map.CustomPins.Clear();
            map.Pins.Clear();
        }

        private async Task<CustomPin> PreparePin(Request request)
        {
            var profile = await _profileService.GetByUserId(request.SenderId);
            var dog = await _dogService.GetByUserId(request.SenderId);
            var sender = await _profileService.GetByUserId(request.SenderId);

            var position = new Position(profile.Lat, profile.Lon);
            return new CustomPin()
            {
                Label = $"Dog: {dog.Name} | Owner: {sender.UserName +  " " + sender.UserSurname}",
                Position = position,
                DogId = dog.Id,
                RequestId = request.Id,
                Type = PinType.SavedPin
            };
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

        private async void addRequestButton_Clicked(object sender, EventArgs e)
        {
            var userDog = await _dogService.GetByUserId(Auth.GetCurrentUserId());
            if (userDog == null)
            {
                await App.Current.MainPage.DisplayAlert("No pet found.", "Looks like you did not provide any information about your pet. " +
                    "Please add a pet profile to use this functionality. ", "Ok");
                return;
            }

            // check if user already had posted a request
            var userHasActiveRequest = (await _requestService.GetItemsAsync())
                                            .Where(x => x.SenderId == Auth.GetCurrentUserId())
                                            .Any(x => x.Status == RequestStatuses.StandBy);

            if (!userHasActiveRequest)
            {
                var answer = await App.Current.MainPage.DisplayAlert("Request", "Would you like to add a request on a map?", "Yes", "No");

                if (answer)
                { 
                    Request newRequest = new Request()
                    {
                        SenderId = Auth.GetCurrentUserId(),
                        Status = RequestStatuses.StandBy
                    };

                    await _requestService.AddItemAsync(newRequest);               
                }
            }
            else
                await Shell.Current.GoToAsync(nameof(RequestInfoPage));



        }
    }
}