using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using XamDogsOut.Helpers;
using XamDogsOut.Models;
using XamDogsOut.Services;
using XamDogsOut.Views;

namespace XamDogsOut.ViewModels
{
    public class EditProfileVM : BaseViewModel
    {
        public Command SaveCommand { get; set; }
        public Command TryLocateMeCommand { get; set; }


        private string userName;
        public string UserName
        {
            get => userName;
            set
            {
                SetProperty(ref userName, value);
                OnPropertyChanged(nameof(ModelIsReady));

            }
        }
        private string userSurname;
        public string UserSurname
        {
            get => userSurname;
            set
            {
                SetProperty(ref userSurname, value);
                OnPropertyChanged(nameof(ModelIsReady));

            }
        }

        private string country;
        public string Country
        {
            get => country;
            set
            {
                SetProperty(ref country, value);
                OnPropertyChanged(nameof(ModelIsReady));

            }
        }

        private string city;
        public string City
        {
            get => city;
            set
            {
                SetProperty(ref city, value);
                OnPropertyChanged(nameof(ModelIsReady));

            }
        }
        private string street;
        public string Street
        {
            get => street;
            set
            {
                SetProperty(ref street, value);
                OnPropertyChanged(nameof(ModelIsReady));

            }
        }
        private string buildingNumber;
        public string BuildingNumber
        {
            get => buildingNumber;
            set
            {
                SetProperty(ref buildingNumber, value);
                OnPropertyChanged(nameof(ModelIsReady));

            }
        }
        private string flatNumber;
        public string FlatNumber
        {
            get => flatNumber;
            set
            {
                SetProperty(ref flatNumber, value);
                OnPropertyChanged(nameof(ModelIsReady));

            }
        }
        private string zipCode;
        public string ZipCode
        {
            get => zipCode;
            set
            {
                SetProperty(ref zipCode, value);
                OnPropertyChanged(nameof(ModelIsReady));

            }
        }

        public bool ModelIsReady
        {
            get
            {
                bool isNameEmpty = string.IsNullOrEmpty(UserName);
                bool isSurnameEmpty = string.IsNullOrEmpty(UserSurname);
                bool isCountryEmpty = string.IsNullOrEmpty(Country);
                bool isCityEmpty = string.IsNullOrEmpty(City);
                bool isStreetEmpty = string.IsNullOrEmpty(Street);
                bool isBuildingNumerEmpty = string.IsNullOrEmpty(BuildingNumber);
                //bool isFlatNumberEmpty = string.IsNullOrEmpty(FlatNumber);
                bool isZipCodeEmpty = string.IsNullOrEmpty(ZipCode);


                return !isNameEmpty && !isSurnameEmpty && !isCityEmpty && !isStreetEmpty 
                    && !isBuildingNumerEmpty /*&& !isFlatNumberEmpty*/ && !isZipCodeEmpty && !isCountryEmpty;
            }
        }

        private IDataProvider<Profile> _profileService;
        private Geocoder geocoder;
        public EditProfileVM()
        {
            _profileService = DependencyService.Get<IDataProvider<Profile>>();
            geocoder = new Geocoder();
            SaveCommand = new Command<bool>(Save, CanSave);
            TryLocateMeCommand = new Command(TrySetEntriesUsingLocation);
        }


        private async void Save(bool param)
        {
            string addressString = $"{Street} {BuildingNumber} {ZipCode} {City} {Country}";

            try
            {
                var locations = await geocoder.GetPositionsForAddressAsync(addressString);

                var position = locations?.FirstOrDefault();
                if (position == null)
                {
                    await App.Current.MainPage.DisplayPromptAsync("Error", "Could not find localization of provided address");
                    return;
                }
                else
                {
                    Profile profile = await _profileService.GetByUserId(Auth.GetCurrentUserId());

                    profile.UserName = UserName;
                    profile.UserSurname = UserSurname;
                    profile.Country = Country;
                    profile.City = City;
                    profile.Street = Street;
                    profile.BuildingNumber = BuildingNumber;
                    profile.FlatNumber = FlatNumber;
                    profile.ZipCode = ZipCode;
                    profile.Lat = position.Value.Latitude;
                    profile.Lon = position.Value.Longitude;
                    profile.IsConfirmed = true;

                    await _profileService.UpdateItemAsync(profile);

                    await Shell.Current.GoToAsync("..");
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayPromptAsync("Error", ex.Message);
                return;
            }
        }

        private bool CanSave(bool param) => param;


        public async void TrySetEntriesUsingLocation()
        {
            var status = await GeoHelpers.CheckAndRequestPermisionsForLocation();

            if (status == PermissionStatus.Granted)
            {
                try
                {
                    var location = await GeoHelpers.GetDeviceLocation();
                    if (location != null)
                    {
                        var placemarks = await Geocoding.GetPlacemarksAsync(location);
                        var placemark = placemarks?.FirstOrDefault();

                        if (placemark != null)
                        {
                            Country = placemark.CountryName;
                            BuildingNumber = placemark.SubThoroughfare;
                            ZipCode = placemark.PostalCode;
                            Street = placemark.Thoroughfare;
                            City = placemark.Locality;
                        }

                        await App.Current.MainPage.DisplayAlert("Done", $"Looks like you are in {placemark.Locality}", "Ok");
                    }
                }
                catch (Exception ex)
                {
                    // Handle not supported on device exception
                    await App.Current.MainPage.DisplayAlert("Error", $"Could not find address. Please insert information manually. \nMessage: {ex.Message}", "Ok");
                }
                
            }
        }

        public async void LoadProfileInfo()
        {
            Profile profile = await _profileService.GetByUserId(Auth.GetCurrentUserId());
            if (profile.IsConfirmed)
            {
                UserName = profile.UserName;
                UserSurname = profile.UserSurname;
                Country = profile.Country;
                City = profile.City;
                Street = profile.Street;
                BuildingNumber = profile.BuildingNumber;
                FlatNumber = profile.FlatNumber;
                ZipCode = profile.ZipCode;
                FlatNumber = profile.FlatNumber;
            }
            
        }




    }
}
