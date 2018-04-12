// ***********************************************************************
// Assembly         : BMCGMobile
// Author           : Derek Maciak
// Created          : 01-22-2018
//
// Last Modified By : Derek Maciak
// Last Modified On : 02-14-2018
// ***********************************************************************
// <copyright file="SettingsEntity.cs" company="">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace BMCGMobile.Entities
{
    /// <summary>
    /// Class SettingsEntity.
    /// </summary>
    /// <seealso cref="BMCGMobile.Entities.EntityBase" />
    public class UserSettingsEntity : EntityBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserSettingsEntity" /> class.
        /// </summary>
        public UserSettingsEntity()
        {
            SetDefaults();
        }

        /// <summary>
        /// Sets the defaults.
        /// </summary>
        public void SetDefaults()
        {
            AutoTrackingMaximumDistanceFromTrailInFeet = 50; // 50 Feet
            IsAutoTracking = true;
            IsDisplayOffTrailAlert = false;
            HeightFeet = 5;
            HeightInches = 8;
            WeightInlbs = 180;
            AverageWalkingSpeed = 3.1; // 3.1 Miles per hour
            NumFitnessHistorySessionsToKeep = 14;
            MinutesToExtendOnTrailStatus = 5;
            IsShowMarkersOnFitnessMap = false;

        }

        /// <summary>
        /// Gets or sets the automatic tracking maximum distance from trail in feet.
        /// </summary>
        /// <value>The automatic tracking maximum distance from trail in feet.</value>
        private double _AutoTrackingMaximumDistanceFromTrailInFeet;

        /// <summary>
        /// Gets or sets the automatic tracking maximum distance from trail in feet.
        /// </summary>
        /// <value>The automatic tracking maximum distance from trail in feet.</value>
        public double AutoTrackingMaximumDistanceFromTrailInFeet
        {
            get { return _AutoTrackingMaximumDistanceFromTrailInFeet; }
            set
            {

                if (_AutoTrackingMaximumDistanceFromTrailInFeet != value)
                {
                    _AutoTrackingMaximumDistanceFromTrailInFeet = value;

                    OnPropertyChanged("AutoTrackingMaximumDistanceFromTrailInFeet");

                }
            }
        }



        /// <summary>
        /// Gets or sets a value indicating whether this instance is automatic tracking.
        /// </summary>
        /// <value><c>true</c> if this instance is automatic tracking; otherwise, <c>false</c>.</value>
        private bool _IsAutoTracking;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is automatic tracking.
        /// </summary>
        /// <value><c>true</c> if this instance is automatic tracking; otherwise, <c>false</c>.</value>
        public bool IsAutoTracking
        {
            get { return _IsAutoTracking; }
            set
            {
                if (_IsAutoTracking != value)
                {
                    _IsAutoTracking = value;

                    OnPropertyChanged("IsAutoTracking");

                    if (!_IsAutoTracking)
                    {
                        IsDisplayOffTrailAlert = false;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is display off trail alert.
        /// </summary>
        /// <value><c>true</c> if this instance is display off trail alert; otherwise, <c>false</c>.</value>
        private bool _IsDisplayOffTrailAlert;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is display off trail alert.
        /// </summary>
        /// <value><c>true</c> if this instance is display off trail alert; otherwise, <c>false</c>.</value>
        public bool IsDisplayOffTrailAlert
        {
            get { return _IsDisplayOffTrailAlert; }
            set
            {
                if (_IsDisplayOffTrailAlert != value)
                {
                    _IsDisplayOffTrailAlert = value;

                    OnPropertyChanged("IsDisplayOffTrailAlert");

                    if (_IsDisplayOffTrailAlert)
                    {
                        IsAutoTracking = true;
                    }
                }
            }
        }

        /// <summary>
        /// The minutes to extend on trail status
        /// </summary>
        private int _MinutesToExtendOnTrailStatus;

        /// <summary>
        /// Gets or sets the minutes to extend on trail status.
        /// </summary>
        /// <value>The minutes to extend on trail status.</value>
        public int MinutesToExtendOnTrailStatus
        {
            get { return _MinutesToExtendOnTrailStatus; }
            set
            {

                if (_MinutesToExtendOnTrailStatus != value)
                {
                    _MinutesToExtendOnTrailStatus = value;

                    OnPropertyChanged("MinutesToExtendOnTrailStatus");

                }
            }
        }

        /// <summary>
        /// Gets or sets the height in inches.
        /// </summary>
        /// <value>The height in inches.</value>
        public int HeightInInches { get { return (_HeightFeet * 12) + _HeightInches; } }


        /// <summary>
        /// The height feet
        /// </summary>
        private int _HeightFeet;

        /// <summary>
        /// Gets or sets the height feet.
        /// </summary>
        /// <value>The height feet.</value>
        public int HeightFeet
        {
            get { return _HeightFeet; }
            set {

                if (_HeightFeet != value)
                {
                    _HeightFeet = value;

                    OnPropertyChanged("HeightFeet");
                   
                }
                }
        }


        /// <summary>
        /// The height inches
        /// </summary>
        private int _HeightInches;

        /// <summary>
        /// Gets or sets the height inches.
        /// </summary>
        /// <value>The height inches.</value>
        public int HeightInches
        {
            get { return _HeightInches; }
            set
            {

                if (_HeightInches != value)
                {
                    _HeightInches = value;

                    OnPropertyChanged("HeightInches");

                }
            }
        }


        /// <summary>
        /// The weight inlbs
        /// </summary>
        private int _WeightInlbs;

        /// <summary>
        /// Gets or sets the weight inlbs.
        /// </summary>
        /// <value>The weight inlbs.</value>
        public int WeightInlbs
        {
            get { return _WeightInlbs; }
            set
            {

                if (_WeightInlbs != value)
                {
                    _WeightInlbs = value;

                    OnPropertyChanged("WeightInlbs");

                }
            }
        }


        /// <summary>
        /// The cadence
        /// </summary>
        private int _Cadence;

        /// <summary>
        /// Gets or sets the cadence.
        /// </summary>
        /// <value>The cadence.</value>
        public int Cadence
        {
            get { return _Cadence; }
            set
            {

                if (_Cadence != value)
                {
                    _Cadence = value;

                    OnPropertyChanged("Cadence");

                }
            }
        }


        /// <summary>
        /// The average walking speed
        /// </summary>
        private double _AverageWalkingSpeed;

        /// <summary>
        /// Gets or sets the average walking speed.
        /// </summary>
        /// <value>The average walking speed.</value>
        public double AverageWalkingSpeed
        {
            get { return _AverageWalkingSpeed; }
            set
            {

                if (_AverageWalkingSpeed != value)
                {
                    _AverageWalkingSpeed = value;

                    OnPropertyChanged("AverageWalkingSpeed");

                }
            }
        }

        /// <summary>
        /// The number fitness history days to keep
        /// </summary>
        private int _NumFitnessHistorySessionsToKeep;

        /// <summary>
        /// Gets or sets the number fitness history days to keep.
        /// </summary>
        /// <value>The number fitness history days to keep.</value>
        public int NumFitnessHistorySessionsToKeep
        {
            get { return _NumFitnessHistorySessionsToKeep; }
            set
            {

                if (_NumFitnessHistorySessionsToKeep != value)
                {
                    _NumFitnessHistorySessionsToKeep = value;

                    OnPropertyChanged("NumFitnessHistorySessionsToKeep");

                }
            }
        }


        /// <summary>
        /// The is show markers on fitness map
        /// </summary>
        private bool _IsShowMarkersOnFitnessMap;


        /// <summary>
        /// Gets or sets a value indicating whether this instance is show markers on fitness map.
        /// </summary>
        /// <value><c>true</c> if this instance is show markers on fitness map; otherwise, <c>false</c>.</value>
        public bool IsShowMarkersOnFitnessMap
        {
            get { return _IsShowMarkersOnFitnessMap; }
            set
            {
                if (_IsShowMarkersOnFitnessMap != value)
                {
                    _IsShowMarkersOnFitnessMap = value;

                    OnPropertyChanged("IsShowMarkersOnFitnessMap");

                }
            }
        }
    }
}