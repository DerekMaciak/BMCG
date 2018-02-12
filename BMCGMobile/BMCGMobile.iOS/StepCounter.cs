using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMCGMobile.Entities;
using CoreFoundation;
using CoreMotion;
using Foundation;
using UIKit;
using Xamarin.Forms;

namespace BMCGMobile.iOS
{
    public class StepCounter : IStepCounter
    {
       
        private CMPedometer _StepCounter;

        public StepCounter()
        {
            _StepCounter = new CMPedometer();

            CheckAuthorizationAsync();

            MessagingCenter.Subscribe<TrackingEntity>(this, "StartStepUpdates", (sender) => {
                StartStepUpdates();
            });

            MessagingCenter.Subscribe<TrackingEntity>(this, "StopStepUpdates", (sender) => {
                StopStepUpdates();
            });
        }
        
        async Task<int> GetStepCountAsync(NSDate nsStartDate)
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

        public void StopStepUpdates()
        {
            _StepCounter.StopPedometerUpdates();
        }

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