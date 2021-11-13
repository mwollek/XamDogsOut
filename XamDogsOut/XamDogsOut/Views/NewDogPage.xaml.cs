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

namespace XamDogsOut.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewDogPage : ContentPage
    {
        private IDataProvider<Dog> dogTable;
        private byte[] newPhotoArray;
        private bool userHasDog;
        private Dog usersDog;
        public NewDogPage()
        {
            InitializeComponent();
            dogTable = DependencyService.Get<IDataProvider<Dog>>();
            
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            var dogs = await dogTable.GetItemsAsync();
            userHasDog = dogs.Any(x => x.UserId == Auth.GetCurrentUserId());
            if (userHasDog)
            {
                usersDog = dogs.Where(x => x.UserId == Auth.GetCurrentUserId()).FirstOrDefault();
                //selectedPhotoArray = usersDog.PhotoContent;

                if (newPhotoArray != null)
                    dogImageButton.Source = ImageSource.FromStream(() => new MemoryStream(newPhotoArray));
                else
                    dogImageButton.Source = ImageSource.FromStream(() => new MemoryStream(usersDog.PhotoContent));

                nameEntry.Text = usersDog.Name;
                weightEntry.Text = usersDog.Weight.ToString();
                raceEntry.Text = usersDog.Race;
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
                await dogTable.UpdateItemAsync(dog);
            }
            else
            {
                await dogTable.AddItemAsync(dog);
            }

            await DisplayAlert("Done", "Succesfully", "Ok");
            await Shell.Current.GoToAsync($"//{nameof(MapPage)}");
            
        }
        private async void dogImageButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                var result = await FilePicker.PickAsync(new PickOptions());
                if (result != null)
                {
                    var resultToStream = await result.OpenReadAsync();
                    dogImageButton.Source = ImageSource.FromStream(x => result.OpenReadAsync());
                    using (var ms = new MemoryStream())
                    {
                        resultToStream.CopyTo(ms);
                        newPhotoArray = ms.ToArray();
                    }              
                }
            }
            catch (Exception ex)
            {
                
            }

        }
    }
}