// ***********************************************************************
// Assembly         : BMCGMobile
// Author           : Derek Maciak
// Created          : 01-22-2018
//
// Last Modified By : Derek Maciak
// Last Modified On : 01-22-2018
// ***********************************************************************
// <copyright file="CultureChangedMessage.cs" company="">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Globalization;

namespace BMCGMobile.Localization
{
    /// <summary>
    /// Class CultureChangedMessage.
    /// </summary>
    public class CultureChangedMessage
    {
        /// <summary>
        /// Gets the new culture information.
        /// </summary>
        /// <value>The new culture information.</value>
        public CultureInfo NewCultureInfo { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CultureChangedMessage"/> class.
        /// </summary>
        /// <param name="lngName">Name of the LNG.</param>
        public CultureChangedMessage(string lngName)
            : this(new CultureInfo(lngName))
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CultureChangedMessage"/> class.
        /// </summary>
        /// <param name="newCultureInfo">The new culture information.</param>
        public CultureChangedMessage(CultureInfo newCultureInfo)
        {
            NewCultureInfo = newCultureInfo;
        }
    }
}