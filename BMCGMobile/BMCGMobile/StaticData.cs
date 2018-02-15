using BMCGMobile.Entities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms.GoogleMaps;

namespace BMCGMobile
{
    public static class StaticData
    {
        public static TrackingEntity TrackingData = new TrackingEntity();
        public static List<Position> RouteCoordinates { set; get;}
        public static ObservableCollection<CustomPinEntity> CustomPins { set; get; }
        public static List<LineSegmentEntity> LineSegments { set; get; }
    }
}