// ***********************************************************************
// Assembly         : BMCGMobile
// Author           : Derek Maciak
// Created          : 10-09-2017
//
// Last Modified By : Derek Maciak
// Last Modified On : 01-22-2018
// ***********************************************************************
// <copyright file="App.xaml.cs" company="">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using Xamarin.Forms;

namespace BMCGMobile
{
    /// <summary>
    /// Class App.
    /// </summary>
    /// <seealso cref="Xamarin.Forms.Application" />
    public partial class App : Application
    {
        /// <summary>
        /// The screen height
        /// </summary>
        public static double ScreenHeight;
        /// <summary>
        /// The screen width
        /// </summary>
        public static double ScreenWidth;
        /// <summary>
        /// The current language
        /// </summary>
        public static string CurrentLanguage = "EN";

        /// <summary>
        /// Initializes a new instance of the <see cref="App"/> class.
        /// </summary>
        public App()
        {
            InitializeComponent();

            MainPage = new BMCGMobile.MainPage();
        }

        /// <summary>
        /// Application developers override this method to perform actions when the application starts.
        /// </summary>
        /// <remarks>To be added.</remarks>
        protected override void OnStart()
        {
            // Handle when your app starts
        }

        /// <summary>
        /// Application developers override this method to perform actions when the application enters the sleeping state.
        /// </summary>
        /// <remarks>To be added.</remarks>
        protected override void OnSleep()
        {
            // Handle when your app sleeps

            StaticData.TrackingData.SaveProperties();
        }

        /// <summary>
        /// Application developers override this method to perform actions when the application resumes from a sleeping state.
        /// </summary>
        /// <remarks>To be added.</remarks>
        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}