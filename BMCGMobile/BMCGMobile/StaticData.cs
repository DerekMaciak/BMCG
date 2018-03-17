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
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms.GoogleMaps;
using static BMCGMobile.Enums;

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

        /// <summary>
        /// Loads the map coordinates.
        /// </summary>
        public static async Task LoadMapCoordinatesAsync()
        {
            try
            {
                if (!TrackingData.IsGPXDataLoaded)
                {
                    await Task.Run(() =>
                     {
                         StaticData.GreenwayTrailRouteCoordinates = new List<Position>();
                         StaticData.MorrisCanalRouteCoordinates = new Dictionary<string, List<Position>>();
                         StaticData.CustomPins = new ObservableCollection<CustomPinEntity>();
                         StaticData.LineSegments = new List<LineSegmentEntity>();

                         var gpxLoader = new GPXLoader();

                         // load track coordinates
                         var trackCoordinates = gpxLoader.LoadGPXTracks(Variables.GPX_URL);
                         if (trackCoordinates != null)
                         {
                             // Bloomfield Greenway Trail
                             foreach (var item in trackCoordinates.Where(s => s.TrackName == TrackTypes.BloomfieldGreenwayTrail.ToString()))
                             {
                                 // Just Combine Track Seqments
                                 StaticData.GreenwayTrailRouteCoordinates.Add(new Position(item.Latitude, item.Longitude));
                             }

                             // Morris Canal
                             foreach (var item in trackCoordinates.Where(s => s.TrackName.StartsWith("Morris Canal")))
                             {
                                 var curTrackSeq = string.Format("Name{0}Track{1}Segment{2}", item.TrackName, item.Track, item.Segment);

                                 if (StaticData.MorrisCanalRouteCoordinates.ContainsKey(curTrackSeq))
                                 {
                                     StaticData.MorrisCanalRouteCoordinates[curTrackSeq].Add(new Position(item.Latitude, item.Longitude));
                                 }
                                 else
                                 {
                                     StaticData.MorrisCanalRouteCoordinates.Add(curTrackSeq, new List<Position>() { new Position(item.Latitude, item.Longitude) });
                                 }
                             }
                         }

                         // Load line Segments Greenway Trail RouteCoordinates
                         Position lastPosition = new Position();
                         var segmentSequence = 0;
                         foreach (var position in StaticData.GreenwayTrailRouteCoordinates)
                         {
                             if (segmentSequence == 0)
                             {
                                 lastPosition = position;
                                 segmentSequence += 1;
                                 continue;
                             }

                             var lineSegment = new LineSegmentEntity(segmentSequence, lastPosition, position, StaticHelpers.CalculateDistance(lastPosition.Latitude, lastPosition.Longitude, position.Latitude, position.Longitude, Units.Miles));

                             if (!StaticData.LineSegments.Contains(lineSegment))
                             {
                                 StaticData.LineSegments.Add(lineSegment);
                             }

                             lastPosition = position;
                             segmentSequence += 1;
                         }

                         // Load Wayfinding Pins
                         StaticData.WayfindingCoordinates = gpxLoader.LoadGPXWayPoints(Variables.GPX_URL);
                     });

                    TrackingData.IsGPXDataLoaded = true;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}