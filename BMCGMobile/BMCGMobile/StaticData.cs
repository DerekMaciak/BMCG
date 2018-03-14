// ***********************************************************************
// Assembly         : BMCGMobile
// Author           : Derek Maciak
// Created          : 02-14-2018
//
// Last Modified By : Derek Maciak
// Last Modified On : 02-14-2018
// ***********************************************************************
// <copyright file="StaticData.cs" company="">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using BMCGMobile.Entities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms.GoogleMaps;

namespace BMCGMobile
{
    /// <summary>
    /// Class StaticData.
    /// </summary>
    public static class StaticData
    {
        /// <summary>
        /// The tracking data
        /// </summary>
        public static TrackingEntity TrackingData = new TrackingEntity();
        /// <summary>
        /// Gets or sets the the greenway trail route coordinates.
        /// </summary>
        /// <value>The route coordinates.</value>
        public static List<Position> GreenwayTrailRouteCoordinates { set; get; }
        /// <summary>
        /// Gets or sets the custom pins.
        /// </summary>
        /// <value>The custom pins.</value>
        public static ObservableCollection<CustomPinEntity> CustomPins { set; get; }
        /// <summary>
        /// Gets or sets the line segments.
        /// </summary>
        /// <value>The line segments.</value>
        public static List<LineSegmentEntity> LineSegments { set; get; }

        /// <summary>
        /// Gets or sets the Morris Canal route coordinates.
        /// </summary>
        /// <value>The route coordinates.</value>
        public static Dictionary<string, List<Position>> MorrisCanalRouteCoordinates { set; get; }

        /// <summary>
        /// Gets or sets the wayfinding coordinates.
        /// </summary>
        /// <value>The wayfinding coordinates.</value>
        public static List<GPXWayPoint> WayfindingCoordinates { set; get; }
    }
}