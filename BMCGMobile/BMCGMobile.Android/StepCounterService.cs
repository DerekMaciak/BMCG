// ***********************************************************************
// Assembly         : BMCGMobile.Android
// Author           : Derek Maciak
// Created          : 02-14-2018
//
// Last Modified By : Derek Maciak
// Last Modified On : 02-14-2018
// ***********************************************************************
// <copyright file="StepCounterService.cs" company="">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using Android.App;
using Android.Content;
using Android.Hardware;
using BMCGMobile.Entities;
using System;
using Xamarin.Forms;

namespace BMCGMobile.Droid
{
    /// <summary>
    /// Class StepCounterService.
    /// </summary>
    /// <seealso cref="Android.App.Service" />
    /// <seealso cref="Android.Hardware.ISensorEventListener" />
    /// <seealso cref="BMCGMobile.IStepCounter" />
    [Service(Enabled = true)]
    [IntentFilter(new String[] { "com.refractored.mystepcounter.StepService" })]
    public class StepCounterService : Service, ISensorEventListener, IStepCounter
    {
        /// <summary>
        /// The is running
        /// </summary>
        private bool _IsRunning;
        /// <summary>
        /// The steps today
        /// </summary>
        private Int64 _StepsToday = 0;
        /// <summary>
        /// The new steps
        /// </summary>
        private Int64 _NewSteps = 0;
        /// <summary>
        /// The last steps
        /// </summary>
        private Int64 _LastSteps = 0;
        /// <summary>
        /// The is on trail
        /// </summary>
        private bool _IsOnTrail = false;
        /// <summary>
        /// Gets or sets a value indicating whether [warning state].
        /// </summary>
        /// <value><c>true</c> if [warning state]; otherwise, <c>false</c>.</value>
        public bool _WarningState { get; set; }
        /// <summary>
        /// The binder
        /// </summary>
        private StepCounterServiceBinder _Binder;

