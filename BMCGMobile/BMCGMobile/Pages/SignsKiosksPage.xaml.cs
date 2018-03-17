// ***********************************************************************
// Assembly         : BMCGMobile
// Author           : Derek Maciak
// Created          : 01-23-2018
//
// Last Modified By : Derek Maciak
// Last Modified On : 02-14-2018
// ***********************************************************************
// <copyright file="SignsKiosksPage.xaml.cs" company="">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using BMCGMobile.Entities;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BMCGMobile
{
    /// <summary>
    /// Class SignsKiosksPage.
    /// </summary>
    /// <seealso cref="Xamarin.Forms.ContentPage" />
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignsKiosksPage : ContentPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SignsKiosksPage"/> class.
        /// </summary>
        public SignsKiosksPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// When overridden, allows application developers to customize behavior immediately prior to the <see cref="T:Xamarin.Forms.Page" /> becoming visible.
        /// </summary>
        /// <remarks>To be added.</remarks>
        protected override void OnAppearing()
        {
            base.OnAppearing();

            listViewPins.ItemsSource = StaticData.CustomPins;

            StaticData.TrackingData.PropertyChanged -= TrackingData_PropertyChanged;
            StaticData.TrackingData.PropertyChanged += TrackingData_PropertyChanged;
        }

        /// <summary>
        /// Handles the PropertyChanged event of the TrackingData control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        private void TrackingData_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "NextPin")
            {
                // Need to rebind so the height of cell is adjusted
                listViewPins.ItemsSource = null;
                listViewPins.ItemsSource = StaticData.CustomPins;

                // Move Selected pin to center
                var selectedPin = StaticData.CustomPins.Where(s => s.IsStatusInfoVisible).FirstOrDefault();
                if (selectedPin != null)
                {
                    listViewPins.ScrollTo(selectedPin, ScrollToPosition.MakeVisible, true);
                }
            }
        }

        /// <summary>
        /// Handles the Clicked event of the Button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void Button_Clicked(object sender, EventArgs e)
        {
            var customerPin = (sender as Button).CommandParameter as CustomPinEntity;

            var destAddr = customerPin.Pin.Address.Replace(" ", "+").Replace("\n", "+");

            if (Device.RuntimePlatform == Device.iOS)
            {
                //https://developer.apple.com/library/ios/featuredarticles/iPhoneURLScheme_Reference/MapLinks/MapLinks.html
                Device.OpenUri(new Uri(string.Format("http://maps.apple.com/?daddr={0},{1}&saddr={2},{3}", customerPin.Pin.Position.Latitude, customerPin.Pin.Position.Longitude, StaticData.TrackingData.LastKnownPosition.Latitude, StaticData.TrackingData.LastKnownPosition.Longitude)));
            }
            else if (Device.RuntimePlatform == Device.Android)
            {
                // opens the 'task chooser' so the user can pick Maps, Chrome or other mapping app
                Device.OpenUri(new Uri(string.Format("http://maps.google.com/?daddr={0},{1}&saddr={2},{3}", customerPin.Pin.Position.Latitude, customerPin.Pin.Position.Longitude, StaticData.TrackingData.LastKnownPosition.Latitude, StaticData.TrackingData.LastKnownPosition.Longitude)));
            }
            else if (Device.RuntimePlatform == Device.WinPhone)
            {
                Device.OpenUri(new Uri("bingmaps:?rtp=adr.394 Pacific Ave San Francisco CA~adr.One Microsoft Way Redmond WA 98052"));
            }
        }

        /// <summary>
        /// Handles the ItemSelected event of the listViewPins control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SelectedItemChangedEventArgs"/> instance containing the event data.</param>
        private void listViewPins_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var selectedCustomPin = e.SelectedItem as CustomPinEntity;
            Device.OpenUri(new Uri(selectedCustomPin.Url));
        }
    }
}