// ***********************************************************************
// Assembly         : BMCGMobile
// Author           : Derek Maciak
// Created          : 01-22-2018
//
// Last Modified By : Derek Maciak
// Last Modified On : 02-11-2018
// ***********************************************************************
// <copyright file="TrackingEntity.cs" company="">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using BMCGMobile.Resources;
using Plugin.Geolocator.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace BMCGMobile.Entities
{
    /// <summary>
    /// Class TrackingEntity.
    /// </summary>
    /// <seealso cref="BMCGMobile.Entities.EntityBase" />
    public class TrackingEntity : EntityBase
    {

        /// <summary>
        /// The last known position
        /// </summary>
        public Plugin.Geolocator.Abstractions.Position LastKnownPosition;
        /// <summary>
        /// The is on trail
        /// </summary>
        private bool _IsOnTrail;
        /// <summary>
        /// Gets a value indicating whether this instance is on trail.
        /// </summary>
        /// <value><c>true</c> if this instance is on trail; otherwise, <c>false</c>.</value>
        public bool IsOnTrail { get { return StaticHelpers.ConvertMilesToFeet(DistanceFromTrailCenter) < UserSettings.AutoTrackingMaximumDistanceFromTrailInFeet; } }
        /// <summary>
        /// Gets the status.
        /// </summary>
        /// <value>The status.</value>
        public string Status { get { return IsOnTrail ? DesciptionResource.OnTrail : DesciptionResource.OffTrail; } }
        /// <summary>
        /// Gets the color of the status information background.
        /// </summary>
        /// <value>The color of the status information background.</value>
        public Color StatusInfoBackgroundColor { get { return IsOnTrail ? (Color)Application.Current.Resources["StatusInfoOnTrailBackgroundColor"] : (Color)Application.Current.Resources["StatusInfoOffTrailBackgroundColor"]; } }
        /// <summary>
        /// Gets a value indicating whether this instance is status information visible.
        /// </summary>
        /// <value><c>true</c> if this instance is status information visible; otherwise, <c>false</c>.</value>
        public bool IsStatusInfoVisible { get { return DistanceFromTrailCenter <= Variables.DISPLAY_STATUS_INFO_MIN_DIST; } }

        /// <summary>
        /// The user on trail segments
        /// </summary>
        public List<UserOnTrailSegmentEntity> UserOnTrailSegments = new List<UserOnTrailSegmentEntity>();
        /// <summary>
        /// The create new user on trail segment
        /// </summary>
        private bool _CreateNewUserOnTrailSegment = true;

        /// <summary>
        /// The user times on trail
        /// </summary>
        public Dictionary<DateTime, DateTime> UserTimesOnTrail = new Dictionary<DateTime, DateTime>();

        /// <summary>
        /// The distance from trail center
        /// </summary>
        private double _DistanceFromTrailCenter;

        /// <summary>
        /// Gets or sets the distance from trail center.
        /// </summary>
        /// <value>The distance from trail center.</value>
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

        /// <summary>
        /// Gets the distance from trail center display.
        /// </summary>
        /// <value>The distance from trail center display.</value>
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

        /// <summary>
        /// The next pin
        /// </summary>
        private string _NextPin;

        /// <summary>
        /// Gets or sets the next pin.
        /// </summary>
        /// <value>The next pin.</value>
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

        /// <summary>
        /// The distance to next pin
        /// </summary>
        private double _DistanceToNextPin;

        /// <summary>
        /// Gets or sets the distance to next pin.
        /// </summary>
        /// <value>The distance to next pin.</value>
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

        /// <summary>
        /// Gets the distance to next pin display.
        /// </summary>
        /// <value>The distance to next pin display.</value>
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

        /// <summary>
        /// Gets the time to next pin.
        /// </summary>
        /// <value>The time to next pin.</value>
        public TimeSpan TimeToNextPin { get { return TimeSpan.FromHours(DistanceToNextPin / Variables.AVE_WALKING_SPEED); } }

        /// <summary>
        /// Gets the eta to next pin.
        /// </summary>
        /// <value>The eta to next pin.</value>
        public DateTime ETAToNextPin { get { return DateTime.Now.Add(TimeToNextPin); } }

        /// <summary>
        /// Gets the eta to next pin display.
        /// </summary>
        /// <value>The eta to next pin display.</value>
        public string ETAToNextPinDisplay { get { return ETAToNextPin.ToString("t"); } }

        /// <summary>
        /// Gets or sets the user settings.
        /// </summary>
        /// <value>The user settings.</value>
        public SettingsEntity UserSettings { set; get; }

        // Today On Trail Properties

        /// <summary>
        /// The on trail start date time
        /// </summary>
        private DateTime _OnTrailStartDateTime;

        /// <summary>
        /// Gets the on trail time.
        /// </summary>
        /// <value>The on trail time.</value>
        private TimeSpan _OnTrailTime { get { return DateTime.Now - _OnTrailStartDateTime; } }

        /// <summary>
        /// Gets a value indicating whether this instance is just on trail.
        /// </summary>
        /// <value><c>true</c> if this instance is just on trail; otherwise, <c>false</c>.</value>
        public bool IsJustOnTrail { get { return _OnTrailTime.TotalSeconds < 3; } }

        /// <summary>
        /// The total distance on trail today
        /// </summary>
        private double _TotalDistanceOnTrailToday;

        /// <summary>
        /// Gets the total distance on trail today.
        /// </summary>
        /// <value>The total distance on trail today.</value>
        public double TotalDistanceOnTrailToday { get { return _TotalDistanceOnTrailToday; } }

        /// <summary>
        /// Gets the total distance on trail today display.
        /// </summary>
        /// <value>The total distance on trail today display.</value>
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

        /// <summary>
        /// The total time on trail today
        /// </summary>
        private TimeSpan _TotalTimeOnTrailToday;

        /// <summary>
        /// Gets the total time on trail today.
        /// </summary>
        /// <value>The total time on trail today.</value>
        public TimeSpan TotalTimeOnTrailToday { get { return _TotalTimeOnTrailToday; } }

        /// <summary>
        /// Gets the total time on trail today display.
        /// </summary>
        /// <value>The total time on trail today display.</value>
        public string TotalTimeOnTrailTodayDisplay
        {
            get
            {
                return string.Format(DesciptionResource.TimeSpanDisplay, TotalTimeOnTrailToday);
            }
        }

        /// <summary>
        /// The total step count today
        /// </summary>
        private int _TotalStepCountToday;

        /// <summary>
        /// Gets the total step count today.
        /// </summary>
        /// <value>The total step count today.</value>
        public int TotalStepCountToday { get { return _TotalStepCountToday; } }

        /// <summary>
        /// The total calories today
        /// </summary>
        private double _TotalCaloriesToday;

        /// <summary>
        /// Gets the total calories today.
        /// </summary>
        /// <value>The total calories today.</value>
        public double TotalCaloriesToday { get { return _TotalCaloriesToday; } }

        /// <summary>
        /// Gets the total calories today display.
        /// </summary>
        /// <value>The total calories today display.</value>
        public string TotalCaloriesTodayDisplay { get { return TotalCaloriesToday.ToString("N0"); } }

        /// <summary>
        /// Initializes a new instance of the <see cref="TrackingEntity"/> class.
        /// </summary>
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

        /// <summary>
        /// Adds the user position.
        /// </summary>
        /// <param name="position">The position.</param>
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