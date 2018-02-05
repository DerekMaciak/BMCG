using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.GoogleMaps;

namespace BMCGMobile.Entities
{
    public class LineSegmentEntity : EntityBase
    {
      
        public int SegmentSequence { set; get; }
        public Position Position1 { set; get; }
        public Position Position2 { set; get; }
        public double SegmentDistanceInMiles { set; get; }

        public Position ClosestPositionToLocation { set; get; }
        public double ClosestPositionToLocationDistance { set; get; }
      
        // Only 1 pin should be on segment - make sure GPX file complies
        public CustomPinEntity CustomPinOnSegment { set; get; }


        public LineSegmentEntity(int segmentSequence, Position position1, Position position2, double segmentDistanceInMiles)
        {
            SegmentSequence = segmentSequence;
            Position1 = position1;
            Position2 = position2;
            SegmentDistanceInMiles = segmentDistanceInMiles;
            
        }
    }
}
