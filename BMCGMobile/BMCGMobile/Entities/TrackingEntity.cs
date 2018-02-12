using BMCGMobile.Resources;
using Plugin.Geolocator.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public List<UserOnTrailSegmentEntity> UserOnTrailSegments = new List<UserOnTrailSegmentEntity>();
        private bool _CreateNewUserOnTrailSegment = true;

        public Dictionary<DateTime, DateTime> UserTimesOnTrail = new Dictionary<DateTime, DateTime>();

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
                            _OnTrailStartDateTime = DateTime.Now;
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
                    OnPropertyChanged("TimeToNextPin");
                    OnPropertyChanged("ETAToNextPin");
                    OnPropertyChanged("ETAToNextPinDisplay");
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

        public TimeSpan TimeToNextPin { get { return TimeSpan.FromHours(DistanceToNextPin / Variables.AVE_WALKING_SPEED); } }

        public DateTime ETAToNextPin { get { return DateTime.Now.Add(TimeToNextPin); } }

        public string ETAToNextPinDisplay { get { return ETAToNextPin.ToString("t"); } }

        public SettingsEntity UserSettings { set; get; }

        // Today On Trail Properties

        private DateTime _OnTrailStartDateTime;

        private TimeSpan _OnTrailTime { get { return DateTime.Now - _OnTrailStartDateTime; } }

        public bool IsJustOnTrail { get { return _OnTrailTime.TotalSeconds < 3; } }

        private double _TotalDistanceOnTrailToday;

        public double TotalDistanceOnTrailToday { get { return _TotalDistanceOnTrailToday; } }

        public string TotalDistanceOnTrailTodayDisplay
        {
            get
            {
                if (StaticHelpers.ConvertMilesToFeet(TotalDistanceOnTrailToday) < Variables.DISPLAY_AS_FEET_MIN)
                {
                    return string.Format("{0} {1}", StaticHelpers.ConvertMilesToFeet(TotalDistanceOnTrailToday).ToString("N0"), DesciptionResource.Feet);
                }

                return string.Format("{0} {1}", TotalDistanceOnTrailToday.ToString("N2"), DesciptionResource.Miles);
            }
        }

        private TimeSpan _TotalTimeOnTrailToday;

        public TimeSpan TotalTimeOnTrailToday { get { return _TotalTimeOnTrailToday; } }

        public string TotalTimeOnTrailTodayDisplay
        {
            get
            {
                return string.Format(DesciptionResource.TimeSpanDisplay, TotalTimeOnTrailToday);
            }
        }

        private int _TotalStepCountToday;

        public int TotalStepCountToday { get { return _TotalStepCountToday; } }

        private double _TotalCaloriesToday;

        public double TotalCaloriesToday { get { return _TotalCaloriesToday; } }

        public string TotalCaloriesTodayDisplay { get { return TotalCaloriesToday.ToString("N0"); } }

        public TrackingEntity()
        {
            MessagingCenter.Subscribe<IStepCounter, int>(this, "StepCount", (sender, args) =>
            {
                if (UserOnTrailSegments != null && UserOnTrailSegments.Count > 0)
                {
                    var curSeg = UserOnTrailSegments.Last();

                    curSeg.TotalSegmentStepCount = args;
                }
              
            });

            MessagingCenter.Subscribe<IStepCounter, double>(this, "Distance", (sender, args) =>
            {
                if (UserOnTrailSegments != null && UserOnTrailSegments.Count > 0)
                {
                    var curSeg = UserOnTrailSegments.Last();

                    curSeg.TotalSegmentDistanceBySteps = args;
                }
            });
        }

        public void AddUserPosition(Position position)
        {
            if (IsOnTrail)
            {
               
                if (_CreateNewUserOnTrailSegment)
                {
                    UserOnTrailSegments.Add(new UserOnTrailSegmentEntity());

                    MessagingCenter.Send<TrackingEntity>(this, "StartStepUpdates");
                    _CreateNewUserOnTrailSegment = false;
                }

                var currentSegment = UserOnTrailSegments.Last();
                currentSegment.AddUserPosition(position);

                // Add up Total Distance And Time from each Segment
                int totalStepCount = 0;
                double totalDistance = 0;
                TimeSpan totalTime = new TimeSpan();
                double totalCalories = 0;
                foreach (var seg in UserOnTrailSegments)
                {
                    totalStepCount = totalStepCount + seg.TotalSegmentStepCount;
                    totalDistance = totalDistance + seg.TotalSegmentDistanceBySteps;
                    totalTime = totalTime + seg.TotalSegmentTimeSpan;
                    totalCalories = totalCalories + seg.TotalSegmentCaloriesByDistance;
                }

                _TotalStepCountToday = totalStepCount;
                _TotalDistanceOnTrailToday = totalDistance;
                _TotalTimeOnTrailToday = totalTime;
                _TotalCaloriesToday = totalCalories;


                OnPropertyChanged("TotalStepCountToday");
                OnPropertyChanged("TotalDistanceOnTrailToday");
                OnPropertyChanged("TotalDistanceOnTrailTodayDisplay");
                OnPropertyChanged("TotalTimeOnTrailToday");
                OnPropertyChanged("TotalTimeOnTrailTodayDisplay");
                OnPropertyChanged("TotalCaloriesToday");
                OnPropertyChanged("TotalCaloriesTodayDisplay");

            }
            else
            {
                _CreateNewUserOnTrailSegment = true;

                if (UserOnTrailSegments != null && UserOnTrailSegments.Count > 0)
                {
                    var curSeg = UserOnTrailSegments.Last();
                }

                MessagingCenter.Send<TrackingEntity>(this, "StopStepUpdates");
            }
        }
    }
}