using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;

namespace BMCGMobile
{
    public class CustomMap : Map
    {
        private const string _GPXURL = @"http://services.surroundtech.com/BMCGWebsite/downloads/BMCGCoordinates.txt";
        private const string _CMSWEBSITEURL = @"http://services.surroundtech.com/BMCGWebsite/Home";

        public List<Position> RouteCoordinates { get; set; }
        public List<CustomPin> CustomPins { get; set; }

        public CustomMap()
        {
            RouteCoordinates = new List<Position>();
            CustomPins = new List<CustomPin>();

            var gpxLoader = new GPXLoader();

            // load track coordinates
            var trackCoordinates = gpxLoader.LoadGPXTracks(_GPXURL);
            if (trackCoordinates != null)
            {
                foreach (var item in trackCoordinates)
                {
                    RouteCoordinates.Add(new Position(item.Latitude, item.Longitude));
                }
            }

            // Load Wayfinding Pins
            var wayfindingCoordinates = gpxLoader.LoadGPXWayPoints(_GPXURL);
            if (wayfindingCoordinates != null)
            {
                foreach (var item in wayfindingCoordinates.OrderBy(p => p.Sequence))
                {
                    var address = string.IsNullOrWhiteSpace(item.Description) ? string.Format("Latitude: {0} Longitude: {1}", item.Latitude, item.Longitude) : item.Description;

                    var pin = new CustomPin
                    {
                        Pin = new Pin
                        {
                            Type = PinType.Place,
                            Position = new Position(item.Latitude, item.Longitude),
                            Label = item.Name,
                            Address = address
                        },
                        Id = item.Name,
                        Url = new Uri(new Uri(_CMSWEBSITEURL), item.URL).ToString(),
                        PinType = item.PinType
                    };

                    CustomPins.Add(pin);
                    Pins.Add(pin.Pin);
                }

                _RetrieveAddressForPosition();
            }
            
        }
        
        private async void _RetrieveAddressForPosition()
        {
            foreach (var item in CustomPins)
            {
                item.Pin.Address = await _RetrieveAddressForPositionAsync(item.Pin.Position);
            }
        }

        private async Task<string> _RetrieveAddressForPositionAsync(Position position)
        {
            var geoCoder = new Geocoder();

            var possibleAddresses = await geoCoder.GetAddressesForPositionAsync(position);
            foreach (var address in possibleAddresses)
            {
                return address;
            }

            return null;
        }
    }
}