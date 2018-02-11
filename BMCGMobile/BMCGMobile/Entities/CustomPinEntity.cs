using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace BMCGMobile.Entities
{
    public class CustomPinEntity : EntityBase
    {
        public Pin Pin { get; set; }

        public string Id { get; set; }

        public string Url { get; set; }

        public PinTypes PinType { get; set; }

        public string PinImageName
        {
            get
            {
                return GetPinImageName(PinType);
            }
        }

        public enum PinTypes
        {
            Kiosk,
            Wayfinding,
            POI
        }

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

        private bool _IsStatusInfoVisible;

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

        private Color _StatusInfoBackgroundColor;

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

        private string _Status;

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


        private string _DistanceFromTrailCenterDisplay;

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


        private string _ETAToNextPinDisplay;

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

        private string _DistanceToNextPinDisplay;

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