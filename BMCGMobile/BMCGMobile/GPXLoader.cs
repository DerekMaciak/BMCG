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
    public class GPXLoader
    {
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
        /// <param name="sFile">Fully qualified file name (local)</param>
        /// <returns>XDocument</returns>
        private XDocument GetGpxDoc(string fileContent)
        {
            return XDocument.Parse(fileContent);
        }

        /// <summary>
        /// Load the namespace for a standard GPX document
        /// </summary>
        /// <returns></returns>
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

        private class JsonComment
        {
            public string PinType { get; set; }
            public string URL { get; set; }
            public int Sequence { get; set; }
        }
    }

    public class GPXWayPoint
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public PinTypes PinType { get; set; }
        public string URL { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int Sequence { get; set; }

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

    public class GPXLocation
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public GPXLocation(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}