using BMCGMobile.Entities;
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

        public static double ConvertFeetToMiles(double feet)
        {
            return feet * 0.000189394;
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

        public static double CaloriesBurnt(double miles)
        {
            const int caloriesBurntPerMile = 100;
            var val = miles * caloriesBurntPerMile;

            return val;

        }

      
        /// Calories burnt with weight entered
        /// view-source:http://walking.about.com/library/cal/uccalc1.htm
       
        public static double CaloriesBurnt(double miles, float lbs, string cadence)
        {
            if (lbs <= 0)
                return CaloriesBurnt(miles);

            var met = 3.5;
            var pace = 3.0;
            switch (cadence)
            {
                case "0":
                    met = 2.0;
                    pace = 2.0;
                    break;
                case "1":
                    met = 2.5;
                    pace = 2.0;
                    break;
                case "2":
                    met = 3.0;
                    pace = 2.5;
                    break;
                case "3":
                    met = 3.5;
                    pace = 3.0;
                    break;
                case "4":
                    met = 5.0;
                    pace = 4.0;
                    break;
                case "5":
                    met = 6.3;
                    pace = 4.5;
                    break;
                case "6":
                    met = 8.0;
                    pace = 5.0;
                    break;
            }

            var adjusted_weight = lbs / 2.2;
            var val = Math.Round(((adjusted_weight * met) / pace) * miles);
            return val;
        }


        //https://www.beachbodyondemand.com/blog/how-many-steps-walk-per-mile
        public static double ConvertStepsToMiles(int steps)
        {
            return ConvertFeetToMiles((StaticData.TrackingData.UserSettings.HeightInInches * Variables.AVE_STRIDE_MULTIPLE) / 12) * steps;
        }
    }
}
