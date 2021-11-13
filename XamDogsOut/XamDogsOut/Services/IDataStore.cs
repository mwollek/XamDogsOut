using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace XamDogsOut.Services
{

    public interface IDataProvider<T> : IDataService<T>
    {

    }
    public interface IDataService<T>
    {
        Task<string> AddItemAsync(T item);
        Task<bool> UpdateItemAsync(T item);
        Task<bool> DeleteItemAsync(string id);
        Task<T> GetItemAsync(string id);

        // returns dog that stores user 'id'
        Task<T> GetByUserId(string userId);
        Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false);
        
    }
}
