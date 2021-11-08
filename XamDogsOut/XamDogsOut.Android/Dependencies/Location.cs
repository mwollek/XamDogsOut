using Android.App;
using Android.Content;
using Android.Gms.Location;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamDogsOut.Services;

[assembly: Dependency(typeof(XamDogsOut.Droid.Dependencies.Location))]
namespace XamDogsOut.Droid.Dependencies
{
    class Location : ILocationService
    {
        public async Task<Xamarin.Essentials.Location> GetLastLocationAsync()
        {
            Android.Locations.Location location = await MainActivity.fusedLocationProviderClient.GetLastLocationAsync();

            if (location == null)
            {
                throw new Exception("Empty location");
            }
            else
            {
                return new Xamarin.Essentials.Location(location.Latitude, location.Longitude, location.Altitude);
            }
        }
    }
}