// ***********************************************************************
// Assembly         : BMCGMobile
// Author           : Derek Maciak
// Created          : 12-07-2017
//
// Last Modified By : Derek Maciak
// Last Modified On : 02-14-2018
// ***********************************************************************
// <copyright file="Variables.cs" company="">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace BMCGMobile
{
    /// <summary>
    /// Class Variables.
    /// </summary>
    public static class Variables
    {
        // https://developers.google.com/maps/documentation/android-api/signup
        /// <summary>
        /// The google maps android API key
        /// </summary>
        public const string GOOGLE_MAPS_ANDROID_API_KEY = "AIzaSyBTVfimWYh0FIf9cTHGHLX1itKD-hFz2Bs";

        // https://developers.google.com/maps/documentation/ios-sdk/start#step_4_get_an_api_key
        /// <summary>
        /// The google maps ios API key
        /// </summary>
        public const string GOOGLE_MAPS_IOS_API_KEY = "AIzaSyD_vdpTPaJFl94sJ01azQl84zojEXGL8sY";

        // https://msdn.microsoft.com/windows/uwp/maps-and-location/authentication-key
        /// <summary>
        /// The bing maps uwp API key
        /// </summary>
        public const string BING_MAPS_UWP_API_KEY = "your_bing_maps_apikey";

        /// <summary>
        /// The GPX URL
        /// </summary>
        //public const string GPX_URL = @"http://morriscanalbloomfield.org/MorrisCanalBloomfield.gpx.txt";
        public const string GPX_URL = @"http://morriscanalbloomfield.org/DerekHomeTest.gpx.txt";

        /// <summary>
        /// The CMS website URL
        /// </summary>
        public const string CMS_WEBSITE_URL = @"http://morriscanalbloomfield.org/";

        /// <summary>
        /// The debug mode
        /// </summary>
        public const bool DEBUG_MODE = true;

        /// <summary>
        /// The display as feet minimum
        /// </summary>
        public const double DISPLAY_AS_FEET_MIN = 500;

        /// <summary>
        /// The display status information minimum dist
        /// </summary>
        public const double DISPLAY_STATUS_INFO_MIN_DIST = .25; // Display Within quarter Mile of Trail

        /// <summary>
        /// The ave stride multiple
        /// </summary>
        public const double AVE_STRIDE_MULTIPLE = 0.413;
    }
}