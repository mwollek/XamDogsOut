using Plugin.CloudFirestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamDogsOut.Models;

namespace XamDogsOut.Services.FirestoreData
{
    public class AcceptedRequestTable : IDataProvider<AcceptedRequest>
    {
        public async Task<string> AddItemAsync(AcceptedRequest item)
        {
            return (await CrossCloudFirestore.Current
                         .Instance
                         .Collection("acceptedRequests")
                         .AddAsync(item)).Id;
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            await CrossCloudFirestore.Current
                         .Instance
                         .Collection("acceptedRequests")
                         .Document(id)
                         .DeleteAsync();
            return true;
        }

        public async Task<AcceptedRequest> GetByUserId(string userId)
        {
            var items = await GetItemsAsync();
            return items.FirstOrDefault(x => x.SenderId == userId);
        }

        public async Task<AcceptedRequest> GetItemAsync(string id)
        {
            var document = await CrossCloudFirestore.Current
                                        .Instance
                                        .Collection("acceptedRequests")
                                        .Document(id)
                                        .GetAsync();

            return document.ToObject<AcceptedRequest>();
        }

        public async Task<IEnumerable<AcceptedRequest>> GetItemsAsync(bool forceRefresh = false)
        {
            var group = await CrossCloudFirestore.Current
                                      .Instance
                                      .CollectionGroup("acceptedRequests")
                                      .GetAsync();


            return group.ToObjects<AcceptedRequest>();
        }
        public async Task<bool> UpdateItemAsync(AcceptedRequest item)
        {
            await CrossCloudFirestore.Current
                         .Instance
                         .Collection("acceptedRequests")
                         .Document(item.Id)
                         .UpdateAsync(new { SenderId = item.SenderId, ExecutorId = item.ExecutorId, Status = item.Status });

            return true;
        }

        
    }
}
