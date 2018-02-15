// ***********************************************************************
// Assembly         : BMCGMobile
// Author           : Derek Maciak
// Created          : 12-14-2017
//
// Last Modified By : Derek Maciak
// Last Modified On : 12-14-2017
// ***********************************************************************
// <copyright file="ICompass.cs" company="">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace BMCGMobile
{
    /// <summary>
    /// Interface ICompass
    /// </summary>
    public interface ICompass
    {
        /// <summary>
        /// Gets the heading.
        /// </summary>
        /// <returns>System.Double.</returns>
        double GetHeading();

        /// <summary>
        /// Compasses the start.
        /// </summary>
        void CompassStart();

        /// <summary>
        /// Compasses the stop.
        /// </summary>
        void CompassStop();
    }
}