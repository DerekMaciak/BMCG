using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace BMCGMobile
{
    public class CustomMap : Map
    {
        private static List<Position> _RouteCoordinates;
        private static List<CustomPin> _CustomPins;

        public static List<Position> RouteCoordinates { get { return _RouteCoordinates; } }
        public static List<CustomPin> CustomPins { get { return _CustomPins; } }

        public CustomMap()
        {
          
        }

        public void LoadMapCoordinates()
        {

            try
            {
                if (_RouteCoordinates == null)
                {
                    _RouteCoordinates = new List<Position>();
                    _CustomPins = new List<CustomPin>();

                    var gpxLoader = new GPXLoader();

                    // load track coordinates
                    var trackCoordinates = gpxLoader.LoadGPXTracks(Variables.GPX_URL);
                    if (trackCoordinates != null)
                    {
                        foreach (var item in trackCoordinates)
                        {
                            _RouteCoordinates.Add(new Position(item.Latitude, item.Longitude));
                        }
                    }

                    // Load Wayfinding Pins
                    var wayfindingCoordinates = gpxLoader.LoadGPXWayPoints(Variables.GPX_URL);
                    if (wayfindingCoordinates != null)
                    {
                        foreach (var item in wayfindingCoordinates.OrderBy(p => p.Sequence))
                        {
                            var address = string.IsNullOrWhiteSpace(item.Description) ? string.Format("Latitude: {0} Longitude: {1}", item.Latitude, item.Longitude) : item.Description;

                            var pin = new CustomPin
                            {
                                Id = item.Name,
                                Url = new Uri(new Uri(Variables.CMS_WEBSITE_URL), item.URL).ToString(),
                                PinType = item.PinType,
                                Pin = new Pin
                                {
                                    Tag = item.Name,
                                    Type = PinType.Place,
                                    Position = new Position(item.Latitude, item.Longitude),
                                    Label = item.Name,
                                    Address = address,
                                    Icon = BitmapDescriptorFactory.DefaultMarker(CustomPin.GetPinImageColor(item.PinType)),                                  
                                
                                   //Icon = BitmapDescriptorFactory.FromBundle(CustomPin.GetPinImageName(item.PinType))
                                },
                              
                                
                            };

                            _CustomPins.Add(pin);
                        }

                        _RetrieveAddressForPosition();

                        PlotPolylineTrack();
                    }
                }
            }
            catch (Exception ex)
            {
                return;
            }

        }

        public void PlotPolylineTrack()
        {
            var polyline = new Polyline();

            foreach (var item in RouteCoordinates)
            {
                polyline.Positions.Add(new Position(item.Latitude, item.Longitude));
            }

            polyline.IsClickable = false;
            polyline.StrokeColor = Color.FromHex("#A24437"); //Red
            polyline.StrokeWidth = 5f;
            polyline.Tag = "POLYLINE"; // Can set any object

            Polylines.Add(polyline);
        }

        private async void _RetrieveAddressForPosition()
        {
            foreach (var item in CustomPins)
            {
                item.Pin.Address = await _RetrieveAddressForPositionAsync(item.Pin.Position);

                // Add to Map After address has been obtained
                Pins.Add(item.Pin);
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