using BMCGMobile.Entities;
using BMCGMobile.Resources;
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
        private static List<CustomPinEntity> _CustomPins;

        public static List<Position> RouteCoordinates { get { return _RouteCoordinates; } }
        public static List<CustomPinEntity> CustomPins { get { return _CustomPins; } }

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
                    _CustomPins = new List<CustomPinEntity>();

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
                            var address = string.IsNullOrWhiteSpace(item.Description) ? string.Format(DesciptionResource.LatitudeLongitude, item.Latitude, item.Longitude) : item.Description;

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

                            _CustomPins.Add(pin);
                        }

                        _RetrieveAddressForPosition();

                        PlotPolylineTrack();
                       // PlotBufferPolylineTrack();
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
            polyline.StrokeColor = (Color)Application.Current.Resources["TrailColor"]; //Red
            polyline.StrokeWidth = 10f;
            polyline.Tag = "POLYLINE"; // Can set any object

            Polylines.Add(polyline);
        }

        #region Test Code

        public void PlotClosestPolylineTrack(List<Point> points)
        {
            var polyline = new Polyline();

            foreach (var item in points)
            {
                polyline.Positions.Add(new Position(item.X, item.Y));
            }

            polyline.IsClickable = false;
            polyline.StrokeColor = Color.FromHex("#551A8B"); //Purple
            polyline.StrokeWidth = 5f;
            polyline.Tag = "POLYLINEBuffer"; // Can set any object

            Polylines.Add(polyline);
        }

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
        #endregion TestCode


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