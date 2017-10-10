using System.Collections.Generic;
using Xamarin.Forms.Maps;

namespace BMCGMobile
{
    public class CustomMap : Map
    {
        public List<Position> RouteCoordinates { get; set; }
        public List<CustomPin> CustomPins { get; set; }

        public CustomMap()
        {
            RouteCoordinates = new List<Position>();
            CustomPins = new List<CustomPin>();
        }
    }
}

