using Plugin.Geolocator.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BMCGMobile.Entities
{
    public class UserOnTrailSegmentEntity : EntityBase
    {
        private DateTime _SegmentStartTimeStamp;
        public DateTime SegmentStartTimeStamp { get { return _SegmentStartTimeStamp; } }

        private DateTime _SegmentEndTimeStamp;
        public DateTime SegmentEndTimeStamp { get { return _SegmentEndTimeStamp; } }

        public List<Position> UserPositionsOnTrail = new List<Position>();

        //private double _TotalSegmentDistanceTraveled;
        //public double TotalSegmentDistanceTraveled { get { return _TotalSegmentDistanceTraveled; } }

        private TimeSpan _TotalSegmentTimeSpan;
        public TimeSpan TotalSegmentTimeSpan { get { return _TotalSegmentTimeSpan; } }

        //private double _TotalSegmentSpeed;
        //public double AverageSegmentSpeed { get { return (UserPositionsOnTrail.Count > 0) ? _TotalSegmentSpeed / UserPositionsOnTrail.Count : 0; } }

        public int TotalSegmentStepCount { set; get; }
        public double TotalSegmentDistanceBySteps { set; get; }
        public double TotalSegmentCaloriesByDistance { get { return StaticHelpers.CaloriesBurnt(TotalSegmentDistanceBySteps); } }
        public UserOnTrailSegmentEntity()
        {
            _SegmentStartTimeStamp = DateTime.Now;
        }

        public void AddUserPosition(Position position)
        {
            if (UserPositionsOnTrail.Count > 0)
            {
                //Get Last Position
                var last = UserPositionsOnTrail.Last();

                //_TotalSegmentDistanceTraveled = _TotalSegmentDistanceTraveled + last.CalculateDistance(position);
                _TotalSegmentTimeSpan = _TotalSegmentTimeSpan + (position.Timestamp - last.Timestamp);
            }

            //_TotalSegmentSpeed = _TotalSegmentSpeed + position.Speed;

            UserPositionsOnTrail.Add(position);

            _SegmentEndTimeStamp = DateTime.Now;
        }
    }
}