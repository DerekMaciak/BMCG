using Android.App;
using Android.Content;
using Android.Hardware;
using BMCGMobile.Entities;
using System;
using Xamarin.Forms;

namespace BMCGMobile.Droid
{
    [Service(Enabled = true)]
    [IntentFilter(new String[] { "com.refractored.mystepcounter.StepService" })]
    public class StepCounterService : Service, ISensorEventListener, IStepCounter
    {
        private bool _IsRunning;
        private Int64 _StepsToday = 0;
        private Int64 _NewSteps = 0;
        private Int64 _LastSteps = 0;

      
        private Int64 _HeightInInches = 72;
    
        private bool _IsOnTrail = false;
        public bool _WarningState { get; set; }
        private StepCounterServiceBinder _Binder;

        public StepCounterService()
        {
            MessagingCenter.Subscribe<TrackingEntity>(this, "StartStepUpdates", (sender) => {
                StartStepUpdates();
            });

            MessagingCenter.Subscribe<TrackingEntity>(this, "StopStepUpdates", (sender) => {
                StopStepUpdates();
            });
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            Console.WriteLine("StartCommand Called, setting alarm");
#if DEBUG
            Android.Util.Log.Debug("STEPSERVICE", "Start command result called, incoming startup");
#endif

            var alarmManager = ((AlarmManager)ApplicationContext.GetSystemService(Context.AlarmService));
            var intent2 = new Intent(this, typeof(StepCounterService));
            intent2.PutExtra("warning", _WarningState);
            var stepIntent = PendingIntent.GetService(ApplicationContext, 10, intent2, PendingIntentFlags.UpdateCurrent);
            // Workaround as on Android 4.4.2 START_STICKY has currently no
            // effect
            // -> restart service every 60 mins
            alarmManager.Set(AlarmType.Rtc, Java.Lang.JavaSystem
                .CurrentTimeMillis() + 1000 * 60 * 60, stepIntent);

            var warning = false;
            if (intent != null)
                warning = intent.GetBooleanExtra("warning", false);
            Startup();

            return StartCommandResult.Sticky;
        }

        public override void OnTaskRemoved(Intent rootIntent)
        {
            base.OnTaskRemoved(rootIntent);

            UnregisterListeners();
#if DEBUG
            Console.WriteLine("OnTaskRemoved Called, setting alarm for 500 ms");
            Android.Util.Log.Debug("STEPSERVICE", "Task Removed, going down");
#endif
            var intent = new Intent(this, typeof(StepCounterService));
            intent.PutExtra("warning", _WarningState);
            // Restart service in 500 ms
            ((AlarmManager)GetSystemService(Context.AlarmService)).Set(AlarmType.Rtc, Java.Lang.JavaSystem
                .CurrentTimeMillis() + 500,
                PendingIntent.GetService(this, 11, intent, 0));
        }

        private void Startup(bool warning = false)
        {
            ////check if kit kat can sensor compatible
            //if (!Utils.IsKitKatWithStepCounter(PackageManager))
            //{
            //    Console.WriteLine("Not compatible with sensors, stopping service.");
            //    StopSelf();
            //    return;
            //}

            if (!_IsRunning)
            {
                RegisterListeners(warning ? SensorType.StepDetector : SensorType.StepCounter);
                _WarningState = warning;
                
            }

            _IsRunning = true;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            UnregisterListeners();
            _IsRunning = false;
        }

        private void RegisterListeners(SensorType sensorType)
        {
            var sensorManager = (SensorManager)GetSystemService(Context.SensorService);
            var sensor = sensorManager.GetDefaultSensor(sensorType);

            //get faster why not, nearly fast already and when
            //sensor gets messed up it will be better
            sensorManager.RegisterListener(this, sensor, SensorDelay.Normal);
            Console.WriteLine("Sensor listener registered of type: " + sensorType);
        }

        private void UnregisterListeners()
        {
            if (!_IsRunning)
                return;

            try
            {
                var sensorManager = (SensorManager)GetSystemService(Context.SensorService);
                sensorManager.UnregisterListener(this);
                Console.WriteLine("Sensor listener unregistered.");
#if DEBUG
                Android.Util.Log.Debug("STEPSERVICE", "Sensor listener unregistered.");
#endif
                _IsRunning = false;
            }
            catch (Exception ex)
            {
#if DEBUG
                Android.Util.Log.Debug("STEPSERVICE", "Unable to unregister: " + ex);
#endif
            }
        }

  

        public override Android.OS.IBinder OnBind(Android.Content.Intent intent)
        {
            _Binder = new StepCounterServiceBinder(this);
            return _Binder;
        }

        public void OnAccuracyChanged(Sensor sensor, SensorStatus accuracy)
        {
            //do nothing here
        }

    

        public void OnSensorChanged(SensorEvent e)
        {
            switch (e.Sensor.Type)
            {
                case SensorType.StepCounter:

                    if (_LastSteps < 0)
                        _LastSteps = 0;

                    //grab out the current value.
                    var count = (Int64)e.Values[0];
                    //in some instances if things are running too long (about 4 days)
                    //the value flips and gets crazy and this will be -1
                    //so switch to step detector instead, but put up warning sign.
                    if (count < 0)
                    {
                        UnregisterListeners();
                        RegisterListeners(SensorType.StepDetector);
                        _IsRunning = true;
#if DEBUG
                        Android.Util.Log.Debug("STEPSERVICE", "Something has gone wrong with the step counter, simulating steps, 2.");
#endif
                        count = _LastSteps + 3;

                        _WarningState = true;
                    }
                    else
                    {
                        _WarningState = false;
                    }

                    AddSteps(count);

                    break;

                case SensorType.StepDetector:
                    count = _LastSteps + 1;
                    AddSteps(count);
                    break;
            }
        }

        public void AddSteps(Int64 count)
        {
            //if service rebooted or rebound then this will null out to 0, but count will still be since last boot.
            if (_LastSteps == 0)
            {
                _LastSteps = count;
            }

            //calculate new steps
            _NewSteps = count - _LastSteps;

            //ensure we are never negative
            //if so, no worries as we are about to re-set the lastSteps to the
            //current count
            if (_NewSteps < 0)
                _NewSteps = 1;
            else if (_NewSteps > 100)
                _NewSteps = 1;

            _LastSteps = count;

            //save total steps!
            if (_IsOnTrail)
            {
                _StepsToday += _NewSteps;
                MessagingCenter.Send<IStepCounter, int>(this, "StepCount", (int)_StepsToday);
                MessagingCenter.Send<IStepCounter, double>(this, "Distance", StaticHelpers.ConvertStepsToMiles((int)_StepsToday));
            }
          
           
            Console.WriteLine("New step detected by STEP_COUNTER sensor. Total step count: " + _StepsToday);
#if DEBUG
            Android.Util.Log.Debug("STEPSERVICE", "New steps: " + _NewSteps + " total: " + _StepsToday);
#endif
        }

        public void StartStepUpdates()
        {
            _IsOnTrail = true;
        }

        public void StopStepUpdates()
        {
            _IsOnTrail = false;
        }
    }
}