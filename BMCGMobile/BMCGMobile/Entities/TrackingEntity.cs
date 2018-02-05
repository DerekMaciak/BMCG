using BMCGMobile.Resources;
using System;
using Xamarin.Forms;

namespace BMCGMobile.Entities
{
    public class TrackingEntity : EntityBase
    {
        private bool _IsOnTrail;
        public bool IsOnTrail { get { return StaticHelpers.ConvertMilesToFeet(DistanceFromTrailCenter) < UserSettings.AutoTrackingMaximumDistanceFromTrailInFeet; } }
        public string Status { get { return IsOnTrail ? DesciptionResource.OnTrail : DesciptionResource.OffTrail; } }
        public Color StatusInfoBackgroundColor { get { return IsOnTrail ? (Color)Application.Current.Resources["StatusInfoOnTrailBackgroundColor"] : (Color)Application.Current.Resources["StatusInfoOffTrailBackgroundColor"]; } }
        public bool IsStatusInfoVisible { get { return DistanceFromTrailCenter <= Variables.DISPLAY_STATUS_INFO_MIN_DIST; } }

        private double _DistanceFromTrailCenter;

        public double DistanceFromTrailCenter
        {
            get { return _DistanceFromTrailCenter; }
            set
            {
                if (_DistanceFromTrailCenter != value)
                {
                    _DistanceFromTrailCenter = value;
                    OnPropertyChanged("DistanceFromTrailCenter");
                    OnPropertyChanged("DistanceFromTrailCenterDisplay");
                    OnPropertyChanged("IsStatusInfoVisible");

                    if (IsOnTrail != _IsOnTrail)
                    {
                        OnPropertyChanged("IsOnTrail");
                        OnPropertyChanged("Status");
                        OnPropertyChanged("StatusInfoBackgroundColor");

                        if (IsOnTrail)
                        {
                            OnTrailStartDateTime = DateTime.Now;
                        }
                        else
                        {
                            OnTrailEndDateTime = DateTime.Now;
                        }
                    }

                    _IsOnTrail = IsOnTrail;
                }
            }
        }

        public string DistanceFromTrailCenterDisplay
        {
            get
            {
                if (StaticHelpers.ConvertMilesToFeet(DistanceFromTrailCenter) < Variables.DISPLAY_AS_FEET_MIN)
                {
                    return string.Format("{0} {1}", StaticHelpers.ConvertMilesToFeet(DistanceFromTrailCenter).ToString("N0"), DesciptionResource.Feet);
                }

                return string.Format("{0} {1}", DistanceFromTrailCenter.ToString("N2"), DesciptionResource.Miles);
            }
        }

        private string _NextPin;

        public string NextPin
        {
            get { return _NextPin; }
            set
            {
                if (_NextPin != value)
                {
                    _NextPin = value;
                    OnPropertyChanged("NextPin");
                }
            }
        }

        private double _DistanceToNextPin;

        public double DistanceToNextPin
        {
            get { return _DistanceToNextPin; }
            set
            {
                if (_DistanceToNextPin != value)
                {
                    _DistanceToNextPin = value;

                    OnPropertyChanged("DistanceToNextPin");
                    OnPropertyChanged("DistanceToNextPinDisplay");
                }
            }
        }

        public string DistanceToNextPinDisplay
        {
            get
            {
                if (StaticHelpers.ConvertMilesToFeet(DistanceToNextPin) < Variables.DISPLAY_AS_FEET_MIN)
                {
                    return string.Format("{0} {1}", StaticHelpers.ConvertMilesToFeet(DistanceToNextPin).ToString("N0"), DesciptionResource.Feet);
                }

                return string.Format("{0} {1}", DistanceToNextPin.ToString("N2"), DesciptionResource.Miles);
            }
        }

        public TimeSpan TimeToNextPin { set; get; }

        public DateTime ETAToNextPin { get { return DateTime.Now + TimeToNextPin; } }

        public string ETAToNextPinDisplay { get { return ETAToNextPin.ToString("t"); } }

        public SettingsEntity UserSettings { set; get; }

        public DateTime OnTrailStartDateTime { set; get; }

        public DateTime OnTrailEndDateTime { set; get; }

        public TimeSpan OnTrailTime { get { return DateTime.Now - OnTrailStartDateTime; } }

        public bool IsJustOnTrail { get { return OnTrailTime.TotalSeconds < 3; } }
    }
}