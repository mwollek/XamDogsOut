using System;
using System.ComponentModel;
using Xamarin.Forms;
using XamDogsOut.Services;
using XamDogsOut.Views;

namespace XamDogsOut.ViewModels
{
    public class RegisterVM : INotifyPropertyChanged
    {

        public Command RegisterCommand { get; set; }

        private string email;
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                OnPropertyChanged("Email");
                OnPropertyChanged("RegisterIsReady");
            }
        }

        private string password;
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged("Password");
                OnPropertyChanged("RegisterIsReady");
            }
        }

        private string confirmedPassword;
        public string ConfirmedPassword
        {
            get { return confirmedPassword; }
            set
            {
                confirmedPassword = value;
                OnPropertyChanged("ConfirmedPassword");
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
                {
                    return Password.Equals(ConfirmedPassword);

                }
                return false;
            }
        }

        public bool RegisterIsReady
        {
            get
            {
                return PasswordsMatches && EntriesHaveText;
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public RegisterVM()
        {
            RegisterCommand = new Command<bool>(Register, CanRegister);
        }



        private bool CanRegister(bool param)
        {
            return param;
        }

        private async void Register(bool param)
        {
            bool result = await Auth.RegisterUserAsync(Email, Password);
            if (result)
            {
                await App.Current.MainPage.Navigation.PushAsync(new MapPage());
            }
        }

        private void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
