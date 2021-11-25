using MvvmHelpers;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamDogsOut.Models;
using XamDogsOut.Services;
using XamDogsOut.Views;

namespace XamDogsOut.ViewModels
{
    public class DogVM : BaseViewModel
    {
        private byte[] newPhotoArray;
        private bool isEditing = false;


        public Command PickPhotoCommand { get; set; }
        public Command SaveCommand { get; set; }


        private ImageSource dogPhoto;
        public ImageSource DogPhoto
        {
            get => dogPhoto;
            set
            {
                SetProperty(ref dogPhoto, value);
                OnPropertyChanged(nameof(ModelIsReady));


            }
        }
        private string name;
        public string Name
        {
            get => name;
            set
            {
                SetProperty(ref name, value);
                OnPropertyChanged(nameof(ModelIsReady));

            }
        }

        private int? weight;
        public int? Weight
        {
            get => weight;
            set
            {
                SetProperty(ref weight, value);
                OnPropertyChanged(nameof(ModelIsReady));

            }
        }

        private string race;
        public string Race
        {
            get => race;
            set
            {
                SetProperty(ref race, value);
                OnPropertyChanged(nameof(ModelIsReady));
            }
        }

        public bool ModelIsReady
        {
            get
            {
                bool isRaceEmpty = string.IsNullOrEmpty(Race);
                bool isNameEmpty = string.IsNullOrEmpty(Name);
                bool isWeightEmpty = !Weight.HasValue;
                bool isPhotoEmpty = DogPhoto == null;

                return !isRaceEmpty && !isNameEmpty && !isWeightEmpty && !isPhotoEmpty;
            }
        }


        private IDataProvider<Dog> _dogService;

        public DogVM()
        {
            SaveCommand = new Command<bool>(Save, CanSave);
            PickPhotoCommand = new Command(PickPhoto);


            _dogService = DependencyService.Get<IDataProvider<Dog>>();
        }

        private async void Save(bool param)
        {
            Dog dog = new Dog()
            {
                Name = Name,
                Race = Race,
                Weight = Weight,
                PhotoContent = newPhotoArray,
                UserId = Auth.GetCurrentUserId()
            };
            if (await IsUserHasDog())
            {
                var usersDog = await _dogService.GetByUserId(Auth.GetCurrentUserId());
                dog.Id = usersDog.Id;
                await _dogService.UpdateItemAsync(dog);
            }
            else
            {
                await _dogService.AddItemAsync(dog);
            }

            string mess = !await IsUserHasDog() ? "added" : "updated";
            await App.Current.MainPage.DisplayAlert("Done", $"Succesfully { mess }", "Ok");
            await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
        }
        private bool CanSave(bool param) => param;

        private async void PickPhoto()
        {
            isEditing = true;
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await Application.Current.MainPage.DisplayAlert("Not suported", "Acces to this functionality is denied ", "Ok");
                return;
            }

            var mediaOptions = new PickMediaOptions()
            {
                PhotoSize = PhotoSize.Medium
            };

            var selectedImageFile = await CrossMedia.Current.PickPhotoAsync(mediaOptions);
            if (selectedImageFile == null)
            {
                await Application.Current.MainPage.DisplayAlert("Message", "Photo was not picked or could not be found.", "Ok");
                return;
            }
            DogPhoto = ImageSource.FromStream(() => selectedImageFile.GetStream());
            using (var ms = new MemoryStream())
            {
                selectedImageFile.GetStream().CopyTo(ms);
                newPhotoArray = ms.ToArray();
            }
        }

        public async void GetDogInfo()
        {
            var dogs = await _dogService.GetItemsAsync();
            if (await IsUserHasDog())
            {
                var usersDog = await _dogService.GetByUserId(Auth.GetCurrentUserId());
                if (newPhotoArray != null)
                    DogPhoto = ImageSource.FromStream(() => new MemoryStream(newPhotoArray));
                else
                    DogPhoto = ImageSource.FromStream(() => new MemoryStream(usersDog.PhotoContent));

                if (!isEditing)
                {
                    Name = usersDog.Name;
                    Weight = usersDog.Weight;
                    Race = usersDog.Race;
                }

            }
        }

        private async Task<bool> IsUserHasDog()
        {
            return (await _dogService.GetItemsAsync()).Any(x => x.UserId == Auth.GetCurrentUserId());
        }
    }
}
