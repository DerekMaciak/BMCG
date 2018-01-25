using BMCGMobile.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace BMCGMobile
{
    public partial class App : Application
    {
        public static double ScreenHeight;
        public static double ScreenWidth;
        public static string CurrentLanguage = "EN";

        public App()
        {
            InitializeComponent();

            MainPage = new BMCGMobile.MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
