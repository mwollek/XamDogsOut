using Plugin.CloudFirestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamDogsOut.Models;

namespace XamDogsOut.Services.FirestoreData
{
    public class RequestTable : IDataProvider<Request>
    {
        public async Task<string> AddItemAsync(Request item)
        {
            return (await CrossCloudFirestore.Current
                         .Instance
                         .Collection("requests")
                         .AddAsync(item)).Id;
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            await CrossCloudFirestore.Current
                         .Instance
                         .Collection("requests")
                         .Document(id)
                         .DeleteAsync();
            return true;
        }

        public async Task<Request> GetByUserId(string userId)
        {
            var items = await GetItemsAsync();
            return items.FirstOrDefault(x => x.SenderId == userId && x.Status == RequestStatuses.StandBy);
        }

        public async Task<Request> GetItemAsync(string id)
        {
            var document = await CrossCloudFirestore.Current
                                        .Instance
                                        .Collection("requests")
                                        .Document(id)
                                        .GetAsync();

            return document.ToObject<Request>();
        }

        public async Task<IEnumerable<Request>> GetItemsAsync(bool forceRefresh = false)
        {
            var group = await CrossCloudFirestore.Current
                                      .Instance
                                      .CollectionGroup("requests")
                                      .GetAsync();


            return group.ToObjects<Request>();
        }

        public async Task<IEnumerable<Request>> GetStandByItemsAsync(bool forceRefresh = false)
        {
            var group = await CrossCloudFirestore.Current
                                      .Instance
                                      .CollectionGroup("requests")
                                      .WhereEqualsTo("Status", RequestStatuses.StandBy)
                                      .GetAsync();


            return group.ToObjects<Request>();
        }

        public async Task<bool> UpdateItemAsync(Request item)
        {
            await CrossCloudFirestore.Current
                         .Instance
                         .Collection("requests")
                         .Document(item.Id)
                         .UpdateAsync(new { SenderId = item.SenderId, ExecutorId = item.ExecutorId, Status = item.Status});

            return true;
        }

        public async Task<bool> UpdateItemStatusAsync(string itemId, int status)
        {
            await CrossCloudFirestore.Current
                         .Instance
                         .Collection("requests")
                         .Document(itemId)
                         .UpdateAsync(new { Status = status });

            return true;
        }


    }
}
