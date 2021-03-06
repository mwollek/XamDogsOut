using MvvmHelpers;
using System.ComponentModel;
using Xamarin.Forms;
using XamDogsOut.Services;
using XamDogsOut.Views;

namespace XamDogsOut.ViewModels
{

    public class AuthVM : BaseViewModel
    {

        public Command LoginCommand { get; set; }
        public Command RegisterNavigationCommand { get; set; }

        private string email = "mw@gmail.com";
        public string Email 
        {
            get => email;
            set 
            {
                SetProperty(ref email, value);
                OnPropertyChanged("EntriesHaveText"); 
            } 
        }

        private string password = "123123";
        public string Password 
        {
            get => password;
            set 
            {
                SetProperty(ref password, value);
                OnPropertyChanged("EntriesHaveText"); 
            } 
        }
        public bool EntriesHaveText
        {
            get
            {
                bool isEmailEmpty = string.IsNullOrEmpty(Email);
                bool isPasswordEmpty = string.IsNullOrEmpty(Password);

                return !isEmailEmpty && !isPasswordEmpty;
            }
        }

        public AuthVM()
        {
            LoginCommand = new Command<bool>(Login, CanLogin);
            RegisterNavigationCommand = new Command(RegisterNavigation);
        }


        public async void RegisterNavigation()
        {
            await App.Current.MainPage.Navigation.PushAsync(new RegisterPage());
        }
        public async void Login(bool parameter)
        {
            bool result = await Auth.LoginUserAsync(Email, Password);
            if (result)
            {
                await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
            }
        }
        private bool CanLogin(bool parameter)
        {
            return EntriesHaveText;
        }
    }
}

