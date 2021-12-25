using System;
using System.Collections.Generic;
using System.IO;
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
    [QueryProperty(nameof(DogId), nameof(DogId))]
    public partial class DetailsDogPage : ContentPage
    {

        public string DogId { get; set; }
        private IDataService<Dog> _dogService;
        public DetailsDogPage()
        {
            InitializeComponent();
            _dogService = DependencyService.Get<IDataProvider<Dog>>();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            var dog = await _dogService.GetItemAsync(DogId);    
            dogImage.Source = ImageSource.FromStream(() => new MemoryStream(dog.PhotoContent));

            nameLabel.Text = $"Name: {dog.Name}";
            raceLabel.Text = $"Race: {dog.Race}";
            weightLabel.Text = $"Weight: {dog.Weight} [kg]";

        }

        private void AcceptButton_Clicked(object sender, EventArgs e)
        {

        }
    }
}