using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.GoogleMaps;
using static BMCGMobile.Enums;

namespace BMCGMobile
{
   public static class StaticHelpers
    {
        // Calculate the distance between
        // point pt and the segment p1 --> p2.
        //http://csharphelper.com/blog/2016/09/find-the-shortest-distance-between-a-point-and-a-line-segment-in-c/
        public static Position FindClosestPositionOnLineSegment(Position pt, Position p1, Position p2)
        {
            var closest = new Position();

            double dx = p2.Latitude - p1.Latitude;
            double dy = p2.Longitude - p1.Longitude;
            if ((dx == 0) && (dy == 0))
            {
                // It's a point not a line segment.
                closest = p1;
            }

            // Calculate the t that minimizes the distance.
            double t = ((pt.Latitude - p1.Latitude) * dx + (pt.Longitude - p1.Longitude) * dy) /
                (dx * dx + dy * dy);

            // See if this represents one of the segment's
            // end points or a point in the middle.
            if (t < 0)
            {
                closest = new Position(p1.Latitude, p1.Longitude);
            }
            else if (t > 1)
            {
                closest = new Position(p2.Latitude, p2.Longitude);
            }
            else
            {
                closest = new Position(p1.Latitude + t * dx, p1.Longitude + t * dy);
            }

            return closest;
        }

        public static double CalculateDistance(double lat1, double lon1, double lat2, double lon2, Units unit)
        {
            double theta = lon1 - lon2;
            double dist = Math.Sin(deg2rad(lat1)) * Math.Sin(deg2rad(lat2)) + Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) * Math.Cos(deg2rad(theta));
            dist = Math.Acos(dist);
            dist = rad2deg(dist);
            dist = dist * 60 * 1.1515; // Miles - Default

            if (unit == Units.Kilometers)
            {
                dist = dist * 1.609344;
            }
            else if (unit == Units.Feet)
            {
                dist = dist * 5280;
            }
            else if (unit == Units.Yards)
            {
                dist = dist * 1760;
            }

            return (dist);
        }

        public static double ConvertMilesToFeet(double miles)
        {
            return miles * 5280;
        }

        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        //::  This function converts decimal degrees to radians             :::
        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        private static double deg2rad(double deg)
        {
            return (deg * Math.PI / 180.0);
        }

        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        //::  This function converts radians to decimal degrees             :::
        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        private static double rad2deg(double rad)
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
