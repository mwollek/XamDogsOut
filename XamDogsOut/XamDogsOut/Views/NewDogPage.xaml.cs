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
    public partial class NewDogPage : ContentPage
    {
        private IDataProvider<Dog> dogTable;
        public NewDogPage()
        {
            InitializeComponent();
            dogTable = DependencyService.Get<IDataProvider<Dog>>();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            var query = await dogTable.GetItemsAsync();
            var dogs = query.ToList();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {

            Dog dog = new Dog()
            {
                Name = nameEntry.Text,
                Weight = Int32.Parse(weightEntry.Text),
                Race = raceEntry.Text
            };
            await dogTable.AddItemAsync(dog);
        }
    }
}