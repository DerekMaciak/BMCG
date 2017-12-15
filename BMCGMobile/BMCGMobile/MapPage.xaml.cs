using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;

namespace BMCGMobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPage : ContentPage
    {
        private enum MapZooms
        {
            All,
            Pins,
            Street,
            User,
            None
        }

        private MapZooms _CurrentMapZoom = MapZooms.All;

        private double _DefaultStreetZoom = 18d;
        private double _DefaultStreetTilt = 70d;

        private Plugin.Geolocator.Abstractions.Position _LastKnownPosition;

        public MapPage()
        {
            InitializeComponent();

            _SetMapViewToggleButton(MapType.None);
            _SetZoomViewToggleButton(MapZooms.None);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                if (CustomMap.RouteCoordinates == null)
                {
                    customMap.LoadMapCoordinates();

                    customMap.UiSettings.MyLocationButtonEnabled = false;
                    customMap.UiSettings.CompassEnabled = true;
                    customMap.UiSettings.ScrollGesturesEnabled = true;
                    customMap.UiSettings.ZoomControlsEnabled = true;
                    customMap.UiSettings.RotateGesturesEnabled = true;
                    customMap.UiSettings.TiltGesturesEnabled = true;

                    await _RetrieveCurrentLocation();

                    _SetZoomViewToggleButton(MapZooms.All);

                    customMap.MapType = MapType.Street;
                    _SetMapViewToggleButton(customMap.MapType);

                    customMap.InfoWindowClicked += CustomMap_InfoWindowClicked;

                    //customMap.MyLocationButtonClicked += (sender, args) =>
                    //{
                    //    args.Handled = false;
                    //    _SetZoomViewToggleButton(MapZooms.None);
                    //};

                    //customMap.CameraIdled += (sender, e) =>
                    //{
                    //    _CurrentZoom = e.Position.Zoom;
                    //    _CurrentTilt = e.Position.Tilt;
                    //};

                    // Map MyLocationButton clicked
                    //customMap.MyLocationButtonClicked += (sender, args) =>
                    //{
                    //    args.Handled = true;

                    //    customMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(_LastKnownPosition.Latitude, _LastKnownPosition.Longitude), Distance.FromMiles(15)));
                    //};
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void CustomMap_InfoWindowClicked(object sender, InfoWindowClickedEventArgs e)
        {
            var selectedCustomPin = CustomMap.CustomPins.Where(s => s.Id == e.Pin.Tag.ToString()).FirstOrDefault();
            Device.OpenUri(new Uri(selectedCustomPin.Url));
        }

        private async Task _RetrieveCurrentLocation()
        {
            var geolocator = CrossGeolocator.Current;
            geolocator.DesiredAccuracy = 1;
            //geolocator.SupportsHeading = true;

            if (!geolocator.IsListening)
            {
                // MoveToCamera with Position and Zoom
                geolocator.PositionChanged += async (sender, e) =>
                {
                    _LastKnownPosition = e.Position;

                    if (_CurrentMapZoom == MapZooms.Street)
                    {
                        await _StreetViewAsync(e.Position, _DefaultStreetZoom, _DefaultStreetTilt);
                        // _StreetView(e.Position, customMap.CameraPosition.Zoom, customMap.CameraPosition.Tilt);
                    }
                };

                await geolocator.StartListeningAsync(new TimeSpan(1000), 1, true);
            }

            _LastKnownPosition = await geolocator.GetPositionAsync();

            _CenterMap(_LastKnownPosition);
        }

        private async Task _StreetViewAsync(Plugin.Geolocator.Abstractions.Position position, double zoom, double tilt)
        {
            await customMap.MoveCamera(CameraUpdateFactory.NewCameraPosition(
                                 new CameraPosition(
                                     new Position(position.Latitude, position.Longitude),
                                     zoom,
                                     position.Heading, // bearing(rotation)
                                     tilt
                                     )));
        }

        private void _CenterMap(Plugin.Geolocator.Abstractions.Position position)
        {
            if (position != null)
            {
                if (CustomMap.RouteCoordinates != null && CustomMap.RouteCoordinates.Count > 0)
                {
                    var firstLatCoordinate = CustomMap.RouteCoordinates.OrderBy(o => o.Latitude).FirstOrDefault();
                    var lastLatCoordinate = CustomMap.RouteCoordinates.OrderBy(o => o.Latitude).LastOrDefault();
                    var firstLongCoordinate = CustomMap.RouteCoordinates.OrderBy(o => o.Longitude).FirstOrDefault();
                    var lastLongCoordinate = CustomMap.RouteCoordinates.OrderBy(o => o.Longitude).LastOrDefault();

                    var centerPosition = GetCentralGeoCoordinate(new List<Position>() {
                    new Position(position.Latitude, position.Longitude),
                    new Position(firstLatCoordinate.Latitude, firstLatCoordinate.Longitude), new Position(lastLatCoordinate.Latitude, lastLatCoordinate.Longitude),
                    new Position(firstLongCoordinate.Latitude, firstLongCoordinate.Longitude), new Position(lastLongCoordinate.Latitude, lastLongCoordinate.Longitude) });

                    var distLat = distance(firstLatCoordinate.Latitude, firstLatCoordinate.Longitude, lastLatCoordinate.Latitude, lastLatCoordinate.Longitude, 'M');
                    var distLong = distance(firstLongCoordinate.Latitude, firstLongCoordinate.Longitude, lastLongCoordinate.Latitude, lastLongCoordinate.Longitude, 'M');
                    var dist = distLat >= distLong ? distLat : distLong;

                    var distPost = distance(centerPosition.Latitude, centerPosition.Longitude, position.Latitude, position.Longitude, 'M');
                    dist = distPost >= dist ? distPost : dist;

                    dist = dist == 0 ? 0 : dist / 2.5;

                    customMap.MoveToRegion(MapSpan.FromCenterAndRadius(centerPosition, Distance.FromMiles(dist)));
                }
                else
                {
                    customMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude), Distance.FromMiles(15)));
                }
            }
        }

        private void _CenterMapToPins()
        {
            if (CustomMap.RouteCoordinates != null && CustomMap.RouteCoordinates.Count > 0)
            {
                var firstLatCoordinate = CustomMap.RouteCoordinates.OrderBy(o => o.Latitude).FirstOrDefault();
                var lastLatCoordinate = CustomMap.RouteCoordinates.OrderBy(o => o.Latitude).LastOrDefault();
                var firstLongCoordinate = CustomMap.RouteCoordinates.OrderBy(o => o.Longitude).FirstOrDefault();
                var lastLongCoordinate = CustomMap.RouteCoordinates.OrderBy(o => o.Longitude).LastOrDefault();

                var centerPosition = GetCentralGeoCoordinate(new List<Position>() {
                    new Position(firstLatCoordinate.Latitude, firstLatCoordinate.Longitude), new Position(lastLatCoordinate.Latitude, lastLatCoordinate.Longitude),
                    new Position(firstLongCoordinate.Latitude, firstLongCoordinate.Longitude), new Position(lastLongCoordinate.Latitude, lastLongCoordinate.Longitude) });

                var distLat = distance(firstLatCoordinate.Latitude, firstLatCoordinate.Longitude, lastLatCoordinate.Latitude, lastLatCoordinate.Longitude, 'M');
                var distLong = distance(firstLongCoordinate.Latitude, firstLongCoordinate.Longitude, lastLongCoordinate.Latitude, lastLongCoordinate.Longitude, 'M');
                var dist = distLat >= distLong ? distLat : distLong;

                dist = dist == 0 ? 0 : dist / 2.5;

                customMap.MoveToRegion(MapSpan.FromCenterAndRadius(centerPosition, Distance.FromMiles(dist)));
            }
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

            _SetMapViewToggleButton(customMap.MapType);
        }

        private void StreetViewButton_Clicked(object sender, EventArgs e)
        {
            _SetZoomViewToggleButton(MapZooms.Street);

            customMap.AnimateCamera(CameraUpdateFactory.NewCameraPosition(
                                new CameraPosition(
                                    new Position(_LastKnownPosition.Latitude, _LastKnownPosition.Longitude),
                                    _DefaultStreetZoom,
                                    _LastKnownPosition.Heading, // bearing(rotation)
                                    _DefaultStreetTilt
                                    )));
        }

        private void ZoomPinsButton_Clicked(object sender, EventArgs e)
        {
            _SetZoomViewToggleButton(MapZooms.Pins);
            _CenterMapToPins();
        }

        private void ZoomAllButton_Clicked(object sender, EventArgs e)
        {
            _SetZoomViewToggleButton(MapZooms.All);
            _CenterMap(_LastKnownPosition);
        }

        private void ZoomUserButton_Clicked(object sender, EventArgs e)
        {
            _SetZoomViewToggleButton(MapZooms.User);
            customMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(_LastKnownPosition.Latitude, _LastKnownPosition.Longitude), Distance.FromMiles(1)));
        }

        private void _SetZoomViewToggleButton(MapZooms mapZoom)
        {
            var onButtonBorderColor = Color.Black;
            var offButtonBorderColor = Color.DarkGray;

            var onButtonBackgroundColor = Color.FromHex("#99999999");
            var offButtonBackgroundColor = Color.FromHex("#99e6e6e6");

            //Set All Off
            btnZoomAll.BorderColor = offButtonBorderColor;
            btnZoomUser.BorderColor = offButtonBorderColor;
            btnZoomPins.BorderColor = offButtonBorderColor;
            btnZoomStreet.BorderColor = offButtonBorderColor;

            btnZoomAll.BackgroundColor = offButtonBackgroundColor;
            btnZoomUser.BackgroundColor = offButtonBackgroundColor;
            btnZoomPins.BackgroundColor = offButtonBackgroundColor;
            btnZoomStreet.BackgroundColor = offButtonBackgroundColor;

            switch (mapZoom)
            {
                case MapZooms.All:
                    btnZoomAll.BorderColor = onButtonBorderColor;
                    btnZoomAll.BackgroundColor = onButtonBackgroundColor;
                    break;

                case MapZooms.User:
                    btnZoomUser.BorderColor = onButtonBorderColor;
                    btnZoomUser.BackgroundColor = onButtonBackgroundColor;
                    break;

                case MapZooms.Pins:
                    btnZoomPins.BorderColor = onButtonBorderColor;
                    btnZoomPins.BackgroundColor = onButtonBackgroundColor;
                    break;

                case MapZooms.Street:
                    btnZoomStreet.BorderColor = onButtonBorderColor;
                    btnZoomStreet.BackgroundColor = onButtonBackgroundColor;
                    break;

                default:
                    break;
            }

            _CurrentMapZoom = mapZoom;
        }

        private void _SetMapViewToggleButton(MapType mapType)
        {
            var onButtonBorderColor = Color.Black;
            var offButtonBorderColor = Color.DarkGray;

            var onButtonBackgroundColor = Color.FromHex("#99999999");
            var offButtonBackgroundColor = Color.FromHex("#99e6e6e6");

            //Set All Off
            btnMapTypeStreet.BorderColor = offButtonBorderColor;
            btnMapTypeHybrid.BorderColor = offButtonBorderColor;
            btnMapTypeSatellite.BorderColor = offButtonBorderColor;

            btnMapTypeStreet.BackgroundColor = offButtonBackgroundColor;
            btnMapTypeHybrid.BackgroundColor = offButtonBackgroundColor;
            btnMapTypeSatellite.BackgroundColor = offButtonBackgroundColor;

            switch (mapType)
            {
                case MapType.Street:
                    btnMapTypeStreet.BorderColor = onButtonBorderColor;
                    btnMapTypeStreet.BackgroundColor = onButtonBackgroundColor;
                    break;

                case MapType.Hybrid:
                    btnMapTypeHybrid.BorderColor = onButtonBorderColor;
                    btnMapTypeHybrid.BackgroundColor = onButtonBackgroundColor;
                    break;

                case MapType.Satellite:
                    btnMapTypeSatellite.BorderColor = onButtonBorderColor;
                    btnMapTypeSatellite.BackgroundColor = onButtonBackgroundColor;
                    break;

                default:
                    break;
            }
        }
    }
};