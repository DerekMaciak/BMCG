using BMCGMobile.Resources;
using System;
using System.Collections.Generic;

namespace BMCGMobile.Entities
{
    public class FitnessEntity : EntityBase
    {
        public FitnessEntity()
        {
        }

        public FitnessEntity(DateTime fitnessDate)
        {
            FitnessDate = fitnessDate;
        }

        public DateTime FitnessDate { get; set; }

        /// <summary>
        /// The user on trail segments
        /// </summary>
        public List<UserOnTrailSegmentEntity> UserOnTrailSegments = new List<UserOnTrailSegmentEntity>();

        /// <summary>
        /// The total distance on trail
        /// </summary>
        private double _TotalDistanceOnTrail;

        /// <summary>
        /// Gets the total distance on trail.
        /// </summary>
        /// <value>The total distance on trail.</value>
        public double TotalDistanceOnTrail { get { return _TotalDistanceOnTrail; } }

        /// <summary>
        /// Gets the total distance on trail display.
        /// </summary>
        /// <value>The total distance on trail display.</value>
        public string TotalDistanceOnTrailDisplay
        {
            get
            {
                if (StaticHelpers.ConvertMilesToFeet(TotalDistanceOnTrail) < Variables.DISPLAY_AS_FEET_MIN)
                {
                    return string.Format("{0} {1}", StaticHelpers.ConvertMilesToFeet(TotalDistanceOnTrail).ToString("N0"), DesciptionResource.FeetAbrev);
                }

                return string.Format("{0} {1}", TotalDistanceOnTrail.ToString("N2"), DesciptionResource.MilesAbrev);
            }
        }

        /// <summary>
        /// The total time on trail
        /// </summary>
        private TimeSpan _TotalTimeOnTrail;

        /// <summary>
        /// Gets the total time on trail.
        /// </summary>
        /// <value>The total time on trail.</value>
        public TimeSpan TotalTimeOnTrail { get { return _TotalTimeOnTrail; } }

        /// <summary>
        /// Gets the total time on trail display.
        /// </summary>
        /// <value>The total time on trail display.</value>
        public string TotalTimeOnTrailDisplay
        {
            get
            {
                return string.Format(DesciptionResource.TimeSpanDisplay, TotalTimeOnTrail);
            }
        }

        /// <summary>
        /// The total step count
        /// </summary>
        private int _TotalStepCount;

        /// <summary>
        /// Gets the total step count.
        /// </summary>
        /// <value>The total step count.</value>
        public int TotalStepCount { get { return _TotalStepCount; } }

        /// <summary>
        /// The total calories
        /// </summary>
        private double _TotalCalories;

        /// <summary>
        /// Gets the total calories.
        /// </summary>
        /// <value>The total calories.</value>
        public double TotalCalories { get { return _TotalCalories; } }

        /// <summary>
        /// Gets the total calories display.
        /// </summary>
        /// <value>The total calories display.</value>
        public string TotalCaloriesDisplay { get { return TotalCalories.ToString("N0"); } }

        public void CalculateFitness()
        {
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

            _TotalStepCount = totalStepCount;
            _TotalDistanceOnTrail = totalDistance;
            _TotalTimeOnTrail = totalTime;
            _TotalCalories = totalCalories;

            OnPropertyChanged("TotalStepCount");
            OnPropertyChanged("TotalDistanceOnTrail");
            OnPropertyChanged("TotalDistanceOnTrailDisplay");
            OnPropertyChanged("TotalTimeOnTrail");
            OnPropertyChanged("TotalTimeOnTrailDisplay");
            OnPropertyChanged("TotalCalories");
            OnPropertyChanged("TotalCaloriesDisplay");
        }
    }
}