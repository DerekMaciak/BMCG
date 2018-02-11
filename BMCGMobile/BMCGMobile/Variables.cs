using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMCGMobile
{
    public static class Variables
    {
        // https://developers.google.com/maps/documentation/android-api/signup
        public const string GOOGLE_MAPS_ANDROID_API_KEY = "AIzaSyBTVfimWYh0FIf9cTHGHLX1itKD-hFz2Bs";

        // https://developers.google.com/maps/documentation/ios-sdk/start#step_4_get_an_api_key
        public const string GOOGLE_MAPS_IOS_API_KEY = "AIzaSyD_vdpTPaJFl94sJ01azQl84zojEXGL8sY";

        // https://msdn.microsoft.com/windows/uwp/maps-and-location/authentication-key
        public const string BING_MAPS_UWP_API_KEY = "your_bing_maps_apikey";

        //public const string GPX_URL = @"http://services.surroundtech.com/BMCGWebsite/downloads/BMCGCoordinates.txt";
        public const string GPX_URL = @"http://services.surroundtech.com/BMCGWebsite/downloads/DerekHomeTest.txt";

        public const string CMS_WEBSITE_URL = @"http://services.surroundtech.com/BMCGWebsite/Home/";

        public const bool DEBUG_MODE = true;

        public const double DISPLAY_AS_FEET_MIN = 500;

        public const double DISPLAY_STATUS_INFO_MIN_DIST = .25; // Display Within quarter Mile of Trail

        public const double AVE_WALKING_SPEED = 3.1; // 3.1 Miles per hour
    }
}