        /// <summary>
        /// Initializes a new instance of the <see cref="StepCounterService"/> class.
        /// </summary>
        public StepCounterService()
        {
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
        /// Called by the system every time a client explicitly starts the service by calling
        /// <c><see cref="M:Android.Content.Context.StartService(Android.Content.Intent)" /></c>, providing the arguments it supplied and a
        /// unique integer token representing the start request.
        /// </summary>
        /// <param name="intent">The Intent supplied to <c><see cref="M:Android.Content.Context.StartService(Android.Content.Intent)" /></c>,
        /// as given.  This may be null if the service is being restarted after
        /// its process has gone away, and it had previously returned anything
        /// except <c><see cref="F:Android.App.StartCommandResult.StickyCompatibility" /></c>.</param>
        /// <param name="flags">Additional data about this start request.  Currently either
        /// 0, <c><see cref="F:Android.App.StartCommandFlags.Redelivery" /></c>, or <c><see cref="F:Android.App.StartCommandFlags.Retry" /></c>.</param>
        /// <param name="startId">A unique integer representing this specific request to
        /// start.  Use with <c><see cref="M:Android.App.Service.StopSelfResult(System.Int32)" /></c>.</param>
        /// <returns>To be added.</returns>
        /// <since version="Added in API level 5" />
        /// <altmember cref="M:Android.App.Service.StopSelfResult(System.Int32)" />
        /// <remarks><para tool="javadoc-to-mdoc">Called by the system every time a client explicitly starts the service by calling
        /// <c><see cref="M:Android.Content.Context.StartService(Android.Content.Intent)" /></c>, providing the arguments it supplied and a
        /// unique integer token representing the start request.  Do not call this method directly.
        /// </para>
        /// <para tool="javadoc-to-mdoc">For backwards compatibility, the default implementation calls
        /// <c><see cref="M:Android.App.Service.OnStart(Android.Content.Intent, System.Int32)" /></c> and returns either <c><see cref="F:Android.App.StartCommandResult.Sticky" /></c>
        /// or <c><see cref="F:Android.App.StartCommandResult.StickyCompatibility" /></c>.
        /// </para>
        /// <para tool="javadoc-to-mdoc">If you need your application to run on platform versions prior to API
        /// level 5, you can use the following model to handle the older <c><see cref="M:Android.App.Service.OnStart(Android.Content.Intent, System.Int32)" /></c>
        /// callback in that case.  The <c>handleCommand</c> method is implemented by
        /// you as appropriate:
        /// <example><code lang="java">// This is the old onStart method that will be called on the pre-2.0
        /// // platform.  On 2.0 or later we override onStartCommand() so this
        /// // method will not be called.
        /// @Override
        /// public void onStart(Intent intent, int startId) {
        /// handleCommand(intent);
        /// }
        /// @Override
        /// public int onStartCommand(Intent intent, int flags, int startId) {
        /// handleCommand(intent);
        /// // We want this service to continue running until it is explicitly
        /// // stopped, so return sticky.
        /// return START_STICKY;
        /// }</code></example></para>
        /// <para tool="javadoc-to-mdoc">Note that the system calls this on your
        /// service's main thread.  A service's main thread is the same
        /// thread where UI operations take place for Activities running in the
        /// same process.  You should always avoid stalling the main
        /// thread's event loop.  When doing long-running operations,
        /// network calls, or heavy disk I/O, you should kick off a new
        /// thread, or use <c><see cref="T:Android.OS.AsyncTask`3" /></c>.</para>
        /// <para tool="javadoc-to-mdoc">
        ///   <format type="text/html">
        ///     <a href="http://developer.android.com/reference/android/app/Service.html#onStartCommand(android.content.Intent, int, int)" target="_blank">[Android Documentation]</a>
        ///   </format>
        /// </para></remarks>
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

        /// <summary>
        /// This is called if the service is currently running and the user has
        /// removed a task that comes from the service's application.
        /// </summary>
        /// <param name="rootIntent">The original root Intent that was used to launch
        /// the task that is being removed.</param>
        /// <since version="Added in API level 14" />
        /// <remarks><para tool="javadoc-to-mdoc">This is called if the service is currently running and the user has
        /// removed a task that comes from the service's application.  If you have
        /// set <c><see cref="F:Android.Content.PM.ServiceInfo.FlagStopWithTask" /></c>
        /// then you will not receive this callback; instead, the service will simply
        /// be stopped.</para>
        /// <para tool="javadoc-to-mdoc">
        ///   <format type="text/html">
        ///     <a href="http://developer.android.com/reference/android/app/Service.html#onTaskRemoved(android.content.Intent)" target="_blank">[Android Documentation]</a>
        ///   </format>
        /// </para></remarks>
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

        /// <summary>
        /// Startups the specified warning.
        /// </summary>
        /// <param name="warning">if set to <c>true</c> [warning].</param>
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

        /// <summary>
        /// Called by the system to notify a Service that it is no longer used and is being removed.
        /// </summary>
        /// <since version="Added in API level 1" />
        /// <remarks><para tool="javadoc-to-mdoc">Called by the system to notify a Service that it is no longer used and is being removed.  The
        /// service should clean up any resources it holds (threads, registered
        /// receivers, etc) at this point.  Upon return, there will be no more calls
        /// in to this Service object and it is effectively dead.  Do not call this method directly.
        /// </para>
        /// <para tool="javadoc-to-mdoc">
        ///   <format type="text/html">
        ///     <a href="http://developer.android.com/reference/android/app/Service.html#onDestroy()" target="_blank">[Android Documentation]</a>
        ///   </format>
        /// </para></remarks>
        public override void OnDestroy()
        {
            base.OnDestroy();
            UnregisterListeners();
            _IsRunning = false;
        }

        /// <summary>
        /// Registers the listeners.
        /// </summary>
        /// <param name="sensorType">Type of the sensor.</param>
        private void RegisterListeners(SensorType sensorType)
        {
            var sensorManager = (SensorManager)GetSystemService(Context.SensorService);
            var sensor = sensorManager.GetDefaultSensor(sensorType);

            //get faster why not, nearly fast already and when
            //sensor gets messed up it will be better
            sensorManager.RegisterListener(this, sensor, SensorDelay.Normal);
            Console.WriteLine("Sensor listener registered of type: " + sensorType);
        }

        /// <summary>
        /// Unregisters the listeners.
        /// </summary>
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

        /// <summary>
        /// Return the communication channel to the service.
        /// </summary>
        /// <param name="intent">The Intent that was used to bind to this service,
        /// as given to <c><see cref="M:Android.Content.Context.BindService(Android.Content.Intent, Android.Content.IServiceConnection, Android.Content.IServiceConnection)" /></c>.  Note that any extras that were included with
        /// the Intent at that point will <i>not</i> be seen here.</param>
        /// <returns>To be added.</returns>
        /// <since version="Added in API level 1" />
        /// <remarks><para tool="javadoc-to-mdoc">Return the communication channel to the service.  May return null if
        /// clients can not bind to the service.  The returned
        /// <c><see cref="T:Android.OS.BinderConsts" /></c> is usually for a complex interface
        /// that has been <format type="text/html"><a href="http://developer.android.com/reference/../guide/components/aidl.html">described using
        /// aidl</a></format>.
        /// </para>
        /// <para tool="javadoc-to-mdoc">
        ///   <i>Note that unlike other application components, calls on to the
        /// IBinder interface returned here may not happen on the main thread
        /// of the process</i>.  More information about the main thread can be found in
        /// <format type="text/html"><a href="http://developer.android.com/reference/../guide/topics/fundamentals/processes-and-threads.html">Processes and
        /// Threads</a></format>.</para>
        /// <para tool="javadoc-to-mdoc">
        ///   <format type="text/html">
        ///     <a href="http://developer.android.com/reference/android/app/Service.html#onBind(android.content.Intent)" target="_blank">[Android Documentation]</a>
        ///   </format>
        /// </para></remarks>
        public override Android.OS.IBinder OnBind(Android.Content.Intent intent)
        {
            _Binder = new StepCounterServiceBinder(this);
            return _Binder;
        }

        /// <summary>
        /// Called when the accuracy of a sensor has changed.
        /// </summary>
        /// <param name="sensor">To be added.</param>
        /// <param name="accuracy">The new accuracy of this sensor</param>
        /// <since version="Added in API level 3" />
        /// <remarks><para tool="javadoc-to-mdoc">Called when the accuracy of a sensor has changed.
        /// </para>
        /// <para tool="javadoc-to-mdoc">See <c><see cref="T:Android.Hardware.SensorManager" /></c>
        /// for details.</para>
        /// <para tool="javadoc-to-mdoc">
        ///   <format type="text/html">
        ///     <a href="http://developer.android.com/reference/android/hardware/SensorEventListener.html#onAccuracyChanged(android.hardware.Sensor, int)" target="_blank">[Android Documentation]</a>
        ///   </format>
        /// </para></remarks>
        public void OnAccuracyChanged(Sensor sensor, SensorStatus accuracy)
        {
            //do nothing here
        }

        /// <summary>
        /// Called when sensor values have changed.
        /// </summary>
        /// <param name="e">the <c><see cref="T:Android.Hardware.SensorEvent" /></c>.</param>
        /// <since version="Added in API level 3" />
        /// <remarks><para tool="javadoc-to-mdoc">Called when sensor values have changed.
        /// </para>
        /// <para tool="javadoc-to-mdoc">See <c><see cref="T:Android.Hardware.SensorManager" /></c>
        /// for details on possible sensor types.
        /// </para>
        /// <para tool="javadoc-to-mdoc">See also <c><see cref="T:Android.Hardware.SensorEvent" /></c>.
        /// </para>
        /// <para tool="javadoc-to-mdoc">
        ///   <format type="text/html">
        ///     <b>NOTE:</b>
        ///   </format> The application doesn't own the
        /// <c><see cref="T:Android.Hardware.SensorEvent" /></c>
        /// object passed as a parameter and therefore cannot hold on to it.
        /// The object may be part of an internal pool and may be reused by
        /// the framework.</para>
        /// <para tool="javadoc-to-mdoc">
        ///   <format type="text/html">
        ///     <a href="http://developer.android.com/reference/android/hardware/SensorEventListener.html#onSensorChanged(android.hardware.SensorEvent)" target="_blank">[Android Documentation]</a>
        ///   </format>
        /// </para></remarks>
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

        /// <summary>
        /// Adds the steps.
        /// </summary>
        /// <param name="count">The count.</param>
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

        /// <summary>
        /// Starts the step updates.
        /// </summary>
        public void StartStepUpdates()
        {
            _IsOnTrail = true;
        }

        /// <summary>
        /// Stops the step updates.
        /// </summary>
        public void StopStepUpdates()
        {
            _IsOnTrail = false;
        }
    }
}