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

namespace XamDogsOut.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewDogPage : ContentPage
    {
        private IDataProvider<Dog> _dogService;
        private byte[] newPhotoArray;
        private bool userHasDog;
        private Dog usersDog;
        public NewDogPage()
        {
            InitializeComponent();
            _dogService = DependencyService.Get<IDataProvider<Dog>>();
            
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            var dogs = await _dogService.GetItemsAsync();
            userHasDog = dogs.Any(x => x.UserId == Auth.GetCurrentUserId());
            if (userHasDog)
            {
                usersDog = await _dogService.GetByUserId(Auth.GetCurrentUserId());

                if (newPhotoArray != null)
                    dogImageButton.Source = ImageSource.FromStream(() => new MemoryStream(newPhotoArray));
                else
                    dogImageButton.Source = ImageSource.FromStream(() => new MemoryStream(usersDog.PhotoContent));

                nameEntry.Text = usersDog.Name;
                weightEntry.Text = usersDog.Weight.ToString();
                raceEntry.Text = usersDog.Race;
            }
            else
            {
                nameEntry.Text = string.Empty;
                weightEntry.Text = string.Empty;
                raceEntry.Text = string.Empty;
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            dogImageButton.Source = null;
            newPhotoArray = null;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            Dog dog = new Dog()
            {
                Name = nameEntry.Text,
                Race = raceEntry.Text,
                Weight = int.Parse(weightEntry.Text ?? "0"),
                PhotoContent = newPhotoArray,
                UserId = Auth.GetCurrentUserId()
            };
            if (userHasDog)
            {
                dog.Id = usersDog.Id;
                await _dogService.UpdateItemAsync(dog);
            }
            else
            {
                await _dogService.AddItemAsync(dog);
            }

            string mess = !userHasDog ? "added" : "updated";
            await DisplayAlert("Done", $"Succesfully { mess }", "Ok");
            await Shell.Current.GoToAsync($"//{nameof(MapPage)}");
            
        }
        private async void dogImageButton_Clicked(object sender, EventArgs e)
        {

            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Not suported", "Acces to this functionality is denied ", "ok");
                return;
            }

            var mediaOptions = new PickMediaOptions()
            {
                PhotoSize = PhotoSize.Medium
            };

            var selectedImageFile = await CrossMedia.Current.PickPhotoAsync(mediaOptions);
            if (selectedImageFile == null)
            {
                await DisplayAlert("Error", "Could not find photo", "ok");
                return;
            }
            dogImageButton.Source = ImageSource.FromStream(() => selectedImageFile.GetStream());
            using (var ms = new MemoryStream())
            {
                selectedImageFile.GetStream().CopyTo(ms);
                newPhotoArray = ms.ToArray();
            }

        }
    }
}