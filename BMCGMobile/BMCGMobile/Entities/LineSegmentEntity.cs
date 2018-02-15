// ***********************************************************************
// Assembly         : BMCGMobile
// Author           : Derek Maciak
// Created          : 01-25-2018
//
// Last Modified By : Derek Maciak
// Last Modified On : 01-25-2018
// ***********************************************************************
// <copyright file="LineSegmentEntity.cs" company="">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using Xamarin.Forms.GoogleMaps;

namespace BMCGMobile.Entities
{
    /// <summary>
    /// Class LineSegmentEntity.
    /// </summary>
    /// <seealso cref="BMCGMobile.Entities.EntityBase" />
    public class LineSegmentEntity : EntityBase
    {
        /// <summary>
        /// Gets or sets the segment sequence.
        /// </summary>
        /// <value>The segment sequence.</value>
        public int SegmentSequence { set; get; }
        /// <summary>
        /// Gets or sets the position1.
        /// </summary>
        /// <value>The position1.</value>
        public Position Position1 { set; get; }
        /// <summary>
        /// Gets or sets the position2.
        /// </summary>
        /// <value>The position2.</value>
        public Position Position2 { set; get; }
        /// <summary>
        /// Gets or sets the segment distance in miles.
        /// </summary>
        /// <value>The segment distance in miles.</value>
        public double SegmentDistanceInMiles { set; get; }

        /// <summary>
        /// Gets or sets the closest position to location.
        /// </summary>
        /// <value>The closest position to location.</value>
        public Position ClosestPositionToLocation { set; get; }
        /// <summary>
        /// Gets or sets the closest position to location distance.
        /// </summary>
        /// <value>The closest position to location distance.</value>
        public double ClosestPositionToLocationDistance { set; get; }

        // Only 1 pin should be on segment - make sure GPX file complies
        /// <summary>
        /// Gets or sets the custom pin on segment.
        /// </summary>
        /// <value>The custom pin on segment.</value>
        public CustomPinEntity CustomPinOnSegment { set; get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LineSegmentEntity"/> class.
        /// </summary>
        /// <param name="segmentSequence">The segment sequence.</param>
        /// <param name="position1">The position1.</param>
        /// <param name="position2">The position2.</param>
        /// <param name="segmentDistanceInMiles">The segment distance in miles.</param>
        public LineSegmentEntity(int segmentSequence, Position position1, Position position2, double segmentDistanceInMiles)
        {
            SegmentSequence = segmentSequence;
            Position1 = position1;
            Position2 = position2;
            SegmentDistanceInMiles = segmentDistanceInMiles;
        }
    }
}