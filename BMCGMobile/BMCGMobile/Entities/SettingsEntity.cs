// ***********************************************************************
// Assembly         : BMCGMobile
// Author           : Derek Maciak
// Created          : 01-22-2018
//
// Last Modified By : Derek Maciak
// Last Modified On : 02-14-2018
// ***********************************************************************
// <copyright file="SettingsEntity.cs" company="">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace BMCGMobile.Entities
{
    /// <summary>
    /// Class SettingsEntity.
    /// </summary>
    /// <seealso cref="BMCGMobile.Entities.EntityBase" />
    public class SettingsEntity : EntityBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsEntity"/> class.
        /// </summary>
        public SettingsEntity()
        {
            AutoTrackingMaximumDistanceFromTrailInFeet = 50; // 50 Feet
            IsAutoTracking = true;
            HeightInInches = 70;
        }

        /// <summary>
        /// Gets or sets the automatic tracking maximum distance from trail in feet.
        /// </summary>
        /// <value>The automatic tracking maximum distance from trail in feet.</value>
        public double AutoTrackingMaximumDistanceFromTrailInFeet { set; get; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is automatic tracking.
        /// </summary>
        /// <value><c>true</c> if this instance is automatic tracking; otherwise, <c>false</c>.</value>
        public bool IsAutoTracking { set; get; }

        /// <summary>
        /// Gets or sets the height in inches.
        /// </summary>
        /// <value>The height in inches.</value>
        public int HeightInInches { set; get; }
    }
}