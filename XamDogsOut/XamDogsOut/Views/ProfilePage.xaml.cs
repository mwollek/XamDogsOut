using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamDogsOut.ViewModels;

namespace XamDogsOut.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        private ProfileVM vm;

        public ProfilePage()
        { 
            InitializeComponent();
            vm = Resources["vm"] as ProfileVM;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            vm.LoadProfileInfo();
        }
    }
}