using MvvmHelpers;
using Xamarin.Forms;
using XamDogsOut.Models;
using XamDogsOut.Services;
using XamDogsOut.Views;

namespace XamDogsOut.ViewModels
{
    public class ProfileVM : BaseViewModel
    {
        public Command LoadProfileInfoCommand { get; set; }
        public Command NavigateToEditCommand { get; set; }

        private string userName;
        public string UserName
        {
            get => userName;
            set
            {
                SetProperty(ref userName, value);

            }
        }
        private string userSurname;
        public string UserSurname
        {
            get => userSurname;
            set
            {
                SetProperty(ref userSurname, value);

            }
        }

        private string country;
        public string Country
        {
            get => country;
            set
            {
                SetProperty(ref country, value);

            }
        }

        private string city;
        public string City
        {
            get => city;
            set
            {
                SetProperty(ref city, value);

            }
        }
        private string street;
        public string Street
        {
            get => street;
            set
            {
                SetProperty(ref street, value);

            }
        }
        private string buildingNumber;
        public string BuildingNumber
        {
            get => buildingNumber;
            set
            {
                SetProperty(ref buildingNumber, value);

            }
        }
        private string flatNumber;
        public string FlatNumber
        {
            get => flatNumber;
            set
            {
                SetProperty(ref flatNumber, value);

            }
        }
        private string zipCode;
        public string ZipCode
        {
            get => zipCode;
            set
            {
                SetProperty(ref zipCode, value);

            }
        }

        private IDataProvider<Profile> _profileService;
        public ProfileVM()
        {
            _profileService = DependencyService.Get<IDataProvider<Profile>>();
            LoadProfileInfoCommand = new Command(LoadProfileInfo);
            NavigateToEditCommand = new Command(NavigateToEdit);

        }

        private async void NavigateToEdit()
        {
            await Shell.Current.GoToAsync(nameof(EditProfilePage));
        }
        public async void LoadProfileInfo()
        {
            Profile profile = await _profileService.GetByUserId(Auth.GetCurrentUserId());

            UserName = profile.UserName;
            UserSurname = profile.UserSurname;
            Country = profile.Country;
            City = profile.City;
            Street = profile.Street;
            BuildingNumber = profile.BuildingNumber;
            FlatNumber = profile.FlatNumber;
            ZipCode = profile.ZipCode;
            FlatNumber = profile.FlatNumber ?? "-";
        }
    }
}
