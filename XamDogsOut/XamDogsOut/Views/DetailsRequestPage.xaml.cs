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
    [QueryProperty(nameof(RequestId), nameof(RequestId))]

    public partial class DetailsRequestPage : ContentPage
    {

        private byte isButtonClickable;

        public string DogId { get; set; }
        public string RequestId { get; set; }

        private IDataService<Dog> _dogService;
        private IDataService<AcceptedRequest> _acceptedRequestService;
        private IDataService<Request> _requestService;



        public DetailsRequestPage()
        {
            InitializeComponent();
            _dogService = DependencyService.Get<IDataProvider<Dog>>();
            _requestService = DependencyService.Get<IDataProvider<Request>>();
            _acceptedRequestService = DependencyService.Get<IDataProvider<AcceptedRequest>>();

            isButtonClickable = 1;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await PreparePage();




        }

        private async Task PreparePage()
        {
            var dog = await _dogService.GetItemAsync(DogId);
            dogImage.Source = ImageSource.FromStream(() => new MemoryStream(dog.PhotoContent));

            nameLabel.Text = $"Name: {dog.Name}";
            raceLabel.Text = $"Race: {dog.Race}";
            weightLabel.Text = $"Weight: {dog.Weight} [kg]";

            var acceptedRequests = await _acceptedRequestService.GetItemsAsync();
            var acceptedUserRequests = acceptedRequests
                .Where(x => x.RequestId == this.RequestId)
                .Where(x => x.ExecutorId == Auth.GetCurrentUserId()).ToList();
            if (acceptedUserRequests.Any())
                SetButtonPropsWhenRequestAccepted();

        }

        private async void AcceptButton_Clicked(object sender, EventArgs e)
        {
            if (isButtonClickable == 1)
            {
                var dog = await _dogService.GetItemAsync(DogId);

                var accepetedRequest = new AcceptedRequest()
                {
                    ExecutorId = Auth.GetCurrentUserId(),
                    SenderId = dog.UserId,
                    RequestId = RequestId,
                };

                await _acceptedRequestService.AddItemAsync(accepetedRequest);
                SetButtonPropsWhenRequestAccepted();
            }
            

        }

        private void SetButtonPropsWhenRequestAccepted()
        {
            AcceptButton.Text = "Request sent";
            AcceptButton.BackgroundColor = Color.OrangeRed;
            isButtonClickable = 0;
        }
    }
}