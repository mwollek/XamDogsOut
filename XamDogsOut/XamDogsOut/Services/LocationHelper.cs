using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace XamDogsOut.Services
{
    public interface ILocationService
    {
        Task<Xamarin.Essentials.Location> GetLastLocationAsync();
    }
    public class LocationHelper
    {
        private static ILocationService locationService = DependencyService.Get<ILocationService>();
        public static async Task<Xamarin.Essentials.Location> GetLastLocationAsync()
        {
            return await locationService.GetLastLocationAsync();
        }
    }
}
