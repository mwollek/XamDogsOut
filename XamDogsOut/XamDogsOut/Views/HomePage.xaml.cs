using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamDogsOut.Models;
using XamDogsOut.Services;

namespace XamDogsOut.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : TabbedPage
    {
        IDataProvider<Profile> _profileService;
        public HomePage()
        {
            _profileService = DependencyService.Get<IDataProvider<Profile>>();
            InitializeComponent();
            
        }

        protected async override void OnAppearing()
        {

            base.OnAppearing();
            await CheckIfUserHasProfileConfirmed();

        }

        private async Task CheckIfUserHasProfileConfirmed()
        {
            var profile = await _profileService.GetByUserId(Auth.GetCurrentUserId());

            if (!profile.IsConfirmed)
                await Shell.Current.Navigation.PushAsync(new EditProfilePage());
        }
    }
}