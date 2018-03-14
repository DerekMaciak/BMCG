// ***********************************************************************
// Assembly         : BMCGMobile
// Author           : Derek Maciak
// Created          : 01-25-2018
//
// Last Modified By : Derek Maciak
// Last Modified On : 01-25-2018
// ***********************************************************************
// <copyright file="Enums.cs" company="">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace BMCGMobile
{
    /// <summary>
    /// Class Enums.
    /// </summary>
    public class Enums
    {
        /// <summary>
        /// Enum Units
        /// </summary>
        public enum Units
        {
            /// <summary>
            /// The miles
            /// </summary>
            Miles,
            /// <summary>
            /// The kilometers
            /// </summary>
            Kilometers,
            /// <summary>
            /// The feet
            /// </summary>
            Feet,
            /// <summary>
            /// The yards
            /// </summary>
            Yards
        }

        /// <summary>
        /// Enum MapZooms
        /// </summary>
        public enum MapZooms
        {
            /// <summary>
            /// All
            /// </summary>
            All,
            /// <summary>
            /// The pins
            /// </summary>
            Pins,
            /// <summary>
            /// The street
            /// </summary>
            Street,
            /// <summary>
            /// The user
            /// </summary>
            User,
            /// <summary>
            /// The none
            /// </summary>
            None
        }

        /// <summary>
        /// Enum TrackTypes
        /// </summary>
        public enum TrackTypes
        {
            BloomfieldGreenwayTrail,
            MorrisCanal
        }

        /// <summary>
        /// Enum SavedDataTypes
        /// </summary>
        public enum SavedDataTypes
        {
            UserSettings,
            FitnessHistory
        }

        public enum Direction
        {
            N = 0,
            NE = 1,
            E = 2,
            SE = 3,
            S = 4,
            SW = 5,
            W = 6,
            NW = 7
        }

    }
}