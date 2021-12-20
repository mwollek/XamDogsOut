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
    public partial class RequestInfoPage : ContentPage
    {

        private IDataProvider<Request> _requestService;
        public RequestInfoPage()
        {
            _requestService = DependencyService.Get<IDataProvider<Request>>();
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var idToDelete = (await _requestService.GetItemsAsync()).Where(x => x.SenderId == Auth.GetCurrentUserId()).Select(x => x.Id).FirstOrDefault();

            await _requestService.DeleteItemAsync(idToDelete);

            await Shell.Current.GoToAsync("..");
        }
    }
}