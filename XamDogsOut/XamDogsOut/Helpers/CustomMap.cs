using System.Collections.Generic;
using Xamarin.Forms.Maps;

namespace XamDogsOut.Helpers
{
    public class CustomMap : Map
    {
        public List<CustomPin> CustomPins { get; set; }
    }
}
