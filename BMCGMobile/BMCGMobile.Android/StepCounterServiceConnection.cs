// ***********************************************************************
// Assembly         : BMCGMobile.Android
// Author           : Derek Maciak
// Created          : 02-14-2018
//
// Last Modified By : Derek Maciak
// Last Modified On : 02-14-2018
// ***********************************************************************
// <copyright file="StepCounterServiceConnection.cs" company="">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using Android.Content;
using Android.OS;

namespace BMCGMobile.Droid
{
    /// <summary>
    /// Class StepCounterServiceConnection.
    /// </summary>
    /// <seealso cref="Java.Lang.Object" />
    /// <seealso cref="Android.Content.IServiceConnection" />
    public class StepCounterServiceConnection : Java.Lang.Object, IServiceConnection
    {
        /// <summary>
        /// The activity
        /// </summary>
        private MainActivity _Activity;

        /// <summary>
        /// Initializes a new instance of the <see cref="StepCounterServiceConnection"/> class.
        /// </summary>
        /// <param name="activity">The activity.</param>
        public StepCounterServiceConnection(MainActivity activity)
        {
            this._Activity = activity;
        }

        /// <summary>
        /// Called when a connection to the Service has been established, with
        /// the <c><see cref="T:Android.OS.BinderConsts" /></c> of the communication channel to the
        /// Service.
        /// </summary>
        /// <param name="name">The concrete component name of the service that has
        /// been connected.</param>
        /// <param name="service">The IBinder of the Service's communication channel,
        /// which you can now make calls on.</param>
        /// <since version="Added in API level 1" />
        /// <remarks><para tool="javadoc-to-mdoc">Called when a connection to the Service has been established, with
        /// the <c><see cref="T:Android.OS.BinderConsts" /></c> of the communication channel to the
        /// Service.</para>
        /// <para tool="javadoc-to-mdoc">
        ///   <format type="text/html">
        ///     <a href="http://developer.android.com/reference/android/content/ServiceConnection.html#onServiceConnected(android.content.ComponentName, android.os.IBinder)" target="_blank">[Android Documentation]</a>
        ///   </format>
        /// </para></remarks>
        public void OnServiceConnected(ComponentName name, IBinder service)
        {
            var serviceBinder = service as StepCounterServiceBinder;
            if (serviceBinder != null)
            {
                _Activity.Binder = serviceBinder;
                _Activity.IsBound = true;
            }
        }

        /// <summary>
        /// Called when a connection to the Service has been lost.
        /// </summary>
        /// <param name="name">The concrete component name of the service whose
        /// connection has been lost.</param>
        /// <since version="Added in API level 1" />
        /// <remarks><para tool="javadoc-to-mdoc">Called when a connection to the Service has been lost.  This typically
        /// happens when the process hosting the service has crashed or been killed.
        /// This does <i>not</i> remove the ServiceConnection itself -- this
        /// binding to the service will remain active, and you will receive a call
        /// to <c><see cref="M:Android.Content.IServiceConnection.OnServiceConnected(Android.Content.ComponentName, Android.OS.IBinder)" /></c> when the Service is next running.</para>
        /// <para tool="javadoc-to-mdoc">
        ///   <format type="text/html">
        ///     <a href="http://developer.android.com/reference/android/content/ServiceConnection.html#onServiceDisconnected(android.content.ComponentName)" target="_blank">[Android Documentation]</a>
        ///   </format>
        /// </para></remarks>
        public void OnServiceDisconnected(ComponentName name)
        {
            _Activity.IsBound = false;
        }
    }
}