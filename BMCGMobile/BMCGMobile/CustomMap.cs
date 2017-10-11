using System.Collections.Generic;
using System.Threading.Tasks;
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

            _LoadRouteCoordinates();

            _LoadPins();

        }

        private void _LoadRouteCoordinates()
        {
            RouteCoordinates.Add(new Position(40.82747, -74.17703));
            RouteCoordinates.Add(new Position(40.82733, -74.17694));
            RouteCoordinates.Add(new Position(40.82722, -74.17686));
            RouteCoordinates.Add(new Position(40.82706, -74.17677));
            RouteCoordinates.Add(new Position(40.82692, -74.17668));
            RouteCoordinates.Add(new Position(40.82681, -74.17662));
            RouteCoordinates.Add(new Position(40.82669, -74.17657));
            RouteCoordinates.Add(new Position(40.82658, -74.17653));
            RouteCoordinates.Add(new Position(40.82646, -74.17649));
            RouteCoordinates.Add(new Position(40.82632, -74.17645));
            RouteCoordinates.Add(new Position(40.82616, -74.17642));
            RouteCoordinates.Add(new Position(40.826, -74.17639));
            RouteCoordinates.Add(new Position(40.82582, -74.17636));
            RouteCoordinates.Add(new Position(40.82568, -74.17636));
            RouteCoordinates.Add(new Position(40.82551, -74.17633));
            RouteCoordinates.Add(new Position(40.82539, -74.17633));
            RouteCoordinates.Add(new Position(40.82527, -74.17633));
            RouteCoordinates.Add(new Position(40.82509, -74.17634));
            RouteCoordinates.Add(new Position(40.82497, -74.17633));
            RouteCoordinates.Add(new Position(40.82481, -74.17635));
            RouteCoordinates.Add(new Position(40.82467, -74.17637));
            RouteCoordinates.Add(new Position(40.82453, -74.17639));
            RouteCoordinates.Add(new Position(40.82436, -74.17643));
            RouteCoordinates.Add(new Position(40.82423, -74.17646));
            RouteCoordinates.Add(new Position(40.82409, -74.17649));
            RouteCoordinates.Add(new Position(40.82393, -74.17653));
            RouteCoordinates.Add(new Position(40.82382, -74.17658));
            RouteCoordinates.Add(new Position(40.82374, -74.1766));
            RouteCoordinates.Add(new Position(40.82351, -74.17668));
            RouteCoordinates.Add(new Position(40.82345, -74.17671));
            RouteCoordinates.Add(new Position(40.8233, -74.17677));
            RouteCoordinates.Add(new Position(40.82318, -74.17681));
            RouteCoordinates.Add(new Position(40.82306, -74.17685));
            RouteCoordinates.Add(new Position(40.82292, -74.17694));
            RouteCoordinates.Add(new Position(40.8228, -74.17699));
            RouteCoordinates.Add(new Position(40.82271, -74.17702));
            RouteCoordinates.Add(new Position(40.82256, -74.17709));
            RouteCoordinates.Add(new Position(40.82237, -74.17721));
            RouteCoordinates.Add(new Position(40.82224, -74.17729));
            RouteCoordinates.Add(new Position(40.82208, -74.1774));
            RouteCoordinates.Add(new Position(40.82194, -74.1775));
            RouteCoordinates.Add(new Position(40.82181, -74.1776));
            RouteCoordinates.Add(new Position(40.82169, -74.1777));
            RouteCoordinates.Add(new Position(40.82155, -74.17777));
            RouteCoordinates.Add(new Position(40.82141, -74.1779));
            RouteCoordinates.Add(new Position(40.82128, -74.17798));
            RouteCoordinates.Add(new Position(40.82111, -74.1781));
            RouteCoordinates.Add(new Position(40.82096, -74.17823));
            RouteCoordinates.Add(new Position(40.82082, -74.17833));
            RouteCoordinates.Add(new Position(40.82068, -74.1784));
            RouteCoordinates.Add(new Position(40.82052, -74.17845));
            RouteCoordinates.Add(new Position(40.82043, -74.17854));
            RouteCoordinates.Add(new Position(40.82026, -74.17862));
            RouteCoordinates.Add(new Position(40.82015, -74.17871));
            RouteCoordinates.Add(new Position(40.82001, -74.1788));
            RouteCoordinates.Add(new Position(40.81987, -74.17888));
            RouteCoordinates.Add(new Position(40.81975, -74.17897));
            RouteCoordinates.Add(new Position(40.81961, -74.17905));
            RouteCoordinates.Add(new Position(40.81947, -74.17914));
            RouteCoordinates.Add(new Position(40.81934, -74.1792));
            RouteCoordinates.Add(new Position(40.81918, -74.17926));
            RouteCoordinates.Add(new Position(40.81901, -74.17931));
            RouteCoordinates.Add(new Position(40.81887, -74.17937));
            RouteCoordinates.Add(new Position(40.81872, -74.17943));
            RouteCoordinates.Add(new Position(40.81853, -74.17951));
            RouteCoordinates.Add(new Position(40.81836, -74.17956));
            RouteCoordinates.Add(new Position(40.81814, -74.17965));
            RouteCoordinates.Add(new Position(40.81796, -74.17973));
            RouteCoordinates.Add(new Position(40.81778, -74.17977));
            RouteCoordinates.Add(new Position(40.81762, -74.17979));
            RouteCoordinates.Add(new Position(40.81747, -74.17983));
            RouteCoordinates.Add(new Position(40.81727, -74.17986));
            RouteCoordinates.Add(new Position(40.8171, -74.17989));
            RouteCoordinates.Add(new Position(40.81695, -74.17992));
            RouteCoordinates.Add(new Position(40.81685, -74.17995));
            RouteCoordinates.Add(new Position(40.81669, -74.17998));
            RouteCoordinates.Add(new Position(40.81658, -74.18001));
            RouteCoordinates.Add(new Position(40.81643, -74.18002));
            RouteCoordinates.Add(new Position(40.81631, -74.18003));
            RouteCoordinates.Add(new Position(40.81597, -74.18004));
            RouteCoordinates.Add(new Position(40.81584, -74.18004));
            RouteCoordinates.Add(new Position(40.81568, -74.18003));
            RouteCoordinates.Add(new Position(40.81554, -74.18001));
            RouteCoordinates.Add(new Position(40.8154, -74.18001));
            RouteCoordinates.Add(new Position(40.8153, -74.17998));
            RouteCoordinates.Add(new Position(40.81514, -74.17997));
            RouteCoordinates.Add(new Position(40.81495, -74.17995));
            RouteCoordinates.Add(new Position(40.8148, -74.17994));
            RouteCoordinates.Add(new Position(40.81461, -74.17992));
            RouteCoordinates.Add(new Position(40.81442, -74.17991));
            RouteCoordinates.Add(new Position(40.81427, -74.1799));
            RouteCoordinates.Add(new Position(40.81413, -74.17991));
            RouteCoordinates.Add(new Position(40.81399, -74.17991));
            RouteCoordinates.Add(new Position(40.81387, -74.17991));
            RouteCoordinates.Add(new Position(40.81377, -74.17991));
            RouteCoordinates.Add(new Position(40.81368, -74.17992));
            RouteCoordinates.Add(new Position(40.81343, -74.17995));
            RouteCoordinates.Add(new Position(40.81327, -74.17999));
            RouteCoordinates.Add(new Position(40.81311, -74.18002));
            RouteCoordinates.Add(new Position(40.81296, -74.18006));
            RouteCoordinates.Add(new Position(40.81285, -74.18008));
            RouteCoordinates.Add(new Position(40.81272, -74.1801));
            RouteCoordinates.Add(new Position(40.8126, -74.18013));
            RouteCoordinates.Add(new Position(40.81246, -74.18016));
            RouteCoordinates.Add(new Position(40.81231, -74.1802));
            RouteCoordinates.Add(new Position(40.81215, -74.18021));
            RouteCoordinates.Add(new Position(40.81201, -74.18027));
            RouteCoordinates.Add(new Position(40.81187, -74.18032));
            RouteCoordinates.Add(new Position(40.81169, -74.18038));
            RouteCoordinates.Add(new Position(40.81151, -74.18045));
            RouteCoordinates.Add(new Position(40.81135, -74.18051));
            RouteCoordinates.Add(new Position(40.81121, -74.18059));
            RouteCoordinates.Add(new Position(40.81107, -74.18068));
            RouteCoordinates.Add(new Position(40.81092, -74.18072));
            RouteCoordinates.Add(new Position(40.81079, -74.18079));
            RouteCoordinates.Add(new Position(40.81067, -74.18086));
            RouteCoordinates.Add(new Position(40.81056, -74.18092));
            RouteCoordinates.Add(new Position(40.81044, -74.181));
            RouteCoordinates.Add(new Position(40.81037, -74.18104));
            RouteCoordinates.Add(new Position(40.81012, -74.18117));
            RouteCoordinates.Add(new Position(40.80997, -74.18124));
            RouteCoordinates.Add(new Position(40.80979, -74.18136));
            RouteCoordinates.Add(new Position(40.80959, -74.18146));
            RouteCoordinates.Add(new Position(40.80939, -74.1816));
            RouteCoordinates.Add(new Position(40.80925, -74.18174));
            RouteCoordinates.Add(new Position(40.80909, -74.18183));
            RouteCoordinates.Add(new Position(40.80895, -74.18195));
            RouteCoordinates.Add(new Position(40.80879, -74.18206));
            RouteCoordinates.Add(new Position(40.80864, -74.18215));
            RouteCoordinates.Add(new Position(40.80852, -74.18222));
            RouteCoordinates.Add(new Position(40.80843, -74.18229));
            RouteCoordinates.Add(new Position(40.80829, -74.18237));

        }

        private void _LoadPins()
        {

            var pin = new CustomPin
            {
                Pin = new Pin
                {
                    Type = PinType.Place,
                    Position = new Position(40.827485, -74.177098),
                    Label = "Morris Canal Park",
                    Address = "1115 Broad St, Clifton, NJ 07013"
                },
                Id = "Morris Canal Park",
                Url = "https://www.facebook.com/bloomfieldmorriscanal/"
            };

            CustomPins.Add(pin);
            Pins.Add(pin.Pin);

            // Plot points
            var cnt = 0;
            for (int i = 10; i < RouteCoordinates.Count; i += 10)
            {
                cnt += 1;


                if (i == 30 || i == 60 || i == 90 || i == 120 || i == 150)
                {
                    // Plot Kiosk
                    pin = new CustomPin
                    {
                        Pin = new Pin
                        {
                            Type = PinType.Place,
                            Position = new Position(RouteCoordinates[i].Latitude, RouteCoordinates[i].Longitude),
                            Label = string.Format("Informational Kiosk #{0}", cnt),
                            Address = string.Format("Informational Kiosk #{0} Address", cnt)
                        },
                        Id = string.Format("Informational Kiosk #{0}", cnt),
                        Url = string.Format("http://services.surroundtech.com/BMCGWebsite/Home/GenericInformationalKiosk/{0}", cnt),
                        PinType = CustomPin.PinTypes.Kiosk
                    };

                    CustomPins.Add(pin);
                    Pins.Add(pin.Pin);
                }
                else
                {

                   pin = new CustomPin
                    {
                        Pin = new Pin
                        {
                            Type = PinType.Place,
                            Position = new Position(RouteCoordinates[i].Latitude, RouteCoordinates[i].Longitude),
                            Label = string.Format("Wayfinding Sign #{0}", cnt),
                            Address = string.Format("Wayfinding Sign #{0} Address", cnt)
                        },
                        Id = string.Format("Wayfinding Sign #{0}", cnt),
                        Url = string.Format("http://services.surroundtech.com/BMCGWebsite/Home/GenericWayFindingSign/{0}", cnt),
                        PinType = CustomPin.PinTypes.Sign
                    };

                    CustomPins.Add(pin);
                    Pins.Add(pin.Pin);
                }
            }

            _RetrieveAddressForPosition();
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

