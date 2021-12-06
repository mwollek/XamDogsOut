using Plugin.CloudFirestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using XamDogsOut.Models;

namespace XamDogsOut.Services
{
    public class DogTable : IDataProvider<Dog>
    {
        public async Task<string> AddItemAsync(Dog item)
        {
            return (await CrossCloudFirestore.Current
                         .Instance
                         .Collection("dogs")
                         .AddAsync(item)).Id;
           
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            await CrossCloudFirestore.Current
                         .Instance
                         .Collection("dogs")
                         .Document(id)
                         .DeleteAsync();
            return true;
        }

        public async Task<Dog> GetByUserId(string userId)
        {
            var items = await GetItemsAsync();
            return items.FirstOrDefault(x => x.UserId == userId);
        }

        public async Task<Dog> GetItemAsync(string id)
        {
            var document = await CrossCloudFirestore.Current
                                        .Instance
                                        .Collection("dogs")
                                        .Document(id)
                                        .GetAsync();

            return document.ToObject<Dog>();
        }

        public async Task<IEnumerable<Dog>> GetItemsAsync(bool forceRefresh = false)
        {
            var group = await CrossCloudFirestore.Current
                                     .Instance
                                     .CollectionGroup("dogs")
                                     .GetAsync();
            

            return group.ToObjects<Dog>();
        }

        public async Task <bool> UpdateItemAsync(Dog item)
        {
            await CrossCloudFirestore.Current
                         .Instance
                         .Collection("dogs")
                         .Document(item.Id)
                         .UpdateAsync(new { Name = item.Name, Weight = item.Weight, Race = item.Race, PhotoContent = item.PhotoContent});

            return true;
        }
    }
}
