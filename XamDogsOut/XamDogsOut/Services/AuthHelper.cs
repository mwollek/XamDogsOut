using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamDogsOut.Services
{
    public interface IAuth
    {
        Task<bool> RegisterUserAsync(string email, string password);
        bool IsAuthenticated();
        string GetCurrentUserId();
        Task<bool> LoginUserAsync(string email, string password);
    }


    public class Auth
    {
        private static IAuth auth = DependencyService.Get<IAuth>();
        public static async Task<bool> RegisterUserAsync(string email, string password)
        {
            try
            {
                return await auth.RegisterUserAsync(email, password);
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
                return false;
            }

        }

        public static async Task<bool> LoginUserAsync(string email, string password)
        {
            try
            {
                return await auth.LoginUserAsync(email, password);
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
                return false;
            }
        }

        public static bool IsAuthenticated()
        {
            return auth.IsAuthenticated();
        }

        public static string GetCurrentUserId()
        {
            return auth.GetCurrentUserId();
        }

    }
}
