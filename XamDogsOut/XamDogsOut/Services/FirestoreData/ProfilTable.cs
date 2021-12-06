using Plugin.CloudFirestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamDogsOut.Models;

namespace XamDogsOut.Services.FirestoreData
{
    public class ProfileTable : IDataProvider<Profile>
    {
        public async Task<string> AddItemAsync(Profile item)
        {
            return (await CrossCloudFirestore.Current
                         .Instance
                         .Collection("profils")
                         .AddAsync(item)).Id;
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            await CrossCloudFirestore.Current
                         .Instance
                         .Collection("profils")
                         .Document(id)
                         .DeleteAsync();
            return true;
        }

        public async Task<Profile> GetByUserId(string userId)
        {
            var items = await GetItemsAsync();
            return items.FirstOrDefault(x => x.UserId == userId);
        }

        public async Task<Profile> GetItemAsync(string id)
        {
            var document = await CrossCloudFirestore.Current
                                        .Instance
                                        .Collection("profils")
                                        .Document(id)
                                        .GetAsync();

            return document.ToObject<Profile>();
        }

        public async Task<IEnumerable<Profile>> GetItemsAsync(bool forceRefresh = false)
        {
            var group = await CrossCloudFirestore.Current
                                     .Instance
                                     .CollectionGroup("profils")
                                     .GetAsync();


            return group.ToObjects<Profile>();
        }

        public async Task<bool> UpdateItemAsync(Profile item)
        {
            await CrossCloudFirestore.Current
                         .Instance
                         .Collection("profils")
                         .Document(item.Id)
                         .UpdateAsync(new 
                         { 
                             UserName = item.UserName, 
                             UserSurname = item.UserSurname,
                             Country = item.Country,
                             City = item.City,
                             Street = item.Street,
                             BuildingNumber = item.BuildingNumber,
                             FlatNumber = item.FlatNumber,
                             ZipCode = item.ZipCode,
                             Lat = item.Lat,
                             Lon = item.Lon,
                             IsConfirmed = item.IsConfirmed
                         });

            return true;
        }
    }
}
