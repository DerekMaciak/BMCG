// ***********************************************************************
// Assembly         : BMCGMobile
// Author           : Derek Maciak
// Created          : 02-11-2018
//
// Last Modified By : Derek Maciak
// Last Modified On : 02-11-2018
// ***********************************************************************
// <copyright file="IStepCounter.cs" company="">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace BMCGMobile
{
    /// <summary>
    /// Interface IStepCounter
    /// </summary>
    public interface IStepCounter
    {
        /// <summary>
        /// Starts the step updates.
        /// </summary>
        void StartStepUpdates();

        /// <summary>
        /// Stops the step updates.
        /// </summary>
        void StopStepUpdates();
    }
}