using MvvmHelpers;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using XamDogsOut.Models;
using XamDogsOut.Services;
using XamDogsOut.Views;

namespace XamDogsOut.ViewModels
{
    public class RegisterVM : BaseViewModel
    {

        

        public Command RegisterCommand { get; set; }

        private string email;
        public string Email
        {
            get => email;
            set
            {
                SetProperty(ref email, value);
                OnPropertyChanged("RegisterIsReady");
            }
        }

        private string password;
        public string Password
        {
            get => password;
            set
            {
                SetProperty(ref password, value);
                OnPropertyChanged("RegisterIsReady");
            }
        }

        private string confirmedPassword;
        public string ConfirmedPassword
        {
            get => confirmedPassword;
            set
            {
                SetProperty(ref confirmedPassword, value);
                OnPropertyChanged("RegisterIsReady");
            }
        }

        public bool EntriesHaveText
        {
            get
            {
                bool isEmailEmpty = string.IsNullOrEmpty(Email);
                bool isPasswordEmpty = string.IsNullOrEmpty(Password);
                bool isConfirmedPasswordEmpty = string.IsNullOrEmpty(ConfirmedPassword);

                return !isEmailEmpty && !isPasswordEmpty && !isConfirmedPasswordEmpty;
            }
        }
        public bool PasswordsMatches
        {
            get
            {
                if (Password != null && Email != null)
                    return Password.Equals(ConfirmedPassword);

                return false;
            }
        }

        public bool RegisterIsReady => PasswordsMatches && EntriesHaveText;



        private IDataProvider<Profile> _profileService;
        public RegisterVM()
        {
            _profileService = DependencyService.Get<IDataProvider<Profile>>();
            RegisterCommand = new Command<bool>(Register, CanRegister);
        }

        private bool CanRegister(bool param) => param;

        private async void Register(bool param)
        {
            bool result = await Auth.RegisterUserAsync(Email, Password);
            if (result)
            {
                Profile profile = new Profile()
                {
                    UserId = Auth.GetCurrentUserId(),
                    IsConfirmed = false
                };
                await _profileService.AddItemAsync(profile);


                await App.Current.MainPage.Navigation.PushAsync(new HomePage());
            }

            
        }
    }
}
