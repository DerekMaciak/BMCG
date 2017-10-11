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
        Plugin.Geolocator.Abstractions.Position _LastKnownPosition;

        public MapPage()
        {
            InitializeComponent();

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

            _LastKnownPosition = await geolocator.GetPositionAsync();

            _CenterMap(_LastKnownPosition);
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

        private void _CenterMapToPins()
        {
           
                var centerPosition = GetCentralGeoCoordinate(new List<Position>() {new Position(customMap.RouteCoordinates[0].Latitude, customMap.RouteCoordinates[0].Longitude), new Position(customMap.RouteCoordinates[customMap.RouteCoordinates.Count - 1].Latitude, customMap.RouteCoordinates[customMap.RouteCoordinates.Count - 1].Longitude) });
                var dist = distance(customMap.RouteCoordinates[0].Latitude, customMap.RouteCoordinates[0].Longitude, customMap.RouteCoordinates[customMap.RouteCoordinates.Count - 1].Latitude, customMap.RouteCoordinates[customMap.RouteCoordinates.Count - 1].Longitude, 'M');
                dist = dist == 0 ? 0 : dist / 2.5;
                customMap.MoveToRegion(MapSpan.FromCenterAndRadius(centerPosition, Distance.FromMiles(dist)));
           
        }

        private void Geolocator_PositionChanged(object sender, Plugin.Geolocator.Abstractions.PositionEventArgs e)
        {
           // _CenterMap(e.Position);
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

        private void MapViewButton_Clicked(object sender, EventArgs e)
        {
            var b = sender as Button;
            switch (b.Text)
            {
                case "Street":
                    customMap.MapType = MapType.Street;
                    break;
                case "Hybrid":
                    customMap.MapType = MapType.Hybrid;
                    break;
                case "Satellite":
                    customMap.MapType = MapType.Satellite;
                    break;
            }
        }

        private void ZoomPinsButton_Clicked(object sender, EventArgs e)
        {

            _CenterMapToPins();
        }

        private void ZoomAllButton_Clicked(object sender, EventArgs e)
        {
            _CenterMap(_LastKnownPosition);
        }
    }
}