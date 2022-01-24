using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using XamDogsOut.Models;
using XamDogsOut.Services;

namespace XamDogsOut.ViewModels
{
    class RequestInfoHelperModel
    {
        public string RequestId { get; set; }

        public string ExecutorId { get; set; }
        public string SenderId { get; set; }


        public string ExecutorFullName { get; set; }
        public string Address { get; set; }



        // helpers command 

        public Xamarin.Forms.Command SetRequestDoneCommand { get; set; }

        private IDataProvider<Request> _requestService;
        private IDataProvider<AcceptedRequest> _acceptedRequestService;

        public RequestInfoHelperModel()
        {
            SetRequestDoneCommand = new Xamarin.Forms.Command(SetRequestDone);


            _requestService = DependencyService.Get<IDataProvider<Request>>();
            _acceptedRequestService = DependencyService.Get<IDataProvider<AcceptedRequest>>();
        }

        private async void SetRequestDone()
        {
            var request = await _requestService.GetItemAsync(RequestId);

            request.ExecutorId = ExecutorId;
            request.Status = RequestStatuses.Done;

            await _requestService.UpdateItemAsync(request);

            var reletedAcceptedRequests = (await _acceptedRequestService.GetItemsAsync())
                .Where(x => x.SenderId == Auth.GetCurrentUserId()).ToList();

            foreach (var reletedAcceptedRequest in reletedAcceptedRequests)
            {
                await _acceptedRequestService.DeleteItemAsync(reletedAcceptedRequest.Id);
            }
        }


    }


    class RequestInfoVM : BaseViewModel
    {
        public Xamarin.Forms.Command DeleteRequestCommand { get; set; }

        public ObservableCollection<RequestInfoHelperModel> AcceptedRequests { get; set; }


        private IDataProvider<Request> _requestService;
        private IDataProvider<AcceptedRequest> _acceptedRequestService;
        private IDataProvider<Profile> _profileService;


        public RequestInfoVM()
        {
            AcceptedRequests = new ObservableCollection<RequestInfoHelperModel>();

            DeleteRequestCommand = new Xamarin.Forms.Command(DeleteRequest);




            _requestService = DependencyService.Get<IDataProvider<Request>>();
            _acceptedRequestService = DependencyService.Get<IDataProvider<AcceptedRequest>>();
            _profileService = DependencyService.Get<IDataProvider<Profile>>();


        }


        public async void GetRequests()
        {
            AcceptedRequests.Clear();
            var requests = (await _acceptedRequestService.GetItemsAsync())
                .Where(x => x.SenderId == Auth.GetCurrentUserId()).ToList();

            Header = requests.Count > 0 ? "List of users intrested in your request." : "No one has accepted your request yet.";

            var executorsIds = requests.Select(x => x.ExecutorId).ToArray();

            var executors = (await _profileService.GetItemsAsync()).Where(x => executorsIds.Contains(x.UserId)).ToList();

            foreach (var request in requests)
            {
                var executor = executors.FirstOrDefault(x => x.UserId == request.ExecutorId);

                var requestHelper = new RequestInfoHelperModel()
                {
                    ExecutorId = request.ExecutorId,
                    SenderId = request.SenderId,

                    ExecutorFullName = $"{executor.UserName} {executor.UserSurname}",
                    Address = $"{executor.Street} {executor.BuildingNumber} {executor.City}",

                    RequestId = request.RequestId
                };


                AcceptedRequests.Add(requestHelper);
            }
        }

        private async void DeleteRequest()
        {
            var idToDelete = (await _requestService.GetItemsAsync())
                .Where(x => x.SenderId == Auth.GetCurrentUserId())
                .Select(x => x.Id)
                .FirstOrDefault();

            await _requestService.DeleteItemAsync(idToDelete);


            var reletedAcceptedRequests = (await _acceptedRequestService.GetItemsAsync())
                .Where(x => x.SenderId == Auth.GetCurrentUserId()).ToList();

            foreach (var reletedAcceptedRequest in reletedAcceptedRequests)
            {
                await _acceptedRequestService.DeleteItemAsync(reletedAcceptedRequest.Id);
            }

            await Shell.Current.GoToAsync("..");
        }

        

    }
}
