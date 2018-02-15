// ***********************************************************************
// Assembly         : BMCGMobile
// Author           : Derek Maciak
// Created          : 01-23-2018
//
// Last Modified By : Derek Maciak
// Last Modified On : 02-06-2018
// ***********************************************************************
// <copyright file="CustomPinEntity.cs" company="">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace BMCGMobile.Entities
{
    /// <summary>
    /// Class CustomPinEntity.
    /// </summary>
    /// <seealso cref="BMCGMobile.Entities.EntityBase" />
    public class CustomPinEntity : EntityBase
    {
        /// <summary>
        /// Gets or sets the pin.
        /// </summary>
        /// <value>The pin.</value>
        public Pin Pin { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>The URL.</value>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the type of the pin.
        /// </summary>
        /// <value>The type of the pin.</value>
        public PinTypes PinType { get; set; }

        /// <summary>
        /// Gets the name of the pin image.
        /// </summary>
        /// <value>The name of the pin image.</value>
        public string PinImageName
        {
            get
            {
                return GetPinImageName(PinType);
            }
        }

        /// <summary>
        /// Enum PinTypes
        /// </summary>
        public enum PinTypes
        {
            /// <summary>
            /// The kiosk
            /// </summary>
            Kiosk,

            /// <summary>
            /// The wayfinding
            /// </summary>
            Wayfinding,

            /// <summary>
            /// The poi
            /// </summary>
            POI
        }

        /// <summary>
        /// Gets the name of the pin image.
        /// </summary>
        /// <param name="pinType">Type of the pin.</param>
        /// <returns>System.String.</returns>
        public static string GetPinImageName(PinTypes pinType)
        {
            switch (pinType)
            {
                case PinTypes.Kiosk:
                    return "marker26green.png";

                case PinTypes.Wayfinding:
                    return "marker26red.png";

                case PinTypes.POI:
                    return "marker26yellow.png";

                default:
                    break;
            }

            return "marker26red.png";
        }

        /// <summary>
        /// Gets the color of the pin image.
        /// </summary>
        /// <param name="pinType">Type of the pin.</param>
        /// <returns>Color.</returns>
        public static Color GetPinImageColor(PinTypes pinType)
        {
            switch (pinType)
            {
                case PinTypes.Kiosk:
                    return Color.Green;

                case PinTypes.Wayfinding:
                    return Color.FromHex("#A24437"); //Red

                case PinTypes.POI:
                    return Color.Yellow;

                default:
                    break;
            }

            return Color.Red;
        }

        /// <summary>
        /// The is status information visible
        /// </summary>
        private bool _IsStatusInfoVisible;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is status information visible.
        /// </summary>
        /// <value><c>true</c> if this instance is status information visible; otherwise, <c>false</c>.</value>
        public bool IsStatusInfoVisible
        {
            get { return _IsStatusInfoVisible; }
            set
            {
                if (_IsStatusInfoVisible != value)
                {
                    _IsStatusInfoVisible = value;
                    OnPropertyChanged("IsStatusInfoVisible");
                }
            }
        }

        /// <summary>
        /// The status information background color
        /// </summary>
        private Color _StatusInfoBackgroundColor;

        /// <summary>
        /// Gets or sets the color of the status information background.
        /// </summary>
        /// <value>The color of the status information background.</value>
        public Color StatusInfoBackgroundColor
        {
            get { return _StatusInfoBackgroundColor; }
            set
            {
                if (_StatusInfoBackgroundColor != value)
                {
                    _StatusInfoBackgroundColor = value;
                    OnPropertyChanged("StatusInfoBackgroundColor");
                }
            }
        }

        /// <summary>
        /// The status
        /// </summary>
        private string _Status;

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        public string Status
        {
            get { return _Status; }
            set
            {
                if (_Status != value)
                {
                    _Status = value;
                    OnPropertyChanged("Status");
                }
            }
        }

        /// <summary>
        /// The distance from trail center display
        /// </summary>
        private string _DistanceFromTrailCenterDisplay;

        /// <summary>
        /// Gets or sets the distance from trail center display.
        /// </summary>
        /// <value>The distance from trail center display.</value>
        public string DistanceFromTrailCenterDisplay
        {
            get { return _DistanceFromTrailCenterDisplay; }
            set
            {
                if (_DistanceFromTrailCenterDisplay != value)
                {
                    _DistanceFromTrailCenterDisplay = value;
                    OnPropertyChanged("DistanceFromTrailCenterDisplay");
                }
            }
        }

        /// <summary>
        /// The eta to next pin display
        /// </summary>
        private string _ETAToNextPinDisplay;

        /// <summary>
        /// Gets or sets the eta to next pin display.
        /// </summary>
        /// <value>The eta to next pin display.</value>
        public string ETAToNextPinDisplay
        {
            get { return _ETAToNextPinDisplay; }
            set
            {
                if (_ETAToNextPinDisplay != value)
                {
                    _ETAToNextPinDisplay = value;
                    OnPropertyChanged("ETAToNextPinDisplay");
                }
            }
        }

        /// <summary>
        /// The distance to next pin display
        /// </summary>
        private string _DistanceToNextPinDisplay;

        /// <summary>
        /// Gets or sets the distance to next pin display.
        /// </summary>
        /// <value>The distance to next pin display.</value>
        public string DistanceToNextPinDisplay
        {
            get { return _DistanceToNextPinDisplay; }
            set
            {
                if (_DistanceToNextPinDisplay != value)
                {
                    _DistanceToNextPinDisplay = value;
                    OnPropertyChanged("DistanceToNextPinDisplay");
                }
            }
        }

        /// <summary>
        /// Sets the status information.
        /// </summary>
        /// <param name="isStatusInfoVisible">if set to <c>true</c> [is status information visible].</param>
        /// <param name="statusInfoBackgroundColor">Color of the status information background.</param>
        /// <param name="status">The status.</param>
        /// <param name="distanceFromTrailCenterDisplay">The distance from trail center display.</param>
        /// <param name="etaToNextPinDisplay">The eta to next pin display.</param>
        /// <param name="distanceToNextPinDisplay">The distance to next pin display.</param>
        public void SetStatusInfo(bool isStatusInfoVisible, Color statusInfoBackgroundColor, string status, string distanceFromTrailCenterDisplay, string etaToNextPinDisplay, string distanceToNextPinDisplay)
        {
            IsStatusInfoVisible = isStatusInfoVisible;
            StatusInfoBackgroundColor = statusInfoBackgroundColor;
            Status = status;
            DistanceFromTrailCenterDisplay = distanceFromTrailCenterDisplay;
            ETAToNextPinDisplay = etaToNextPinDisplay;
            DistanceToNextPinDisplay = distanceToNextPinDisplay;
        }
    }
}