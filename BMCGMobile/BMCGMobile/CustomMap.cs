// ***********************************************************************
// Assembly         : BMCGMobile
// Author           : Derek Maciak
// Created          : 10-09-2017
//
// Last Modified By : Derek Maciak
// Last Modified On : 02-14-2018
// ***********************************************************************
// <copyright file="CustomMap.cs" company="">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using BMCGMobile.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using static BMCGMobile.Enums;

namespace BMCGMobile
{
    /// <summary>
    /// Class CustomMap.
    /// </summary>
    /// <seealso cref="Xamarin.Forms.GoogleMaps.Map" />
    public class CustomMap : Map
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomMap"/> class.
        /// </summary>
        public CustomMap()
        {
            // Set initial Map state
            //this.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(40.810657, -74.186378), Distance.FromMiles(1)));
           
        }

      
        public void LoadWayFindingCoordinatePins()
        {
            if (StaticData.WayfindingCoordinates != null)
            {
                foreach (var item in StaticData.WayfindingCoordinates.OrderBy(p => p.Sequence))
                {
                    // var address = string.IsNullOrWhiteSpace(item.Description) ? string.Format(DesciptionResource.LatitudeLongitude, item.Latitude, item.Longitude) : item.Description;
                    var address = item.Description;

                    var pin = new CustomPinEntity
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
                            Icon = BitmapDescriptorFactory.DefaultMarker(CustomPinEntity.GetPinImageColor(item.PinType)),

                            //Icon = BitmapDescriptorFactory.FromBundle(CustomPinEntity.GetPinImageName(item.PinType))
                        },
                    };

                    if (StaticData.CustomPins.Where(p => p.Id == pin.Id).Count() == 0)
                    {
                        // Add Pin
                        StaticData.CustomPins.Add(pin);
                    }
                   
                    Pins.Add(pin.Pin);

                    // Add Pin to Closest Line Segment
                    if (item.PinType == CustomPinEntity.PinTypes.Kiosk || item.PinType == CustomPinEntity.PinTypes.Marker)
                    {
                        var lineSegment = FindClosestLineSegment(new Position(item.Latitude, item.Longitude));
                        // Only 1 pin should be on segment - make sure GPX file complies
                        lineSegment.CustomPinOnSegment = pin;
                    }
                }
            }
        }

        /// <summary>
        /// Plots the polyline track.
        /// </summary>
        public void PlotPolylineTrack()
        {
            // Plot Morris Canal Route Coordinates
            //===============================================
            if (StaticData.MorrisCanalRouteCoordinates != null)
            {
                foreach (var trackSegments in StaticData.MorrisCanalRouteCoordinates)
                {
                    var polylineMorrisCanal = new Polyline();
                    foreach (var tracks in trackSegments.Value)
                    {
                        polylineMorrisCanal.Positions.Add(new Position(tracks.Latitude, tracks.Longitude));
                    }
                    polylineMorrisCanal.IsClickable = false;
                    polylineMorrisCanal.StrokeColor = (Color)Application.Current.Resources["MorrisCanalColor"];
                    polylineMorrisCanal.StrokeWidth = 2.5f;
                    polylineMorrisCanal.Tag = trackSegments.Key;

                    Polylines.Add(polylineMorrisCanal);
                }
            }
            // Plot Greenway Trail Route Coordinates
            //===============================================
            if (StaticData.GreenwayTrailRouteCoordinates != null)
            {
                var polylineGreenwayTrail = new Polyline();

                foreach (var item in StaticData.GreenwayTrailRouteCoordinates)
                {
                    polylineGreenwayTrail.Positions.Add(new Position(item.Latitude, item.Longitude));
                }

                polylineGreenwayTrail.IsClickable = false;
                polylineGreenwayTrail.StrokeColor = (Color)Application.Current.Resources["GreenwayTrailColor"];
                polylineGreenwayTrail.StrokeWidth = 5f;
                polylineGreenwayTrail.Tag = TrackTypes.BloomfieldGreenwayTrail.ToString();

                Polylines.Add(polylineGreenwayTrail);
            }
        }

        /// <summary>
        /// Plots the closest polyline track.
        /// </summary>
        /// <param name="positions">The positions.</param>
        public void PlotClosestPolylineTrack(List<Position> positions)
        {
            var polyline = new Polyline();

            foreach (var pos in positions)
            {
                polyline.Positions.Add(pos);
            }

            polyline.IsClickable = false;
            polyline.StrokeColor = (Color)Application.Current.Resources["TrackerColor"];
            polyline.StrokeWidth = 2.5f;
            polyline.Tag = "TRACKER"; // Can set any object

            foreach (var item in Polylines)
            {
                if (item.Tag == polyline.Tag)
                {
                    Polylines.Remove(item);
                    break;
                }
            }

            Polylines.Add(polyline);
        }

        public void PlotUserOnTrailSegmentsPolylineTrack(DateTime fitnessDate)
        {
            // Plot User On Trail Segments Polyline Track
            //===============================================

            // Remove existing Polyline
            var poyLinesToRemove = new List<Polyline>();
            foreach (var item in Polylines.Where(s => s.Tag.ToString() == "UserOnTrailSegments"))
            {
                poyLinesToRemove.Add(item);
            }
            foreach (var item in poyLinesToRemove)
            {
                Polylines.Remove(item);
            }

            var fitnessEntity = StaticData.TrackingData.FitnessHistory.Where(w => w.FitnessDate.Date == fitnessDate.Date).FirstOrDefault();
            if (fitnessEntity != null)
            {
                foreach (var trackSegments in fitnessEntity.UserOnTrailSegments)
                {
                    var polyline = new Polyline();
                    foreach (var tracks in trackSegments.UserPositionsOnTrail)
                    {
                        polyline.Positions.Add(new Position(tracks.Latitude, tracks.Longitude));
                    }
                    polyline.IsClickable = false;
                    polyline.StrokeColor = (Color)Application.Current.Resources["UserOnTrailSegmentsColor"];
                    polyline.StrokeWidth = 5f;
                    polyline.Tag = "UserOnTrailSegments";

                    if (polyline.Positions.Count() > 2)
                    {
                        Polylines.Add(polyline);
                    }
                }
            }
        }

        /// <summary>
        /// Finds the closest line segment.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns>LineSegmentEntity.</returns>
        public LineSegmentEntity FindClosestLineSegment(Position position)
        {
            foreach (var lineSegment in StaticData.LineSegments)
            {
                lineSegment.ClosestPositionToLocation = StaticHelpers.FindClosestPositionOnLineSegment(position, lineSegment.Position1, lineSegment.Position2);
                lineSegment.ClosestPositionToLocationDistance = StaticHelpers.CalculateDistance(position.Latitude, position.Longitude, lineSegment.ClosestPositionToLocation.Latitude, lineSegment.ClosestPositionToLocation.Longitude, Units.Miles);
            }

            return StaticData.LineSegments.OrderBy(o => o.ClosestPositionToLocationDistance).FirstOrDefault();
        }

        /// <summary>
        /// Determines whether [is point in polygon] [the specified poly].
        /// </summary>
        /// <param name="poly">The poly.</param>
        /// <param name="point">The point.</param>
        /// <returns><c>true</c> if [is point in polygon] [the specified poly]; otherwise, <c>false</c>.</returns>
        public bool IsPointInPolygon(List<Position> poly, Position point)
        {
            var isPointInPolygon = false;

            int i, j;

            for (i = 0, j = poly.Count - 1; i < poly.Count; j = i++)
            {
                if ((((poly[i].Latitude <= point.Latitude) && (point.Latitude < poly[j].Latitude))
                        || ((poly[j].Latitude <= point.Latitude) && (point.Latitude < poly[i].Latitude)))
                        && (point.Longitude < (poly[j].Longitude - poly[i].Longitude) * (point.Latitude - poly[i].Latitude)
                            / (poly[j].Latitude - poly[i].Latitude) + poly[i].Longitude))

                    isPointInPolygon = !isPointInPolygon;
            }

            return isPointInPolygon;
        }

        /// <summary>
        /// Retrieves the address for position.
        /// </summary>
        private async void _RetrieveAddressForPosition()
        {
            foreach (var item in StaticData.CustomPins)
            {
                item.Pin.Address = await _RetrieveAddressForPositionAsync(item.Pin.Position);
                // Add to Map After address has been obtained
                Pins.Add(item.Pin);
            }
        }

        /// <summary>
        /// retrieve address for position as an asynchronous operation.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
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

        public async void CreateMapSnapShotAsync()
        {
            var stream = await this.TakeSnapshot();

            // StaticData.TrackingData.FitnessToday.MapSnapShot = ImageSource.FromStream(() => stream);
        }

        /// <summary>
        /// Centers the map.
        /// </summary>
        /// <param name="position">The position.</param>
        public void CenterMap(Plugin.Geolocator.Abstractions.Position position)
        {
            if (position != null)
            {
                if (StaticData.GreenwayTrailRouteCoordinates != null && StaticData.GreenwayTrailRouteCoordinates.Count > 0)
                {
                    var firstLatCoordinate = StaticData.GreenwayTrailRouteCoordinates.OrderBy(o => o.Latitude).FirstOrDefault();
                    var lastLatCoordinate = StaticData.GreenwayTrailRouteCoordinates.OrderBy(o => o.Latitude).LastOrDefault();
                    var firstLongCoordinate = StaticData.GreenwayTrailRouteCoordinates.OrderBy(o => o.Longitude).FirstOrDefault();
                    var lastLongCoordinate = StaticData.GreenwayTrailRouteCoordinates.OrderBy(o => o.Longitude).LastOrDefault();

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

                    this.MoveToRegion(MapSpan.FromCenterAndRadius(centerPosition, Distance.FromMiles(dist)));
                }
                else
                {
                    this.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude), Distance.FromMiles(15)));
                }
            }
        }

        /// <summary>
        /// Centers the map to pins.
        /// </summary>
        public void CenterMapToPins()
        {
            if (StaticData.GreenwayTrailRouteCoordinates != null && StaticData.GreenwayTrailRouteCoordinates.Count > 0)
            {
                var firstLatCoordinate = StaticData.GreenwayTrailRouteCoordinates.OrderBy(o => o.Latitude).FirstOrDefault();
                var lastLatCoordinate = StaticData.GreenwayTrailRouteCoordinates.OrderBy(o => o.Latitude).LastOrDefault();
                var firstLongCoordinate = StaticData.GreenwayTrailRouteCoordinates.OrderBy(o => o.Longitude).FirstOrDefault();
                var lastLongCoordinate = StaticData.GreenwayTrailRouteCoordinates.OrderBy(o => o.Longitude).LastOrDefault();

                var centerPosition = StaticHelpers.GetCentralGeoCoordinate(new List<Position>() {
                    new Position(firstLatCoordinate.Latitude, firstLatCoordinate.Longitude), new Position(lastLatCoordinate.Latitude, lastLatCoordinate.Longitude),
                    new Position(firstLongCoordinate.Latitude, firstLongCoordinate.Longitude), new Position(lastLongCoordinate.Latitude, lastLongCoordinate.Longitude) });

                var distLat = StaticHelpers.CalculateDistance(firstLatCoordinate.Latitude, firstLatCoordinate.Longitude, lastLatCoordinate.Latitude, lastLatCoordinate.Longitude, Units.Miles);
                var distLong = StaticHelpers.CalculateDistance(firstLongCoordinate.Latitude, firstLongCoordinate.Longitude, lastLongCoordinate.Latitude, lastLongCoordinate.Longitude, Units.Miles);
                var dist = distLat >= distLong ? distLat : distLong;

                dist = dist == 0 ? 0 : dist / 2;

                this.MoveToRegion(MapSpan.FromCenterAndRadius(centerPosition, Distance.FromMiles(dist)));
            }
        }

        public void CenterMapToUserPositions(DateTime fitnessDate)
        {
            var userOnTrailSegments = StaticData.TrackingData.FitnessHistory.Where(w => w.FitnessDate.Date == fitnessDate.Date).FirstOrDefault().UserOnTrailSegments;

            if (userOnTrailSegments != null && userOnTrailSegments.Count > 0)
            {
                var userPositions = new List<Position>();

                foreach (var item in userOnTrailSegments)
                {
                    foreach (var pos in item.UserPositionsOnTrail)
                    {
                        userPositions.Add(new Position(pos.Latitude, pos.Longitude));
                    }
                }

                if (userPositions.Count == 0)
                {
                    CenterMapToPins();
                    return;
                }

                var firstLatCoordinate = userPositions.OrderBy(o => o.Latitude).FirstOrDefault();
                var lastLatCoordinate = userPositions.OrderBy(o => o.Latitude).LastOrDefault();
                var firstLongCoordinate = userPositions.OrderBy(o => o.Longitude).FirstOrDefault();
                var lastLongCoordinate = userPositions.OrderBy(o => o.Longitude).LastOrDefault();

                var centerPosition = StaticHelpers.GetCentralGeoCoordinate(new List<Position>() {
                    new Position(firstLatCoordinate.Latitude, firstLatCoordinate.Longitude), new Position(lastLatCoordinate.Latitude, lastLatCoordinate.Longitude),
                    new Position(firstLongCoordinate.Latitude, firstLongCoordinate.Longitude), new Position(lastLongCoordinate.Latitude, lastLongCoordinate.Longitude) });

                var distLat = StaticHelpers.CalculateDistance(firstLatCoordinate.Latitude, firstLatCoordinate.Longitude, lastLatCoordinate.Latitude, lastLatCoordinate.Longitude, Units.Miles);
                var distLong = StaticHelpers.CalculateDistance(firstLongCoordinate.Latitude, firstLongCoordinate.Longitude, lastLongCoordinate.Latitude, lastLongCoordinate.Longitude, Units.Miles);
                var dist = distLat >= distLong ? distLat : distLong;

                dist = dist == 0 ? 0 : dist / 2;

                this.MoveToRegion(MapSpan.FromCenterAndRadius(centerPosition, Distance.FromMiles(dist)));
            }
            else
            {
                CenterMapToPins();
            }
        }

        #region Test Code

        //public void PlotBufferPolylineTrack()
        //{
        //    var polygon = new Polygon();

        //    var polylineBufferPositions = GetBuffer(RouteCoordinates);

        //    foreach (var item in polylineBufferPositions)
        //    {
        //        polygon.Positions.Add(new Position(item.Latitude, item.Longitude));
        //    }

        //    polygon.IsClickable = false;
        //    polygon.StrokeColor = Color.FromHex("#551A8B"); //Purple
        //    polygon.StrokeWidth = 5f;
        //    polygon.Tag = "POLYLINEBuffer"; // Can set any object

        //    this.Polygons.Add(polygon);
        //}

        //public void PlotBufferPolylineTrack()
        //{
        //    var polyline = new Polyline();

        //    var polylineBufferPositions = GetBuffer(RouteCoordinates);

        //    foreach (var item in polylineBufferPositions)
        //    {
        //        polyline.Positions.Add(new Position(item.Latitude, item.Longitude));
        //    }

        //    polyline.IsClickable = false;
        //    polyline.StrokeColor = Color.FromHex("#551A8B"); //Purple
        //    polyline.StrokeWidth = 5f;
        //    polyline.Tag = "POLYLINEBuffer"; // Can set any object

        //    Polylines.Add(polyline);
        //}

        //public static List<Position> GetBuffer(List<Position> polylinePositions)
        //{
        //    var polylineBufferPositions = new List<Position>();

        //    var polylineBufferPositions1 = new List<Position>();
        //    var polylineBufferPositions2 = new List<Position>();

        //    //// our latitude/longitude values for test
        //    //var g1 = new Point(40.792013, -74.181315);
        //    //var g2 = new Point(40.789584, -74.190936);

        //    // 100 km
        //    double buffer = 5;

        //    Position prevPosition = polylinePositions.FirstOrDefault();

        //    var loop = 0;

        //    foreach (var position in polylinePositions)
        //    {
        //        loop += 1;

        //        if (loop == 40)
        //        {
        //           // break;
        //        }

        //        var g1 = new Point(prevPosition.Latitude, prevPosition.Longitude);

        //        var g2 = new Point(position.Latitude, position.Longitude);

        //        // convert to Mercator (conformal)
        //        double x1, y1, x2, y2;
        //        var m1 = Wgs_2_SphereMercator(g1);
        //        var m2 = Wgs_2_SphereMercator(g2);

        //        // The maxmimum absolute latitude will be used for ground resolution
        //        double maxLat = Math.Max(Math.Abs(g1.Y), Math.Abs(g2.Y));

        //        // calculate the ground resoutlion for the latitude
        //        double groundResolution = Math.Cos(maxLat * Math.PI / 180.0);

        //        // the ground resolution applied to the buffer
        //        var mercatorBuffer = buffer / groundResolution;

        //        // now add the mercator buffer
        //        var mb1 = new Point(m1.X - mercatorBuffer, m1.Y - mercatorBuffer);
        //        var mb2 = new Point(m2.X + mercatorBuffer, m2.Y + mercatorBuffer);

        //        // back to geo
        //        var gb1 = SphereMercator_2_Wgs(mb1);
        //        var gb2 = SphereMercator_2_Wgs(mb2);

        //        polylineBufferPositions1.Add(new Position(gb1.X, gb1.Y));
        //        polylineBufferPositions2.Add(new Position(gb2.X, gb2.Y));

        //        prevPosition = position;

        //    }

        //    polylineBufferPositions.AddRange(polylineBufferPositions1);

        //    polylineBufferPositions2.Reverse();
        //    polylineBufferPositions.AddRange(polylineBufferPositions2);

        //    return polylineBufferPositions;

        //}

        //private static Point Wgs_2_SphereMercator(Point point, double earthRadius = 6371000)
        //{
        //    double x = earthRadius * point.X * Math.PI / 180.0;
        //    double y = earthRadius * Math.Log(Math.Tan(Math.PI / 4.0 + point.Y * Math.PI / 360.0));

        //    return new Point(x, y);
        //}

        //private static Point SphereMercator_2_Wgs(Point point, double earthRadius = 6371000)
        //{
        //    double x = (180.0 / Math.PI) * (point.X / earthRadius);
        //    double y = (360 / Math.PI) * (Math.Atan(Math.Exp(point.Y / earthRadius)) - (Math.PI / 4));

        //    return new Point(x, y);
        //}

        #endregion Test Code
    }
}