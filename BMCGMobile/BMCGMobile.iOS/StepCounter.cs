// ***********************************************************************
// Assembly         : BMCGMobile.iOS
// Author           : Derek Maciak
// Created          : 02-11-2018
//
// Last Modified By : Derek Maciak
// Last Modified On : 02-14-2018
// ***********************************************************************
// <copyright file="StepCounter.cs" company="">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using BMCGMobile.Entities;
using CoreFoundation;
using CoreMotion;
using Foundation;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BMCGMobile.iOS
{
    /// <summary>
    /// Class StepCounter.
    /// </summary>
    /// <seealso cref="BMCGMobile.IStepCounter" />
    public class StepCounter : IStepCounter
    {
        /// <summary>
        /// The step counter
        /// </summary>
        private CMPedometer _StepCounter;

        /// <summary>
        /// Initializes a new instance of the <see cref="StepCounter"/> class.
        /// </summary>
        public StepCounter()
        {
            _StepCounter = new CMPedometer();

            CheckAuthorizationAsync();

            MessagingCenter.Subscribe<TrackingEntity>(this, "StartStepUpdates", (sender) =>
            {
                StartStepUpdates();
            });

            MessagingCenter.Subscribe<TrackingEntity>(this, "StopStepUpdates", (sender) =>
            {
                StopStepUpdates();
            });
        }

        /// <summary>
        /// get step count as an asynchronous operation.
        /// </summary>
        /// <param name="nsStartDate">The ns start date.</param>
        /// <returns>Task&lt;System.Int32&gt;.</returns>
        private async Task<int> GetStepCountAsync(NSDate nsStartDate)
        {
            int stepCount = 0;
            if (CMPedometer.IsStepCountingAvailable && CMPedometer.AuthorizationStatus == CMAuthorizationStatus.Authorized)
            {
                var nsEndDate = NSDate.Now;

                try
                {
                    var pedometerData = await _StepCounter.QueryPedometerDataAsync(nsStartDate, nsEndDate);

                    stepCount = pedometerData.NumberOfSteps.Int32Value;
                    var distance = pedometerData.Distance;
                    MessagingCenter.Send<IStepCounter, int>(this, "StepCount", stepCount);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error, unable to retrieve step counts for range {0}, {1}", nsStartDate.SecondsSinceReferenceDate, nsEndDate.SecondsSinceReferenceDate);
                    Console.WriteLine(ex.Message);
                }
            }
            return stepCount;
        }

        /// <summary>
        /// Starts the step updates.
        /// </summary>
        public void StartStepUpdates()
        {
            if (CMPedometer.IsStepCountingAvailable && CMPedometer.AuthorizationStatus == CMAuthorizationStatus.Authorized)
            {
                _StepCounter.StartPedometerUpdates(NSDate.Now, ((pedometerData, error) =>
                {
                    DispatchQueue.MainQueue.DispatchAsync(() =>
                    {
                        MessagingCenter.Send<IStepCounter, int>(this, "StepCount", pedometerData.NumberOfSteps.Int32Value);

                        if (CMPedometer.IsDistanceAvailable)
                        {
                            //Meters converted to Miles
                            MessagingCenter.Send<IStepCounter, double>(this, "Distance", (pedometerData.Distance.DoubleValue * 0.000621371));
                        }
                        else
                        {
                            MessagingCenter.Send<IStepCounter, double>(this, "Distance", StaticHelpers.ConvertStepsToMiles(pedometerData.NumberOfSteps.Int32Value));
                        }

                        //if (CMPedometer.IsCadenceAvailable)
                        //{
                        //    // Steps per second
                        //    MessagingCenter.Send<IStepCounter, double>(this, "Cadence", (pedometerData.CurrentCadence.DoubleValue));
                        //}

                        //if (CMPedometer.IsPaceAvailable)
                        //{
                        //    // Seconds per Meter converted to Miles per Hour
                        //    MessagingCenter.Send<IStepCounter, double>(this, "Pace", (pedometerData.CurrentPace.DoubleValue * 2.23694));
                        //}
                    });
                }));
            }
        }

        /// <summary>
        /// Stops the step updates.
        /// </summary>
        public void StopStepUpdates()
        {
            _StepCounter.StopPedometerUpdates();
        }

        /// <summary>
        /// check authorization as an asynchronous operation.
        /// </summary>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public async Task<bool> CheckAuthorizationAsync()
        {
            NSDate now = NSDate.Now;

            try
            {
                await _StepCounter.QueryPedometerDataAsync(now, now);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}