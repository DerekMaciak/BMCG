using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace BMCGMobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPage : ContentPage
    {
        public MapPage()
        {
            InitializeComponent();

            customMap.RouteCoordinates.Add(new Position(40.82747, -74.17703));
            customMap.RouteCoordinates.Add(new Position(40.82733, -74.17694));
            customMap.RouteCoordinates.Add(new Position(40.82722, -74.17686));
            customMap.RouteCoordinates.Add(new Position(40.82706, -74.17677));
            customMap.RouteCoordinates.Add(new Position(40.82692, -74.17668));
            customMap.RouteCoordinates.Add(new Position(40.82681, -74.17662));
            customMap.RouteCoordinates.Add(new Position(40.82669, -74.17657));
            customMap.RouteCoordinates.Add(new Position(40.82658, -74.17653));
            customMap.RouteCoordinates.Add(new Position(40.82646, -74.17649));
            customMap.RouteCoordinates.Add(new Position(40.82632, -74.17645));
            customMap.RouteCoordinates.Add(new Position(40.82616, -74.17642));
            customMap.RouteCoordinates.Add(new Position(40.826, -74.17639));
            customMap.RouteCoordinates.Add(new Position(40.82582, -74.17636));
            customMap.RouteCoordinates.Add(new Position(40.82568, -74.17636));
            customMap.RouteCoordinates.Add(new Position(40.82551, -74.17633));
            customMap.RouteCoordinates.Add(new Position(40.82539, -74.17633));
            customMap.RouteCoordinates.Add(new Position(40.82527, -74.17633));
            customMap.RouteCoordinates.Add(new Position(40.82509, -74.17634));
            customMap.RouteCoordinates.Add(new Position(40.82497, -74.17633));
            customMap.RouteCoordinates.Add(new Position(40.82481, -74.17635));
            customMap.RouteCoordinates.Add(new Position(40.82467, -74.17637));
            customMap.RouteCoordinates.Add(new Position(40.82453, -74.17639));
            customMap.RouteCoordinates.Add(new Position(40.82436, -74.17643));
            customMap.RouteCoordinates.Add(new Position(40.82423, -74.17646));
            customMap.RouteCoordinates.Add(new Position(40.82409, -74.17649));
            customMap.RouteCoordinates.Add(new Position(40.82393, -74.17653));
            customMap.RouteCoordinates.Add(new Position(40.82382, -74.17658));
            customMap.RouteCoordinates.Add(new Position(40.82374, -74.1766));
            customMap.RouteCoordinates.Add(new Position(40.82351, -74.17668));
            customMap.RouteCoordinates.Add(new Position(40.82345, -74.17671));
            customMap.RouteCoordinates.Add(new Position(40.8233, -74.17677));
            customMap.RouteCoordinates.Add(new Position(40.82318, -74.17681));
            customMap.RouteCoordinates.Add(new Position(40.82306, -74.17685));
            customMap.RouteCoordinates.Add(new Position(40.82292, -74.17694));
            customMap.RouteCoordinates.Add(new Position(40.8228, -74.17699));
            customMap.RouteCoordinates.Add(new Position(40.82271, -74.17702));
            customMap.RouteCoordinates.Add(new Position(40.82256, -74.17709));
            customMap.RouteCoordinates.Add(new Position(40.82237, -74.17721));
            customMap.RouteCoordinates.Add(new Position(40.82224, -74.17729));
            customMap.RouteCoordinates.Add(new Position(40.82208, -74.1774));
            customMap.RouteCoordinates.Add(new Position(40.82194, -74.1775));
            customMap.RouteCoordinates.Add(new Position(40.82181, -74.1776));
            customMap.RouteCoordinates.Add(new Position(40.82169, -74.1777));
            customMap.RouteCoordinates.Add(new Position(40.82155, -74.17777));
            customMap.RouteCoordinates.Add(new Position(40.82141, -74.1779));
            customMap.RouteCoordinates.Add(new Position(40.82128, -74.17798));
            customMap.RouteCoordinates.Add(new Position(40.82111, -74.1781));
            customMap.RouteCoordinates.Add(new Position(40.82096, -74.17823));
            customMap.RouteCoordinates.Add(new Position(40.82082, -74.17833));
            customMap.RouteCoordinates.Add(new Position(40.82068, -74.1784));
            customMap.RouteCoordinates.Add(new Position(40.82052, -74.17845));
            customMap.RouteCoordinates.Add(new Position(40.82043, -74.17854));
            customMap.RouteCoordinates.Add(new Position(40.82026, -74.17862));
            customMap.RouteCoordinates.Add(new Position(40.82015, -74.17871));
            customMap.RouteCoordinates.Add(new Position(40.82001, -74.1788));
            customMap.RouteCoordinates.Add(new Position(40.81987, -74.17888));
            customMap.RouteCoordinates.Add(new Position(40.81975, -74.17897));
            customMap.RouteCoordinates.Add(new Position(40.81961, -74.17905));
            customMap.RouteCoordinates.Add(new Position(40.81947, -74.17914));
            customMap.RouteCoordinates.Add(new Position(40.81934, -74.1792));
            customMap.RouteCoordinates.Add(new Position(40.81918, -74.17926));
            customMap.RouteCoordinates.Add(new Position(40.81901, -74.17931));
            customMap.RouteCoordinates.Add(new Position(40.81887, -74.17937));
            customMap.RouteCoordinates.Add(new Position(40.81872, -74.17943));
            customMap.RouteCoordinates.Add(new Position(40.81853, -74.17951));

            customMap.RouteCoordinates.Add(new Position(40.81836, -74.17956));

            customMap.RouteCoordinates.Add(new Position(40.81814, -74.17965));
            customMap.RouteCoordinates.Add(new Position(40.81796, -74.17973));
            customMap.RouteCoordinates.Add(new Position(40.81778, -74.17977));
            customMap.RouteCoordinates.Add(new Position(40.81762, -74.17979));
            customMap.RouteCoordinates.Add(new Position(40.81747, -74.17983));
            customMap.RouteCoordinates.Add(new Position(40.81727, -74.17986));
            customMap.RouteCoordinates.Add(new Position(40.8171, -74.17989));
            customMap.RouteCoordinates.Add(new Position(40.81695, -74.17992));
            customMap.RouteCoordinates.Add(new Position(40.81685, -74.17995));
            customMap.RouteCoordinates.Add(new Position(40.81669, -74.17998));
            customMap.RouteCoordinates.Add(new Position(40.81658, -74.18001));
            customMap.RouteCoordinates.Add(new Position(40.81643, -74.18002));
            customMap.RouteCoordinates.Add(new Position(40.81631, -74.18003));
            customMap.RouteCoordinates.Add(new Position(40.81597, -74.18004));
            customMap.RouteCoordinates.Add(new Position(40.81584, -74.18004));
            customMap.RouteCoordinates.Add(new Position(40.81568, -74.18003));
            customMap.RouteCoordinates.Add(new Position(40.81554, -74.18001));
            customMap.RouteCoordinates.Add(new Position(40.8154, -74.18001));
            customMap.RouteCoordinates.Add(new Position(40.8153, -74.17998));
            customMap.RouteCoordinates.Add(new Position(40.81514, -74.17997));
            customMap.RouteCoordinates.Add(new Position(40.81495, -74.17995));
            customMap.RouteCoordinates.Add(new Position(40.8148, -74.17994));
            customMap.RouteCoordinates.Add(new Position(40.81461, -74.17992));
            customMap.RouteCoordinates.Add(new Position(40.81442, -74.17991));
            customMap.RouteCoordinates.Add(new Position(40.81427, -74.1799));
            customMap.RouteCoordinates.Add(new Position(40.81413, -74.17991));
            customMap.RouteCoordinates.Add(new Position(40.81399, -74.17991));
            customMap.RouteCoordinates.Add(new Position(40.81387, -74.17991));
            customMap.RouteCoordinates.Add(new Position(40.81377, -74.17991));
            customMap.RouteCoordinates.Add(new Position(40.81368, -74.17992));
            customMap.RouteCoordinates.Add(new Position(40.81343, -74.17995));
            customMap.RouteCoordinates.Add(new Position(40.81327, -74.17999));
            customMap.RouteCoordinates.Add(new Position(40.81311, -74.18002));
            customMap.RouteCoordinates.Add(new Position(40.81296, -74.18006));
            customMap.RouteCoordinates.Add(new Position(40.81285, -74.18008));
            customMap.RouteCoordinates.Add(new Position(40.81272, -74.1801));
            customMap.RouteCoordinates.Add(new Position(40.8126, -74.18013));
            customMap.RouteCoordinates.Add(new Position(40.81246, -74.18016));
            customMap.RouteCoordinates.Add(new Position(40.81231, -74.1802));
            customMap.RouteCoordinates.Add(new Position(40.81215, -74.18021));
            customMap.RouteCoordinates.Add(new Position(40.81201, -74.18027));
            customMap.RouteCoordinates.Add(new Position(40.81187, -74.18032));
            customMap.RouteCoordinates.Add(new Position(40.81169, -74.18038));
            customMap.RouteCoordinates.Add(new Position(40.81151, -74.18045));
            customMap.RouteCoordinates.Add(new Position(40.81135, -74.18051));
            customMap.RouteCoordinates.Add(new Position(40.81121, -74.18059));
            customMap.RouteCoordinates.Add(new Position(40.81107, -74.18068));
            customMap.RouteCoordinates.Add(new Position(40.81092, -74.18072));
            customMap.RouteCoordinates.Add(new Position(40.81079, -74.18079));
            customMap.RouteCoordinates.Add(new Position(40.81067, -74.18086));
            customMap.RouteCoordinates.Add(new Position(40.81056, -74.18092));
            customMap.RouteCoordinates.Add(new Position(40.81044, -74.181));
            customMap.RouteCoordinates.Add(new Position(40.81037, -74.18104));
            customMap.RouteCoordinates.Add(new Position(40.81012, -74.18117));
            customMap.RouteCoordinates.Add(new Position(40.80997, -74.18124));
            customMap.RouteCoordinates.Add(new Position(40.80979, -74.18136));
            customMap.RouteCoordinates.Add(new Position(40.80959, -74.18146));
            customMap.RouteCoordinates.Add(new Position(40.80939, -74.1816));
            customMap.RouteCoordinates.Add(new Position(40.80925, -74.18174));
            customMap.RouteCoordinates.Add(new Position(40.80909, -74.18183));
            customMap.RouteCoordinates.Add(new Position(40.80895, -74.18195));
            customMap.RouteCoordinates.Add(new Position(40.80879, -74.18206));
            customMap.RouteCoordinates.Add(new Position(40.80864, -74.18215));
            customMap.RouteCoordinates.Add(new Position(40.80852, -74.18222));
            customMap.RouteCoordinates.Add(new Position(40.80843, -74.18229));
            customMap.RouteCoordinates.Add(new Position(40.80829, -74.18237));

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
                Url = "http://xamarin.com/about/"
            };

            customMap.CustomPins.Add(pin);
            customMap.Pins.Add(pin.Pin);

            var cnt = 0;
            for (int i = 5; i < customMap.RouteCoordinates.Count; i += 15)
            {
                cnt += 1;
                var pin2 = new CustomPin
                {
                    Pin = new Pin
                    {
                        Type = PinType.Place,
                        Position = new Position(customMap.RouteCoordinates[i].Latitude, customMap.RouteCoordinates[i].Longitude),
                        Label = string.Format("Wayfinding Sign #{0}", cnt),
                        Address = string.Format("Wayfinding Sign #{0}", cnt)
                    },
                    Id = string.Format("Wayfinding Sign #{0}", cnt),
                    Url = "http://xamarin.com/about/"
                };

                customMap.CustomPins.Add(pin2);
                customMap.Pins.Add(pin2.Pin);
            }

            customMap.MoveToRegion(MapSpan.FromCenterAndRadius(GetCentralGeoCoordinate(new List<Position>() { new Position(customMap.RouteCoordinates[0].Latitude, customMap.RouteCoordinates[0].Longitude), new Position(customMap.RouteCoordinates[customMap.RouteCoordinates.Count - 1].Latitude, customMap.RouteCoordinates[customMap.RouteCoordinates.Count - 1].Longitude) }), Distance.FromMiles(.5)));
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await _RetrieveCurrentLocation();
        }

        private async Task _RetrieveCurrentLocation()
        {
            var geolocator = CrossGeolocator.Current;
            geolocator.DesiredAccuracy = 1;

            if (!geolocator.IsListening)
            {
                geolocator.PositionChanged += Geolocator_PositionChanged;
                await geolocator.StartListeningAsync(new TimeSpan(1000), 1, true);
            }

            var position = await geolocator.GetPositionAsync();

            _CenterMap(position);
        }

        private void _CenterMap(Plugin.Geolocator.Abstractions.Position position)
        {
            if (position != null)
            {
                var centerPosition = GetCentralGeoCoordinate(new List<Position>() { new Position(position.Latitude, position.Longitude), new Position(customMap.RouteCoordinates[0].Latitude, customMap.RouteCoordinates[0].Longitude), new Position(customMap.RouteCoordinates[customMap.RouteCoordinates.Count - 1].Latitude, customMap.RouteCoordinates[customMap.RouteCoordinates.Count - 1].Longitude) });
                var dist = distance(centerPosition.Latitude, centerPosition.Longitude, position.Latitude, position.Longitude, 'M');

                customMap.MoveToRegion(MapSpan.FromCenterAndRadius(centerPosition, Distance.FromMiles(dist)));
            }
        }

        private void Geolocator_PositionChanged(object sender, Plugin.Geolocator.Abstractions.PositionEventArgs e)
        {
           // _CenterMap(e.Position);
        }

        private static void CalculateBoundingCoordinates(MapSpan region)
        {
            var center = region.Center;
            var halfheightDegrees = region.LatitudeDegrees / 2;
            var halfwidthDegrees = region.LongitudeDegrees / 2;

            var left = center.Longitude - halfwidthDegrees;
            var right = center.Longitude + halfwidthDegrees;
            var top = center.Latitude + halfheightDegrees;
            var bottom = center.Latitude - halfheightDegrees;

            // Adjust for Internation Date Line (+/- 180 degrees longitude)
            if (left < -180) left = 180 + (180 + left);
            if (right > 180) right = (right - 180) - 180;
            // I don't wrap around north or south; I don't think the map control allows this anyway
        }


        private double distance(double lat1, double lon1, double lat2, double lon2, char unit)
        {
            double theta = lon1 - lon2;
            double dist = Math.Sin(deg2rad(lat1)) * Math.Sin(deg2rad(lat2)) + Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) * Math.Cos(deg2rad(theta));
            dist = Math.Acos(dist);
            dist = rad2deg(dist);
            dist = dist * 60 * 1.1515;
            if (unit == 'K')
            {
                dist = dist * 1.609344;
            }
            else if (unit == 'N')
            {
                dist = dist * 0.8684;
            }

            return (dist);
        }

        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        //::  This function converts decimal degrees to radians             :::
        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        private double deg2rad(double deg)
        {
            return (deg * Math.PI / 180.0);
        }

        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        //::  This function converts radians to decimal degrees             :::
        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        private double rad2deg(double rad)
        {
            return (rad / Math.PI * 180.0);
        }
        
        public static Position GetCentralGeoCoordinate(IList<Position> geoCoordinates)
        {
            if (geoCoordinates.Count == 1)
            {
                return geoCoordinates[0];
            }

            double x = 0;
            double y = 0;
            double z = 0;

            foreach (var geoCoordinate in geoCoordinates)
            {
                var latitude = geoCoordinate.Latitude * Math.PI / 180;
                var longitude = geoCoordinate.Longitude * Math.PI / 180;

                x += Math.Cos(latitude) * Math.Cos(longitude);
                y += Math.Cos(latitude) * Math.Sin(longitude);
                z += Math.Sin(latitude);
            }

            var total = geoCoordinates.Count;

            x = x / total;
            y = y / total;
            z = z / total;

            var centralLongitude = Math.Atan2(y, x);
            var centralSquareRoot = Math.Sqrt(x * x + y * y);
            var centralLatitude = Math.Atan2(z, centralSquareRoot);

            return new Position(centralLatitude * 180 / Math.PI, centralLongitude * 180 / Math.PI);
        }
    }
}