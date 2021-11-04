using System.ComponentModel;
using Xamarin.Forms;
using XamDogsOut.Services;
using XamDogsOut.Views;

namespace XamDogsOut.ViewModels
{

    public class AuthVM : INotifyPropertyChanged
    {

        public Command LoginCommand { get; set; }
        public Command RegisterNavigationCommand { get; set; }

        private string email;
        public string Email { get { return email; } set { email = value; OnPropertyChanged("EntriesHaveText"); } }

        private string password;
        public string Password { get { return password; } set { password = value; OnPropertyChanged("EntriesHaveText"); } }

        private bool entriesHaveText;
        public bool EntriesHaveText
        {
            get
            {
                bool isEmailEmpty = string.IsNullOrEmpty(Email);
                bool isPasswordEmpty = string.IsNullOrEmpty(Password);

                return !isEmailEmpty && !isPasswordEmpty;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
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
                await App.Current.MainPage.Navigation.PushAsync(new MapPage());
            }
        }
        private bool CanLogin(bool parameter)
        {
            return EntriesHaveText;
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

