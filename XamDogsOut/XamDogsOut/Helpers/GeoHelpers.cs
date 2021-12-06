using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace XamDogsOut.Helpers
{
    public class GeoHelpers
    {
        public static async Task<PermissionStatus> CheckAndRequestPermisionsForLocation()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

            if (status == PermissionStatus.Granted)
                return status;

            status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            return status;

        }

        public static async Task<Location> GetDeviceLocation()
        {
            var request = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));
            var cts = new CancellationTokenSource();
            return await Geolocation.GetLocationAsync(request, cts.Token);
        }
    }
}
