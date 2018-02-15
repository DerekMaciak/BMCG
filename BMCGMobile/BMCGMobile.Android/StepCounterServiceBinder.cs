// ***********************************************************************
// Assembly         : BMCGMobile.Android
// Author           : Derek Maciak
// Created          : 02-14-2018
//
// Last Modified By : Derek Maciak
// Last Modified On : 02-14-2018
// ***********************************************************************
// <copyright file="StepCounterServiceBinder.cs" company="">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using Android.OS;

namespace BMCGMobile.Droid
{
    /// <summary>
    /// Class StepCounterServiceBinder.
    /// </summary>
    /// <seealso cref="Android.OS.Binder" />
    public class StepCounterServiceBinder : Binder
    {
        /// <summary>
        /// The step counter service
        /// </summary>
        private StepCounterService _StepCounterService;

        /// <summary>
        /// Initializes a new instance of the <see cref="StepCounterServiceBinder"/> class.
        /// </summary>
        /// <param name="service">The service.</param>
        public StepCounterServiceBinder(StepCounterService service)
        {
            this._StepCounterService = service;
        }

        /// <summary>
        /// Gets the step counter service.
        /// </summary>
        /// <value>The step counter service.</value>
        public StepCounterService StepCounterService
        {
            get { return _StepCounterService; }
        }
    }
}