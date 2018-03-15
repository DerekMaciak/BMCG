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
using Newtonsoft.Json;
using Plugin.Geolocator.Abstractions;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using static BMCGMobile.Enums;

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
        public bool IsOnTrail
        {
            get
            {
                return _IsOnTrail;
            }

            set
            {
                if (_IsOnTrail != value)
                {
                    _IsOnTrail = value;

                    OnPropertyChanged("IsOnTrail");
                    OnPropertyChanged("Status");
                    OnPropertyChanged("StatusInfoBackgroundColor");

                    if (_IsOnTrail)
                    {
                        _OnTrailStartDateTime = DateTime.Now;
                    }
                }
            }
        }

        private DateTime UserExtendIsOntrailStartTime;

        private bool _UserExtendIsOntrail;

        public bool UserExtendIsOntrail
        {
            get { return _UserExtendIsOntrail; }
            set
            {
                if (_UserExtendIsOntrail != value)
                {
                    _UserExtendIsOntrail = value;

                    if (_UserExtendIsOntrail)
                    {
                        IsOnTrail = true;

                        UserExtendIsOntrailStartTime = DateTime.Now;
                    }
                }
            }
        }

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
        /// Gets the is user is actually on trail The IsOnTrail is used for fitness tracking, this property does not.
        /// </summary>
        /// <value>The is actually on trail.</value>
        public bool IsActuallyOnTrail { get { return StaticHelpers.ConvertMilesToFeet(DistanceFromTrailCenter) < UserSettings.AutoTrackingMaximumDistanceFromTrailInFeet; } }

        /// <summary>
        /// The create new user on trail segment
        /// </summary>
        private bool _CreateNewUserOnTrailSegment = true;

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

                    if (!UserSettings.IsDisplayOffTrailAlert || (UserExtendIsOntrail && (DateTime.Now - UserExtendIsOntrailStartTime).Minutes >= UserSettings.MinutesToExtendOnTrailStatus))
                    {
                        // Reset Extended On Trail Time
                        UserExtendIsOntrail = false;

                        if (UserSettings.IsAutoTracking)
                        {
                            IsOnTrail = IsActuallyOnTrail;
                        }
                        else
                        {
                            IsOnTrail = false;
                        }
                    }

                    if (!UserExtendIsOntrail && UserSettings.IsAutoTracking)
                    {
                        IsOnTrail = IsActuallyOnTrail;
                    }
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
        /// The heading
        /// </summary>
        private double _Heading;

        /// <summary>
        /// Gets or sets the heading.
        /// </summary>
        /// <value>The heading.</value>
        public double Heading
        {
            get { return _Heading; }
            set
            {
                if (_Heading != value)
                {
                    _Heading = value;

                    OnPropertyChanged("Heading");
                }
            }
        }

        /// <summary>
        /// The heading direction
        /// </summary>
        private Direction _HeadingDirection;

        /// <summary>
        /// Gets or sets the heading direction.
        /// </summary>
        /// <value>The Heading direction.</value>
        public Direction HeadingDirection
        {
            get { return _HeadingDirection; }
            set
            {
                if (_HeadingDirection != value)
                {
                    _HeadingDirection = value;

                    OnPropertyChanged("HeadingDirection");
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
        public TimeSpan TimeToNextPin { get { return TimeSpan.FromHours(DistanceToNextPin / UserSettings.AverageWalkingSpeed); } }

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
        public UserSettingsEntity UserSettings { set; get; }

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

        public ObservableCollection<FitnessEntity> FitnessHistory = new ObservableCollection<FitnessEntity>();

        public FitnessEntity FitnessToday
        {
            get
            {
                if (FitnessHistory != null && FitnessHistory.Count > 0)
                {
                    var todayExistingFitness = FitnessHistory.Where(w => w.FitnessDate.Date == DateTime.Now.Date).FirstOrDefault();
                    if (todayExistingFitness != null)
                    {
                        return todayExistingFitness;
                    }
                }
                var todayFitness = new FitnessEntity(DateTime.Now);
                FitnessHistory.Add(todayFitness);
                return todayFitness;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TrackingEntity"/> class.
        /// </summary>
        public TrackingEntity()
        {
            GetSavedProperties();

            MessagingCenter.Subscribe<IStepCounter, int>(this, "StepCount", (sender, args) =>
            {
                if (FitnessToday.UserOnTrailSegments != null && FitnessToday.UserOnTrailSegments.Count > 0)
                {
                    var curSeg = FitnessToday.UserOnTrailSegments.Last();

                    curSeg.TotalSegmentStepCount = args;
                }
            });

            MessagingCenter.Subscribe<IStepCounter, double>(this, "Distance", (sender, args) =>
            {
                if (FitnessToday.UserOnTrailSegments != null && FitnessToday.UserOnTrailSegments.Count > 0)
                {
                    var curSeg = FitnessToday.UserOnTrailSegments.Last();

                    curSeg.TotalSegmentDistanceBySteps = args;
                }
            });
        }

        private void UserSettings_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //Save Entity for User

            _SetUserSettingsProperty();

            App.Current.SavePropertiesAsync();
        }

        /// <summary>
        /// Adds the user position.
        /// </summary>
        /// <param name="position">The position.</param>
        public void AddUserPosition(Position position)
        {
            if (IsOnTrail)
            {
                if (_CreateNewUserOnTrailSegment || FitnessToday.UserOnTrailSegments.Count == 0)
                {
                    FitnessToday.UserOnTrailSegments.Add(new UserOnTrailSegmentEntity(true));

                    MessagingCenter.Send<TrackingEntity>(this, "StartStepUpdates");
                    _CreateNewUserOnTrailSegment = false;
                }

                var currentSegment = FitnessToday.UserOnTrailSegments.Last();

                if (currentSegment.TotalSegmentStepCount > 0)
                {
                    // Wait for active steps before adding User Positions
                    currentSegment.AddUserPosition(position);

                    FitnessToday.CalculateFitness();
                }
            }
            else
            {
                _CreateNewUserOnTrailSegment = true;

                MessagingCenter.Send<TrackingEntity>(this, "StopStepUpdates");
            }
        }

        public void GetSavedProperties()
        {
            // Get User Settings
            var userSettings = new UserSettingsEntity();

            if (App.Current.Properties.Where(s => s.Key == Enums.SavedDataTypes.UserSettings.ToString()).Count() > 0)
            {
                var savedUserSettings = JsonConvert.DeserializeObject<UserSettingsEntity>(App.Current.Properties[Enums.SavedDataTypes.UserSettings.ToString()] as string);
                if (savedUserSettings != null)
                {
                    userSettings = savedUserSettings;
                }
            }

            UserSettings = userSettings;

            // Save straight away of property changes by user
            UserSettings.PropertyChanged += UserSettings_PropertyChanged;

            // Get Fitness History
            var fitnessHistoryList = new ObservableCollection<FitnessEntity>();

            if (App.Current.Properties.Where(s => s.Key == Enums.SavedDataTypes.FitnessHistory.ToString()).Count() > 0)
            {
                var savedFitnessHistoryList = JsonConvert.DeserializeObject<ObservableCollection<FitnessEntity>>(App.Current.Properties[Enums.SavedDataTypes.FitnessHistory.ToString()] as string);
                if (savedFitnessHistoryList != null)
                {
                    fitnessHistoryList = savedFitnessHistoryList;

                    foreach (var item in fitnessHistoryList)
                    {
                        item.CalculateFitness();
                    }
                }
            }

            FitnessHistory = fitnessHistoryList;
        }

        public void SaveProperties()
        {
            _SetUserSettingsProperty();

            _SetFitnessHistoryProperty();

            App.Current.SavePropertiesAsync();
        }

        private void _SetUserSettingsProperty()
        {
            var userSettingsJson = JsonConvert.SerializeObject(UserSettings);

            if (App.Current.Properties.Where(s => s.Key == Enums.SavedDataTypes.UserSettings.ToString()).Count() > 0)
            {
                App.Current.Properties[Enums.SavedDataTypes.UserSettings.ToString()] = userSettingsJson;
            }
            else
            {
                App.Current.Properties.Add(Enums.SavedDataTypes.UserSettings.ToString(), userSettingsJson);
            }
        }

        private void _SetFitnessHistoryProperty()
        {
            if (FitnessToday.UserOnTrailSegments.Count != 0)
            {
                // Only Save if Active Time on trail - Only Save UserSettings.NumFitnessHistoryDaysToKeep.
                var FitnessHistoryToSave = FitnessHistory.OrderByDescending(o => o.FitnessDate).Take(UserSettings.NumFitnessHistoryDaysToKeep);

                var fitnessHistoryJson = JsonConvert.SerializeObject(FitnessHistoryToSave);

                if (App.Current.Properties.Where(s => s.Key == Enums.SavedDataTypes.FitnessHistory.ToString()).Count() > 0)
                {
                    App.Current.Properties[Enums.SavedDataTypes.FitnessHistory.ToString()] = fitnessHistoryJson;
                }
                else
                {
                    App.Current.Properties.Add(Enums.SavedDataTypes.FitnessHistory.ToString(), fitnessHistoryJson);
                }
            }
        }
    }
}