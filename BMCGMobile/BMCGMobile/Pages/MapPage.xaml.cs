// ***********************************************************************
// Assembly         : BMCGMobile
// Author           : Derek Maciak
// Created          : 01-23-2018
//
// Last Modified By : Derek Maciak
// Last Modified On : 02-14-2018
// ***********************************************************************
// <copyright file="MapPage.xaml.cs" company="">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using BMCGMobile.Entities;
using BMCGMobile.Resources;
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
    /// <summary>
    /// Class MapPage.
    /// </summary>
    /// <seealso cref="Xamarin.Forms.ContentPage" />
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPage : ContentPage
    {
        /// <summary>
        /// The current map zoom
        /// </summary>
        private MapZooms _CurrentMapZoom = MapZooms.All;

        /// <summary>
        /// The default street zoom
        /// </summary>
        private double _DefaultStreetZoom = 19d;

        /// <summary>
        /// The default street tilt
        /// </summary>
        private double _DefaultStreetTilt = 70d;

        /// <summary>
        /// The last compass header
        /// </summary>
        private double _LastCompassHeading;

        /// <summary>
        /// The last position change update
        /// </summary>
        private DateTime _LastPositionChangeUpdate = DateTime.Now;

        /// <summary>
        /// The last Compass change update
        /// </summary>
        private DateTime _LastCompassChangeUpdate = DateTime.Now;

        private bool _FirstTimeOnTrail = true;

        private bool _FirstTime = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapPage"/> class.
        /// </summary>
        public MapPage()
        {
            InitializeComponent();

            if (Device.RuntimePlatform == Device.Android)
            {
                // Move StatusInfo to Right for Android
                AbsoluteLayout.SetLayoutBounds(statusInfo, new Rectangle(1, 0, 280, 110));
            }

            // Set DistanceFromTrailCenter so that IsOnTrail is false while loading
            StaticData.TrackingData.DistanceFromTrailCenter = double.MaxValue;

            StaticData.TrackingData.PropertyChanged += _TrackingEntity_PropertyChanged;

            this.BindingContext = StaticData.TrackingData;

            _SetMapViewToggleButton(MapType.None);
            _SetZoomViewToggleButton(MapZooms.None);
        }

        /// <summary>
        /// Handles the PropertyChanged event of the _TrackingEntity control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        private void _TrackingEntity_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsOnTrail")
            {
                if (!StaticData.TrackingData.IsOnTrail && StaticData.TrackingData.UserSettings.IsAutoTracking && StaticData.TrackingData.UserSettings.IsDisplayOffTrailAlert)
                {
                    _DisplayAlertAsync(DesciptionResource.OffTrailQuestion);
                }

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

        private async void _DisplayAlertAsync(string alert)
        {
            if (alert == DesciptionResource.OffTrailQuestion)
            {
                StaticData.TrackingData.UserExtendIsOntrail = await DisplayAlert(DesciptionResource.Question, string.Format(DesciptionResource.OffTrailQuestion, StaticData.TrackingData.UserSettings.MinutesToExtendOnTrailStatus), "Yes", "No");
            }
            else if (alert == DesciptionResource.OnTrailQuestion)
            {
                StaticData.TrackingData.IsOnTrail = await DisplayAlert(DesciptionResource.Question, string.Format(DesciptionResource.OnTrailQuestion, StaticData.TrackingData.DistanceFromTrailCenterDisplay, DesciptionResource.AutoTrack, DesciptionResource.TodayFitnessTracking), "Yes", "No");
            }
            else if (alert == DesciptionResource.FarFromTrailQuestion)
            {
                StaticData.TrackingData.IsOnTrail = !await DisplayAlert(DesciptionResource.Question, string.Format(DesciptionResource.FarFromTrailQuestion, StaticData.TrackingData.DistanceFromTrailCenterDisplay, DesciptionResource.AutoTrack, DesciptionResource.TodayFitnessTracking), "Yes", "No");
            }
        }

        /// <summary>
        /// When overridden, allows application developers to customize behavior immediately prior to the <see cref="T:Xamarin.Forms.Page" /> becoming visible.
        /// </summary>
        /// <remarks>To be added.</remarks>
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            
            try
            {
                if (_FirstTime)
                {
                    _FirstTime = false;

                    var termsPage = new TermsPage();
                    termsPage.Disappearing += (sender, e) =>
                    {
                        timerSplash.IsVisible = true;

                        Task.Run(async () =>
                        {
                            await Task.Delay(5000);
                            timerSplash.IsVisible = false;
                        });

                    };
                    await Navigation.PushModalAsync(termsPage);
                    
                    await StaticData.LoadMapCoordinatesAsync();
                    customMap.LoadWayFindingCoordinatePins();
                    customMap.PlotPolylineTrack();

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

                    customMap.IsVisible = true;

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
                await DisplayAlert(DesciptionResource.Error, string.Format(DesciptionResource.ErrorMessage, ex), "Ok");
            }
        }

        /// <summary>
        /// Handles the InfoWindowClicked event of the CustomMap control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="InfoWindowClickedEventArgs"/> instance containing the event data.</param>
        private void CustomMap_InfoWindowClicked(object sender, InfoWindowClickedEventArgs e)
        {
            var selectedCustomPin = StaticData.CustomPins.Where(s => s.Id == e.Pin.Tag.ToString()).FirstOrDefault();
            Device.OpenUri(new Uri(selectedCustomPin.Url));
        }

        private int goodReadingCount = 0;

        /// <summary>
        /// Retrieves the current location.
        /// </summary>
        /// <returns>Task.</returns>
        private async Task _RetrieveCurrentLocation()
        {
            try
            {
                var geolocator = CrossGeolocator.Current;
                geolocator.DesiredAccuracy = 1;

                if (!geolocator.IsListening)
                {
                    CrossCompass.Current.CompassChanged += async (s, e) =>
                    {
                        var trueHeading = e.Heading;

                        if (Device.RuntimePlatform == Device.Android && StaticData.TrackingData.LastKnownPosition != null)
                        {
                        // Get declination from Android class
                        var declination = DependencyService.Get<IGeomagneticField>().GetGeomagneticField(
                                (float)StaticData.TrackingData.LastKnownPosition.Latitude,
                                 (float)StaticData.TrackingData.LastKnownPosition.Longitude,
                                 (float)StaticData.TrackingData.LastKnownPosition.Altitude,
                                DateTime.Now.Millisecond);

                            trueHeading += declination;
                        }

                        StaticData.TrackingData.Heading = Math.Truncate(trueHeading);
                        StaticData.TrackingData.HeadingDirection = StaticHelpers.GetDirectionHeading((int)Math.Truncate(trueHeading));

                        var headingChanged = true;

                        if (StaticData.TrackingData.LastKnownPosition != null)
                        {
                            headingChanged = StaticHelpers.IsSectorDifferent(trueHeading, _LastCompassHeading);

                            if (Device.RuntimePlatform == Device.Android)
                            {
                                if (headingChanged)
                                {
                                    goodReadingCount = 0;

                                    headingChanged = false;
                                }

                                if (Math.Abs(trueHeading - _LastCompassHeading) <= 4)
                                {
                                    goodReadingCount += 1;
                                }

                                if (goodReadingCount == 2)
                                {
                                    headingChanged = true;
                                }
                            }

                            _LastCompassHeading = trueHeading;

                            if (headingChanged || (DateTime.Now - _LastCompassChangeUpdate).Seconds >= 10)
                            {
                                var newPosition = new Plugin.Geolocator.Abstractions.Position(StaticData.TrackingData.LastKnownPosition.Latitude, StaticData.TrackingData.LastKnownPosition.Longitude) { Heading = trueHeading };

                                await _StreetViewAsync(newPosition);

                                _LastCompassChangeUpdate = DateTime.Now;
                            }

                            _SetNextPinBasedOnBearing();
                        }
                    };

                    CrossCompass.Current.Start();

                    // MoveToCamera with Position and Zoom
                    geolocator.PositionChanged += (sender, e) =>
                    {
                        var positionChanged = true;

                        if (StaticData.TrackingData.LastKnownPosition != null)
                        {
                            if ((e.Position.Latitude == StaticData.TrackingData.LastKnownPosition.Latitude) && (e.Position.Longitude == StaticData.TrackingData.LastKnownPosition.Longitude))
                            {
                                positionChanged = false;
                            }

                        //Check if position is valid
                        var timeInSeconds = (e.Position.Timestamp - StaticData.TrackingData.LastKnownPosition.Timestamp).TotalSeconds;
                            if (timeInSeconds > 0)
                            {
                                var distInFeet = StaticHelpers.CalculateDistance(e.Position.Latitude, e.Position.Longitude, StaticData.TrackingData.LastKnownPosition.Latitude, StaticData.TrackingData.LastKnownPosition.Longitude, Units.Feet);

                                var feetPerSecond = distInFeet / timeInSeconds;

                                if (feetPerSecond > 7.33333)
                                {
                                // 5 Mile Per Hour = 7.33333 Feet Per Second
                                // Bad position - Diregard it - A person will not be going faster than 5mph on trail
                                return;
                                }
                            }
                        }

                        StaticData.TrackingData.LastKnownPosition = e.Position;

                        if (positionChanged)
                        {
                            if ((DateTime.Now - _LastPositionChangeUpdate).Seconds > 3)
                            {
                            // Set Position for User on trail
                            StaticData.TrackingData.AddUserPosition(e.Position);

                                _LastPositionChangeUpdate = DateTime.Now;
                            }

                            _FindDistanceToNearestCoordinate();

                            if (StaticData.TrackingData.IsJustOnTrail)
                            {
                            // Set zoom to street view if just go on trail
                            _SetZoomViewToggleButton(MapZooms.Street);
                                _SetStreetViewInitialCameraPosition();
                            }
                        }
                    };

                    await geolocator.StartListeningAsync(new TimeSpan(1000), 1, true);
                }

                StaticData.TrackingData.LastKnownPosition = await geolocator.GetPositionAsync();

                customMap.CenterMap(StaticData.TrackingData.LastKnownPosition);
            }
            catch (Exception ex)
            {
                await DisplayAlert(DesciptionResource.Error, string.Format(DesciptionResource.ErrorMessage, ex), "Ok");
            }
        }

        /// <summary>
        /// street view as an asynchronous operation.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns>Task.</returns>
        private async Task _StreetViewAsync(Plugin.Geolocator.Abstractions.Position position)
        {
            if (_CurrentMapZoom == MapZooms.Street)
            {
                // If Street View, Set Camera Position
                var zoom = customMap.CameraPosition.Zoom == 0 || StaticData.TrackingData.IsJustOnTrail ? _DefaultStreetZoom : customMap.CameraPosition.Zoom;
                var tilt = customMap.CameraPosition.Tilt == 0 || StaticData.TrackingData.IsJustOnTrail ? _DefaultStreetTilt : customMap.CameraPosition.Tilt;

                await customMap.AnimateCamera(CameraUpdateFactory.NewCameraPosition(
                                 new CameraPosition(
                                     new Position(position.Latitude, position.Longitude),
                                     zoom,
                                     position.Heading, // bearing(rotation)
                                     tilt
                                     )));
            }
        }

        /// <summary>
        /// Sets the next pin based on bearing.
        /// </summary>
        private void _SetNextPinBasedOnBearing()
        {
            LineSegmentEntity segmentWithNextPin = null;

            //Find Closest Line Segment to current position
            var curPosition = new Position(StaticData.TrackingData.LastKnownPosition.Latitude, StaticData.TrackingData.LastKnownPosition.Longitude);
            var closestLineSegmentToPosition = customMap.FindClosestLineSegment(curPosition);

            if (closestLineSegmentToPosition != null && closestLineSegmentToPosition.CustomPinOnSegment != null)
            {
                // if heading towards current segment pin, then us that
                //if (_IsHeadingTowardsPosition(closestLineSegmentToPosition.CustomPinOnSegment.Pin.Position))
                if (StaticHelpers.IsHeadingTowardsPosition(StaticData.TrackingData.Heading, curPosition, closestLineSegmentToPosition.CustomPinOnSegment.Pin.Position))
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
                    //if (_IsHeadingTowardsPosition(upSeqLineSegment.CustomPinOnSegment.Pin.Position))
                    if (StaticHelpers.IsHeadingTowardsPosition(StaticData.TrackingData.Heading, curPosition, upSeqLineSegment.CustomPinOnSegment.Pin.Position))
                    {
                        segmentWithNextPin = upSeqLineSegment;
                    }
                    //else if (_IsHeadingTowardsPosition(downSeqLineSegment.CustomPinOnSegment.Pin.Position))
                    else if (StaticHelpers.IsHeadingTowardsPosition(StaticData.TrackingData.Heading, curPosition, downSeqLineSegment.CustomPinOnSegment.Pin.Position))
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
                    selPin.SetStatusInfo(true, StaticData.TrackingData.StatusInfoBackgroundColor, StaticData.TrackingData.Status, StaticData.TrackingData.DistanceFromTrailCenterDisplay, StaticData.TrackingData.ETAToNextPinDisplay, StaticData.TrackingData.DistanceToNextPinDisplay, StaticData.TrackingData.HeadingDirection);
                }

                // Set Next Pin
                StaticData.TrackingData.NextPin = segmentWithNextPin.CustomPinOnSegment.Pin.Label;

                if (_CurrentMapZoom == MapZooms.Street)
                {
                    // Auto Select Pin on Street View Only
                    segmentWithNextPin.CustomPinOnSegment.Pin.IsVisible = true;
                    customMap.SelectedPin = segmentWithNextPin.CustomPinOnSegment.Pin;
                }
            }
        }

        /// <summary>
        /// Gets the distanceto next pin.
        /// </summary>
        /// <param name="curPosition">The current position.</param>
        /// <param name="closestLineSegmentToCurPosition">The closest line segment to current position.</param>
        /// <param name="pinLineSegment">The pin line segment.</param>
        /// <returns>System.Double.</returns>
        private double GetDistancetoNextPin(Position curPosition, LineSegmentEntity closestLineSegmentToCurPosition, LineSegmentEntity pinLineSegment)
        {
            double distance = 0;

            // Get Distance to end of current segment from current position
            //if (_IsHeadingTowardsPosition(closestLineSegmentToCurPosition.Position1))
            if (StaticHelpers.IsHeadingTowardsPosition(StaticData.TrackingData.Heading, curPosition, closestLineSegmentToCurPosition.Position1))
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

        /// <summary>
        /// Determines whether [is heading towards position] [the specified position].
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns><c>true</c> if [is heading towards position] [the specified position]; otherwise, <c>false</c>.</returns>
        //private bool _IsHeadingTowardsPosition(Position position)
        //{
        //    if (StaticData.TrackingData.Heading > 0 && StaticData.TrackingData.Heading < 90)
        //    {
        //        // Use Coordinate that is > Lat and > Long
        //        if (position.Latitude > StaticData.TrackingData.LastKnownPosition.Latitude && position.Longitude > StaticData.TrackingData.LastKnownPosition.Longitude)
        //        {
        //            return true;
        //        }
        //    }
        //    else if (StaticData.TrackingData.Heading > 90 && StaticData.TrackingData.Heading < 180)
        //    {
        //        // Use Coordinate that is < lat and > Long
        //        if (position.Latitude < StaticData.TrackingData.LastKnownPosition.Latitude && position.Longitude > StaticData.TrackingData.LastKnownPosition.Longitude)
        //        {
        //            return true;
        //        }
        //    }
        //    else if (StaticData.TrackingData.Heading > 180 && StaticData.TrackingData.Heading < 270)
        //    {
        //        // Use Coordinate that is < lat and < long
        //        if (position.Latitude < StaticData.TrackingData.LastKnownPosition.Latitude && position.Longitude < StaticData.TrackingData.LastKnownPosition.Longitude)
        //        {
        //            return true;
        //        }
        //    }
        //    else if (StaticData.TrackingData.Heading > 270 && StaticData.TrackingData.Heading < 360)
        //    {
        //        // Use Coordinate that is > lat and < long
        //        if (position.Latitude > StaticData.TrackingData.LastKnownPosition.Latitude && position.Longitude < StaticData.TrackingData.LastKnownPosition.Longitude)
        //        {
        //            return true;
        //        }
        //    }

        //    return false;
        //}

        /// <summary>
        /// Finds the distance to nearest coordinate.
        /// </summary>
        private void _FindDistanceToNearestCoordinate()
        {
            //var _LastKnownPosition = new Position(40.805293, -74.19676);
            var curPosition = new Position(StaticData.TrackingData.LastKnownPosition.Latitude, StaticData.TrackingData.LastKnownPosition.Longitude);
            var lineSegment = customMap.FindClosestLineSegment(curPosition);
            StaticData.TrackingData.DistanceFromTrailCenter = lineSegment.ClosestPositionToLocationDistance;

            customMap.RemovePolylineTrack("TRACKER");

            if (StaticData.TrackingData.IsStatusInfoVisible && !StaticData.TrackingData.IsActuallyOnTrail)
            {
                //Plot From Current Position to Line Segment if status info is visible
                var list = new List<Position>();
                list.Add(curPosition);
                list.Add(lineSegment.ClosestPositionToLocation);

                customMap.PlotClosestPolylineTrack(list);
            }

            if (_FirstTimeOnTrail && StaticData.TrackingData.IsActuallyOnTrail && !StaticData.TrackingData.IsOnTrail && !StaticData.TrackingData.UserSettings.IsAutoTracking)
            {
                _FirstTimeOnTrail = false;
                // First Time on Trail and Auto Detect if off.  Notify User that they need to turn on Fitness tracking.
                _DisplayAlertAsync(DesciptionResource.OnTrailQuestion);
            }
            if (StaticData.TrackingData.DistanceFromTrailCenter > Variables.DISPLAY_STATUS_INFO_MIN_DIST && StaticData.TrackingData.IsOnTrail && !StaticData.TrackingData.UserSettings.IsAutoTracking)
            {
                // Auto Tracking is turned off and they user is outside the range of the trail. Prompt them to turn off Is On Trail.
                _DisplayAlertAsync(DesciptionResource.FarFromTrailQuestion);
            }
        }

        /// <summary>
        /// Handles the Clicked event of the MapViewButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
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

        /// <summary>
        /// Handles the Clicked event of the StreetViewButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void StreetViewButton_Clicked(object sender, EventArgs e)
        {
            _SetZoomViewToggleButton(MapZooms.Street);
            _SetStreetViewInitialCameraPosition();
        }

        /// <summary>
        /// Sets the street view initial camera position.
        /// </summary>
        private void _SetStreetViewInitialCameraPosition()
        {
            customMap.AnimateCamera(CameraUpdateFactory.NewCameraPosition(
                                 new CameraPosition(
                                     new Position(StaticData.TrackingData.LastKnownPosition.Latitude, StaticData.TrackingData.LastKnownPosition.Longitude),
                                     _DefaultStreetZoom,
                                     StaticData.TrackingData.LastKnownPosition.Heading, // bearing(rotation)
                                     _DefaultStreetTilt
                                     )));
        }

        /// <summary>
        /// Handles the Clicked event of the ZoomPinsButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ZoomPinsButton_Clicked(object sender, EventArgs e)
        {
            _SetZoomViewToggleButton(MapZooms.Pins);
            customMap.CenterMapToPins();
        }

        /// <summary>
        /// Handles the Clicked event of the ZoomAllButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ZoomAllButton_Clicked(object sender, EventArgs e)
        {
            _SetZoomViewToggleButton(MapZooms.All);
            customMap.CenterMap(StaticData.TrackingData.LastKnownPosition);
        }

        /// <summary>
        /// Handles the Clicked event of the ZoomUserButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ZoomUserButton_Clicked(object sender, EventArgs e)
        {
            _SetZoomViewToggleButton(MapZooms.User);
            customMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(StaticData.TrackingData.LastKnownPosition.Latitude, StaticData.TrackingData.LastKnownPosition.Longitude), Distance.FromMiles(1)));
        }

        /// <summary>
        /// Sets the zoom view toggle button.
        /// </summary>
        /// <param name="mapZoom">The map zoom.</param>
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

        /// <summary>
        /// Sets the map view toggle button.
        /// </summary>
        /// <param name="mapType">Type of the map.</param>
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