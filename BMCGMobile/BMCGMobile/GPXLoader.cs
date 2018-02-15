// ***********************************************************************
// Assembly         : BMCGMobile
// Author           : Derek Maciak
// Created          : 12-03-2017
//
// Last Modified By : Derek Maciak
// Last Modified On : 01-23-2018
// ***********************************************************************
// <copyright file="GPXLoader.cs" company="">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using static BMCGMobile.Entities.CustomPinEntity;

namespace BMCGMobile
{
    /// <summary>
    /// Class GPXLoader.
    /// </summary>
    public class GPXLoader
    {
        /// <summary>
        /// The file content
        /// </summary>
        private static string _FileContent;

        /// <summary>
        /// Download GPX file as an asynchronous operation.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        private async Task<string> _DownloadGPXFileAsync(string url)
        {
            try
            {
                var uri = new Uri(url);
                var client = new HttpClient();

                var response = await client.GetStringAsync(uri);

                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Load the Xml document for parsing
        /// </summary>
        /// <param name="fileContent">Content of the file.</param>
        /// <returns>XDocument</returns>
        private XDocument GetGpxDoc(string fileContent)
        {
            return XDocument.Parse(fileContent);
        }

        /// <summary>
        /// Load the namespace for a standard GPX document
        /// </summary>
        /// <returns>XNamespace.</returns>
        private XNamespace GetGpxNameSpace()
        {
            return XNamespace.Get("http://www.topografix.com/GPX/1/1");
        }

        /// <summary>
        /// When passed a url, download it and parse all waypoints from it.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>List&lt;GPXWayPoint&gt;.</returns>
        public List<GPXWayPoint> LoadGPXWayPoints(string url)
        {
            if (string.IsNullOrWhiteSpace(_FileContent))
            {
                _FileContent = Task.Run(async () => { return await _DownloadGPXFileAsync(url); }).Result;
            }

            try
            {
                var gpxDoc = GetGpxDoc(_FileContent);
                var gpx = GetGpxNameSpace();

                var waypoints = from waypoint in gpxDoc.Descendants(gpx + "wpt")
                                select new
                                {
                                    Latitude = waypoint.Attribute("lat").Value,
                                    Longitude = waypoint.Attribute("lon").Value,
                                    Elevation = waypoint.Element(gpx + "ele") != null ?
                                      waypoint.Element(gpx + "ele").Value : null,
                                    Name = waypoint.Element(gpx + "name") != null ?
                                      waypoint.Element(gpx + "name").Value : null,
                                    Description = waypoint.Element(gpx + "desc") != null ?
                                      waypoint.Element(gpx + "desc").Value : null,
                                    Comment = waypoint.Element(gpx + "cmt") != null ?
                                      waypoint.Element(gpx + "cmt").Value : null
                                };

                var gpxWayPoints = new List<GPXWayPoint>();
                foreach (var wpt in waypoints)
                {
                    var jsonComment = JsonConvert.DeserializeObject<JsonComment>(wpt.Comment);
                    var pinTypeString = string.IsNullOrWhiteSpace(wpt.Comment) ? null : jsonComment.PinType;
                    var pinType = string.IsNullOrWhiteSpace(pinTypeString) ? PinTypes.POI : (PinTypes)Enum.Parse(typeof(PinTypes), pinTypeString);

                    gpxWayPoints.Add(new GPXWayPoint(wpt.Name, wpt.Description, jsonComment.Sequence, pinType, jsonComment?.URL, Convert.ToDouble(wpt.Latitude), Convert.ToDouble(wpt.Longitude)));
                }

                return gpxWayPoints;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// When passed a url, download it and parse all tracks
        /// and track segments from it.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>List&lt;GPXLocation&gt;.</returns>
        public List<GPXLocation> LoadGPXTracks(string url)
        {
            if (string.IsNullOrWhiteSpace(_FileContent))
            {
                _FileContent = Task.Run(async () => { return await _DownloadGPXFileAsync(url); }).Result;
            }

            try
            {
                var gpxDoc = GetGpxDoc(_FileContent);
                var gpx = GetGpxNameSpace();
                var tracks = from track in gpxDoc.Descendants(gpx + "trk")
                             select new
                             {
                                 Name = track.Element(gpx + "name") != null ?
                                track.Element(gpx + "name").Value : null,
                                 Segs = (
                                    from trackpoint in track.Descendants(gpx + "trkpt")
                                    select new
                                    {
                                        Latitude = trackpoint.Attribute("lat").Value,
                                        Longitude = trackpoint.Attribute("lon").Value,
                                        Elevation = trackpoint.Element(gpx + "ele") != null ?
                                        trackpoint.Element(gpx + "ele").Value : null,
                                        Time = trackpoint.Element(gpx + "time") != null ?
                                        trackpoint.Element(gpx + "time").Value : null
                                    }
                                  )
                             };

                var gpxLocations = new List<GPXLocation>();
                foreach (var trk in tracks)
                {
                    // Populate track data objects.
                    foreach (var trkSeg in trk.Segs)
                    {
                        gpxLocations.Add(new GPXLocation(Convert.ToDouble(trkSeg.Latitude), Convert.ToDouble(trkSeg.Longitude)));
                    }
                }
                return gpxLocations;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Class JsonComment.
        /// </summary>
        private class JsonComment
        {
            /// <summary>
            /// Gets or sets the type of the pin.
            /// </summary>
            /// <value>The type of the pin.</value>
            public string PinType { get; set; }
            /// <summary>
            /// Gets or sets the URL.
            /// </summary>
            /// <value>The URL.</value>
            public string URL { get; set; }
            /// <summary>
            /// Gets or sets the sequence.
            /// </summary>
            /// <value>The sequence.</value>
            public int Sequence { get; set; }
        }
    }

    /// <summary>
    /// Class GPXWayPoint.
    /// </summary>
    public class GPXWayPoint
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets the type of the pin.
        /// </summary>
        /// <value>The type of the pin.</value>
        public PinTypes PinType { get; set; }
        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>The URL.</value>
        public string URL { get; set; }
        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        /// <value>The latitude.</value>
        public double Latitude { get; set; }
        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        /// <value>The longitude.</value>
        public double Longitude { get; set; }
        /// <summary>
        /// Gets or sets the sequence.
        /// </summary>
        /// <value>The sequence.</value>
        public int Sequence { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GPXWayPoint"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="sequence">The sequence.</param>
        /// <param name="pinType">Type of the pin.</param>
        /// <param name="url">The URL.</param>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        public GPXWayPoint(string name, string description, int sequence, PinTypes pinType, string url, double latitude, double longitude)
        {
            Name = name;
            Description = description;
            Sequence = sequence;
            PinType = pinType;
            URL = url;
            Latitude = latitude;
            Longitude = longitude;
        }
    }

    /// <summary>
    /// Class GPXLocation.
    /// </summary>
    public class GPXLocation
    {
        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        /// <value>The latitude.</value>
        public double Latitude { get; set; }
        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        /// <value>The longitude.</value>
        public double Longitude { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GPXLocation"/> class.
        /// </summary>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        public GPXLocation(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}