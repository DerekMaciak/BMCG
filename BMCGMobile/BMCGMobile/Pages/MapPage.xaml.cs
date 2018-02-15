﻿using BMCGMobile.Entities;
using Plugin.Compass;
using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;
using static BMCGMobile.Enums;

namespace BMCGMobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPage : ContentPage
    {
        private MapZooms _CurrentMapZoom = MapZooms.All;

        private double _DefaultStreetZoom = 19d;
        private double _DefaultStreetTilt = 70d;
        private Double _LastCompassHeading = 0;

        private Plugin.Geolocator.Abstractions.Position _LastKnownPosition;

        public MapPage()
        {
            InitializeComponent();

            if (Device.RuntimePlatform == Device.Android)
            {
                // Move StatusInfo to Right for Android
                AbsoluteLayout.SetLayoutBounds(statusInfo, new Rectangle(1, 0, 285, 85));
            }

            // Get User Settings
            var settingsEntity = new SettingsEntity();
            StaticData.TrackingData.UserSettings = settingsEntity;

            // Set DistanceFromTrailCenter so that IsOnTrail is false while loading
            StaticData.TrackingData.DistanceFromTrailCenter = double.MaxValue;

            StaticData.TrackingData.PropertyChanged += _TrackingEntity_PropertyChanged;

            this.BindingContext = StaticData.TrackingData;

            _SetMapViewToggleButton(MapType.None);
            _SetZoomViewToggleButton(MapZooms.None);
        }

        private void _TrackingEntity_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsOnTrail")
            {
                if (StaticData.TrackingData.IsOnTrail)
                {
                    var assembly = typeof(App).GetTypeInfo().Assembly;
                    Stream audioStream = assembly.GetManifestResourceStream("BMCGMobile.Audio." + "OnTrail.mp3");
                    var player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
                    player.Load(audioStream);
                    player.Play();
                }
                else
                {
                    var assembly = typeof(App).GetTypeInfo().Assembly;
                    Stream audioStream = assembly.GetManifestResourceStream("BMCGMobile.Audio." + "OffTrail.mp3");
                    var player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
                    player.Load(audioStream);
                    player.Play();
                }
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                if (StaticData.RouteCoordinates == null)
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
            var selectedCustomPin = StaticData.CustomPins.Where(s => s.Id == e.Pin.Tag.ToString()).FirstOrDefault();
            Device.OpenUri(new Uri(selectedCustomPin.Url));
        }

        private async Task _RetrieveCurrentLocation()
        {
            var geolocator = CrossGeolocator.Current;
            geolocator.DesiredAccuracy = 1;

            if (!geolocator.IsListening)
            {
                if (Device.RuntimePlatform == Device.Android)
                {
                    CrossCompass.Current.CompassChanged += async (s, e) =>
                    {
                        if ((Device.RuntimePlatform == Device.Android || !geolocator.SupportsHeading))
                        {
                            if (_LastKnownPosition != null)
                            {
                                if (Math.Abs(_LastCompassHeading - Math.Truncate(e.Heading)) > 2)
                                {
                                    _LastKnownPosition.Heading = e.Heading;

                                    if (_LastKnownPosition.Heading != 0)
                                    {
                                        if (_CurrentMapZoom == MapZooms.Street)
                                        {
                                            // If Street View, Set Camera Position
                                            var zoom = customMap.CameraPosition.Zoom == 0 || StaticData.TrackingData.IsJustOnTrail ? _DefaultStreetZoom : customMap.CameraPosition.Zoom;
                                            var tilt = customMap.CameraPosition.Tilt == 0 || StaticData.TrackingData.IsJustOnTrail ? _DefaultStreetTilt : customMap.CameraPosition.Tilt;

                                            await _StreetViewAsync(_LastKnownPosition, zoom, tilt);
                                        }

                                        _SetNextPinBasedOnBearing();
                                    }

                                    _LastCompassHeading = Math.Truncate(e.Heading);
                                }
                            }
                        }
                    };

                    CrossCompass.Current.Start();
                }

                // MoveToCamera with Position and Zoom
                geolocator.PositionChanged += async (sender, e) =>
                {
                    _LastKnownPosition = e.Position;

                    // Set Position for User on trail
                    StaticData.TrackingData.AddUserPosition(e.Position);

                    _FindDistanceToNearestCoordinate();

                    if (StaticData.TrackingData.IsJustOnTrail)
                    {
                        // Set zoom to street view if just go on trail
                        _SetZoomViewToggleButton(MapZooms.Street);
                        _SetStreetViewInitialCameraPosition();
                    }

                    if (e.Position.Heading != 0)
                    {
                        if (_CurrentMapZoom == MapZooms.Street)
                        {
                            // If Street View, Set Camera Position
                            var zoom = customMap.CameraPosition.Zoom == 0 || StaticData.TrackingData.IsJustOnTrail ? _DefaultStreetZoom : customMap.CameraPosition.Zoom;
                            var tilt = customMap.CameraPosition.Tilt == 0 || StaticData.TrackingData.IsJustOnTrail ? _DefaultStreetTilt : customMap.CameraPosition.Tilt;

                            await _StreetViewAsync(e.Position, zoom, tilt);
                        }

                        _SetNextPinBasedOnBearing();
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
                if (StaticData.RouteCoordinates != null && StaticData.RouteCoordinates.Count > 0)
                {
                    var firstLatCoordinate = StaticData.RouteCoordinates.OrderBy(o => o.Latitude).FirstOrDefault();
                    var lastLatCoordinate = StaticData.RouteCoordinates.OrderBy(o => o.Latitude).LastOrDefault();
                    var firstLongCoordinate = StaticData.RouteCoordinates.OrderBy(o => o.Longitude).FirstOrDefault();
                    var lastLongCoordinate = StaticData.RouteCoordinates.OrderBy(o => o.Longitude).LastOrDefault();

                    var centerPosition = StaticHelpers.GetCentralGeoCoordinate(new List<Position>() {
                    new Position(position.Latitude, position.Longitude),
                    new Position(firstLatCoordinate.Latitude, firstLatCoordinate.Longitude), new Position(lastLatCoordinate.Latitude, lastLatCoordinate.Longitude),
                    new Position(firstLongCoordinate.Latitude, firstLongCoordinate.Longitude), new Position(lastLongCoordinate.Latitude, lastLongCoordinate.Longitude) });

                    var distLat = StaticHelpers.CalculateDistance(firstLatCoordinate.Latitude, firstLatCoordinate.Longitude, lastLatCoordinate.Latitude, lastLatCoordinate.Longitude, Units.Miles);
                    var distLong = StaticHelpers.CalculateDistance(firstLongCoordinate.Latitude, firstLongCoordinate.Longitude, lastLongCoordinate.Latitude, lastLongCoordinate.Longitude, Units.Miles);
                    var dist = distLat >= distLong ? distLat : distLong;

                    var distPost = StaticHelpers.CalculateDistance(centerPosition.Latitude, centerPosition.Longitude, position.Latitude, position.Longitude, Units.Miles);
                    dist = distPost >= dist ? distPost : dist;

                    dist = dist == 0 ? 0 : dist / 2;

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
            if (StaticData.RouteCoordinates != null && StaticData.RouteCoordinates.Count > 0)
            {
                var firstLatCoordinate = StaticData.RouteCoordinates.OrderBy(o => o.Latitude).FirstOrDefault();
                var lastLatCoordinate = StaticData.RouteCoordinates.OrderBy(o => o.Latitude).LastOrDefault();
                var firstLongCoordinate = StaticData.RouteCoordinates.OrderBy(o => o.Longitude).FirstOrDefault();
                var lastLongCoordinate = StaticData.RouteCoordinates.OrderBy(o => o.Longitude).LastOrDefault();

                var centerPosition = StaticHelpers.GetCentralGeoCoordinate(new List<Position>() {
                    new Position(firstLatCoordinate.Latitude, firstLatCoordinate.Longitude), new Position(lastLatCoordinate.Latitude, lastLatCoordinate.Longitude),
                    new Position(firstLongCoordinate.Latitude, firstLongCoordinate.Longitude), new Position(lastLongCoordinate.Latitude, lastLongCoordinate.Longitude) });

                var distLat = StaticHelpers.CalculateDistance(firstLatCoordinate.Latitude, firstLatCoordinate.Longitude, lastLatCoordinate.Latitude, lastLatCoordinate.Longitude, Units.Miles);
                var distLong = StaticHelpers.CalculateDistance(firstLongCoordinate.Latitude, firstLongCoordinate.Longitude, lastLongCoordinate.Latitude, lastLongCoordinate.Longitude, Units.Miles);
                var dist = distLat >= distLong ? distLat : distLong;

                dist = dist == 0 ? 0 : dist / 2;

                customMap.MoveToRegion(MapSpan.FromCenterAndRadius(centerPosition, Distance.FromMiles(dist)));
            }
        }

        private void _SetNextPinBasedOnBearing()
        {
            LineSegmentEntity segmentWithNextPin = null;

            //Find Closest Line Segment to current position
            var curPosition = new Position(_LastKnownPosition.Latitude, _LastKnownPosition.Longitude);
            var closestLineSegmentToPosition = customMap.FindClosestLineSegment(curPosition);

            if (closestLineSegmentToPosition != null && closestLineSegmentToPosition.CustomPinOnSegment != null)
            {
                // if heading towards current segment pin, then us that
                if (_IsHeadingTowardsPosition(closestLineSegmentToPosition.CustomPinOnSegment.Pin.Position))
                {
                    segmentWithNextPin = closestLineSegmentToPosition;

                    //Same segement - just calulate distance between pins
                    StaticData.TrackingData.DistanceToNextPin = StaticHelpers.CalculateDistance(curPosition.Latitude, curPosition.Longitude, segmentWithNextPin.CustomPinOnSegment.Pin.Position.Latitude, segmentWithNextPin.CustomPinOnSegment.Pin.Position.Longitude, Units.Miles);
                }
            }

            if (segmentWithNextPin == null)
            {
                LineSegmentEntity upSeqLineSegment = null;
                //Find Segment with Pins up Sequence
                for (int i = closestLineSegmentToPosition.SegmentSequence + 1; i < StaticData.LineSegments.Count; i++)
                {
                    if (StaticData.LineSegments[i - 1].CustomPinOnSegment != null)
                    {
                        upSeqLineSegment = StaticData.LineSegments[i - 1];
                        break;
                    }
                }

                //Find Segment with Pins down Sequence
                LineSegmentEntity downSeqLineSegment = null;
                //Find Segment with Pins down Sequence
                for (int i = closestLineSegmentToPosition.SegmentSequence - 1; i > 0; i--)
                {
                    if (StaticData.LineSegments[i - 1].CustomPinOnSegment != null)
                    {
                        downSeqLineSegment = StaticData.LineSegments[i - 1];
                        break;
                    }
                }

                if (upSeqLineSegment == null && downSeqLineSegment != null)
                {
                    segmentWithNextPin = downSeqLineSegment;
                }
                else if (upSeqLineSegment != null && downSeqLineSegment == null)
                {
                    segmentWithNextPin = upSeqLineSegment;
                }

                if (segmentWithNextPin == null && (downSeqLineSegment != null && upSeqLineSegment != null))
                {
                    if (_IsHeadingTowardsPosition(upSeqLineSegment.CustomPinOnSegment.Pin.Position))
                    {
                        segmentWithNextPin = upSeqLineSegment;
                    }
                    else if (_IsHeadingTowardsPosition(downSeqLineSegment.CustomPinOnSegment.Pin.Position))
                    {
                        segmentWithNextPin = downSeqLineSegment;
                    }
                }

                if (segmentWithNextPin != null)
                {
                    StaticData.TrackingData.DistanceToNextPin = GetDistancetoNextPin(curPosition, closestLineSegmentToPosition, segmentWithNextPin);
                }
            }

            if (segmentWithNextPin != null)
            {
                //Auto Select List View Item
                foreach (var item in StaticData.CustomPins)
                {
                    item.IsStatusInfoVisible = false;
                }
                var selPin = StaticData.CustomPins.Where(s => s.Pin == segmentWithNextPin.CustomPinOnSegment.Pin).FirstOrDefault();
                if (selPin != null)
                {
                    selPin.SetStatusInfo(true, StaticData.TrackingData.StatusInfoBackgroundColor, StaticData.TrackingData.Status, StaticData.TrackingData.DistanceFromTrailCenterDisplay, StaticData.TrackingData.ETAToNextPinDisplay, StaticData.TrackingData.DistanceToNextPinDisplay);
                }

                // Set Next Pin
                StaticData.TrackingData.NextPin = segmentWithNextPin.CustomPinOnSegment.Pin.Label;
                
                if (_CurrentMapZoom == MapZooms.Street)
                {
                    // Auto Select Pin on Street View Only
                    customMap.SelectedPin = segmentWithNextPin.CustomPinOnSegment.Pin;           
                }

               
            }
        }

        private double GetDistancetoNextPin(Position curPosition, LineSegmentEntity closestLineSegmentToCurPosition, LineSegmentEntity pinLineSegment)
        {
            double distance = 0;

            // Get Distance to end of current segment from current position
            if (_IsHeadingTowardsPosition(closestLineSegmentToCurPosition.Position1))
            {
                distance = StaticHelpers.CalculateDistance(curPosition.Latitude, curPosition.Longitude, closestLineSegmentToCurPosition.Position1.Latitude, closestLineSegmentToCurPosition.Position1.Longitude, Units.Miles);
            }
            else
            {
                distance = StaticHelpers.CalculateDistance(curPosition.Latitude, curPosition.Longitude, closestLineSegmentToCurPosition.Position2.Latitude, closestLineSegmentToCurPosition.Position2.Longitude, Units.Miles);
            }

            // Get Segments in between
            if (closestLineSegmentToCurPosition.SegmentSequence > pinLineSegment.SegmentSequence)
            {
                foreach (var seg in StaticData.LineSegments)
                {
                    if (seg.SegmentSequence > pinLineSegment.SegmentSequence && seg.SegmentSequence < closestLineSegmentToCurPosition.SegmentSequence)
                    {
                        distance = distance + seg.SegmentDistanceInMiles;
                    }
                }
            }
            else if (closestLineSegmentToCurPosition.SegmentSequence < pinLineSegment.SegmentSequence)
            {
                foreach (var seg in StaticData.LineSegments)
                {
                    if (seg.SegmentSequence > closestLineSegmentToCurPosition.SegmentSequence && seg.SegmentSequence < pinLineSegment.SegmentSequence)
                    {
                        distance = distance + seg.SegmentDistanceInMiles;
                    }
                }
            }

            var dist1 = StaticHelpers.CalculateDistance(curPosition.Latitude, curPosition.Longitude, pinLineSegment.Position1.Latitude, pinLineSegment.Position1.Longitude, Units.Miles);
            var dist2 = StaticHelpers.CalculateDistance(curPosition.Latitude, curPosition.Longitude, pinLineSegment.Position2.Latitude, pinLineSegment.Position2.Longitude, Units.Miles);

            if (dist1 < dist2)
            {
                //Use Postion 1 as it is closer

                distance = distance + StaticHelpers.CalculateDistance(pinLineSegment.CustomPinOnSegment.Pin.Position.Latitude, pinLineSegment.CustomPinOnSegment.Pin.Position.Longitude, pinLineSegment.Position1.Latitude, pinLineSegment.Position1.Longitude, Units.Miles);
            }
            else
            {
                distance = distance + StaticHelpers.CalculateDistance(pinLineSegment.CustomPinOnSegment.Pin.Position.Latitude, pinLineSegment.CustomPinOnSegment.Pin.Position.Longitude, pinLineSegment.Position2.Latitude, pinLineSegment.Position2.Longitude, Units.Miles);
            }

            return distance;
        }

        private bool _IsHeadingTowardsPosition(Position position)
        {
            if (_LastKnownPosition.Heading > 0 && _LastKnownPosition.Heading < 90)
            {
                // Use Coordinate that is > Lat and > Long
                if (position.Latitude > _LastKnownPosition.Latitude && position.Longitude > _LastKnownPosition.Longitude)
                {
                    return true;
                }
            }
            else if (_LastKnownPosition.Heading > 90 && _LastKnownPosition.Heading < 180)
            {
                // Use Coordinate that is < lat and > Long
                if (position.Latitude < _LastKnownPosition.Latitude && position.Longitude > _LastKnownPosition.Longitude)
                {
                    return true;
                }
            }
            else if (_LastKnownPosition.Heading > 180 && _LastKnownPosition.Heading < 270)
            {
                // Use Coordinate that is < lat and < long
                if (position.Latitude < _LastKnownPosition.Latitude && position.Longitude < _LastKnownPosition.Longitude)
                {
                    return true;
                }
            }
            else if (_LastKnownPosition.Heading > 270 && _LastKnownPosition.Heading < 360)
            {
                // Use Coordinate that is > lat and < long
                if (position.Latitude > _LastKnownPosition.Latitude && position.Longitude < _LastKnownPosition.Longitude)
                {
                    return true;
                }
            }

            return false;
        }

        private void _FindDistanceToNearestCoordinate()
        {
            //var _LastKnownPosition = new Position(40.805293, -74.19676);
            var curPosition = new Position(_LastKnownPosition.Latitude, _LastKnownPosition.Longitude);
            var lineSegment = customMap.FindClosestLineSegment(curPosition);
            StaticData.TrackingData.DistanceFromTrailCenter = lineSegment.ClosestPositionToLocationDistance;

            if (StaticData.TrackingData.IsStatusInfoVisible)
            {
                //Plot From Current Position to Line Segment if status info is visible
                var list = new List<Position>();
                list.Add(curPosition);
                list.Add(lineSegment.ClosestPositionToLocation);

                customMap.PlotClosestPolylineTrack(list);
                
            }
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
            _SetStreetViewInitialCameraPosition();
        }

        private void _SetStreetViewInitialCameraPosition()
        {
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

//Example how to call device specific code
//if (Device.RuntimePlatform == Device.Android)
//{
//    // Access compass directly if not available through google maps
//    var heading = DependencyService.Get<ICompass>().GetHeading();
//    e.Position.Heading = heading;
//}