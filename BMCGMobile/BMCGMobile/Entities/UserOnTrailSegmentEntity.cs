// ***********************************************************************
// Assembly         : BMCGMobile
// Author           : Derek Maciak
// Created          : 02-11-2018
//
// Last Modified By : Derek Maciak
// Last Modified On : 02-11-2018
// ***********************************************************************
// <copyright file="UserOnTrailSegmentEntity.cs" company="">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using Plugin.Geolocator.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BMCGMobile.Entities
{
    /// <summary>
    /// Class UserOnTrailSegmentEntity.
    /// </summary>
    /// <seealso cref="BMCGMobile.Entities.EntityBase" />
    public class UserOnTrailSegmentEntity : EntityBase
    {
        /// <summary>
        /// The segment start time stamp
        /// </summary>
        private DateTime _SegmentStartTimeStamp;
        /// <summary>
        /// Gets the segment start time stamp.
        /// </summary>
        /// <value>The segment start time stamp.</value>
        public DateTime SegmentStartTimeStamp { set { _SegmentStartTimeStamp = value; } get { return _SegmentStartTimeStamp; } }

        /// <summary>
        /// The segment end time stamp
        /// </summary>
        private DateTime _SegmentEndTimeStamp;
        /// <summary>
        /// Gets the segment end time stamp.
        /// </summary>
        /// <value>The segment end time stamp.</value>
        public DateTime SegmentEndTimeStamp { set { _SegmentEndTimeStamp = value; } get { return _SegmentEndTimeStamp; } }

        /// <summary>
        /// The user positions on trail
        /// </summary>
        public List<Position> UserPositionsOnTrail = new List<Position>();

        //private double _TotalSegmentDistanceTraveled;
        //public double TotalSegmentDistanceTraveled { get { return _TotalSegmentDistanceTraveled; } }

        /// <summary>
        /// The total segment time span
        /// </summary>
        private TimeSpan _TotalSegmentTimeSpan;
        /// <summary>
        /// Gets the total segment time span.
        /// </summary>
        /// <value>The total segment time span.</value>
        public TimeSpan TotalSegmentTimeSpan { set { _TotalSegmentTimeSpan = value; } get { return _TotalSegmentTimeSpan; } }

        //private double _TotalSegmentSpeed;
        //public double AverageSegmentSpeed { get { return (UserPositionsOnTrail.Count > 0) ? _TotalSegmentSpeed / UserPositionsOnTrail.Count : 0; } }

        /// <summary>
        /// Gets or sets the total segment step count.
        /// </summary>
        /// <value>The total segment step count.</value>
        public int TotalSegmentStepCount { set; get; }
        /// <summary>
        /// Gets or sets the total segment distance by steps.
        /// </summary>
        /// <value>The total segment distance by steps.</value>
        public double TotalSegmentDistanceBySteps { set; get; }
       
        /// <summary>
        /// Initializes a new instance of the <see cref="UserOnTrailSegmentEntity"/> class.
        /// </summary>
        public UserOnTrailSegmentEntity()
        {
          
        }

        public UserOnTrailSegmentEntity(bool SetSegmentStartTimeStamp)
        {
            if (SetSegmentStartTimeStamp)
            {
                _SegmentStartTimeStamp = DateTime.Now;
            }
          
        }

        /// <summary>
        /// Adds the user position.
        /// </summary>
        /// <param name="position">The position.</param>
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