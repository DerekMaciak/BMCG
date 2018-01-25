using BMCGMobile.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BMCGMobile.Entities
{
    public class TrackingEntity : EntityBase
    {
        public TrackingEntity()
        {
            NextPin = "WayFinding Sign # 1";
            DistanceToNextWayFindingSign = 1000;
            TimeToNextWayFindingSign = new TimeSpan(0, 10, 30);
        }


        public bool IsOnTrail { get { return DistanceFromTrailCenter < UserSettings.AutoTrackingMaximumDistanceFromTrail; } }
        public string Status { get { return IsOnTrail ? DesciptionResource.OnTrail : DesciptionResource.OffTrail; } }
        public Color StatusInfoBackgroundColor { get { return IsOnTrail ? (Color)Application.Current.Resources["StatusInfoOnTrailBackgroundColor"] : (Color)Application.Current.Resources["StatusInfoOffTrailBackgroundColor"]; } }

        private double _DistanceFromTrailCenter;

        public double DistanceFromTrailCenter
        {
            get { return _DistanceFromTrailCenter; }
            set { _DistanceFromTrailCenter = value;
                OnPropertyChanged("DistanceFromTrailCenter");
                OnPropertyChanged("DistanceFromTrailCenterDisplay");
                OnPropertyChanged("IsOnTrail");
                OnPropertyChanged("Status");
                OnPropertyChanged("StatusInfoBackgroundColor");

            }
        }
        
        public string DistanceFromTrailCenterDisplay { get { return string.Format("{0} {1}", DistanceFromTrailCenter.ToString("N0"), "Feet"); } }
        public string NextPin { set; get; }

        public double DistanceToNextWayFindingSign { set; get; }

        public string DistanceToNextWayFindingSignDisplay { get { return string.Format("{0} {1}", DistanceToNextWayFindingSign.ToString("N0"), "Feet"); } }

        public TimeSpan TimeToNextWayFindingSign { set; get; }

        public DateTime ETAToNextWayFindingSign { get { return DateTime.Now + TimeToNextWayFindingSign; } }

        public string ETAToNextWayFindingSignDisplay { get { return ETAToNextWayFindingSign.ToString("t"); } }

        public SettingsEntity UserSettings { set; get; }

    }
}
