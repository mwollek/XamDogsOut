using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamDogsOut.Models;
using XamDogsOut.Services;
using XamDogsOut.ViewModels;

namespace XamDogsOut.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RequestInfoPage : ContentPage
    {

        

        private RequestInfoVM vm;

        public RequestInfoPage()
        {
            InitializeComponent();

            vm = Resources["vm"] as RequestInfoVM;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            vm.GetRequests();
        }

        
    }
}