using System;
using System.Collections.Generic;
using System.Linq;
using HockeyApp.iOS;
using Foundation;
using UIKit;

namespace BMCGMobile.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            var manager = BITHockeyManager.SharedHockeyManager;
            manager.Configure("4d7260837e894ed2bc5ae5851c1b325");
            manager.StartManager();
           // manager.Authenticator.AuthenticateInstallation(); // This line is obsolete in crash only builds



            global::Xamarin.Forms.Forms.Init();
            Xamarin.FormsGoogleMaps.Init(Variables.GOOGLE_MAPS_IOS_API_KEY);

            App.ScreenWidth = UIScreen.MainScreen.Bounds.Width;
            App.ScreenHeight = UIScreen.MainScreen.Bounds.Height;

            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
    }
}
