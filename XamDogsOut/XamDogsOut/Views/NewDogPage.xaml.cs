using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamDogsOut.Models;
using XamDogsOut.Services;
using System.Drawing;
using Plugin.Media;
using Plugin.Media.Abstractions;
using XamDogsOut.ViewModels;

namespace XamDogsOut.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewDogPage : ContentPage
    {
        
        private byte[] newPhotoArray;
        private bool userHasDog;
        private Dog usersDog;

        private DogVM vm;
        public NewDogPage()
        {
            InitializeComponent();
            vm = Resources["vm"] as DogVM;
               
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            vm.GetDogInfo();
            
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            //dogImageButton.Source = null;
            //newPhotoArray = null;
        }

        
        
    }
}