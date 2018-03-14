// ***********************************************************************
// Assembly         : BMCGMobile
// Author           : Derek Maciak
// Created          : 01-23-2018
//
// Last Modified By : Derek Maciak
// Last Modified On : 10-09-2017
// ***********************************************************************
// <copyright file="SettingsPage.xaml.cs" company="">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using BMCGMobile.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BMCGMobile
{
    /// <summary>
    /// Class SettingsPage.
    /// </summary>
    /// <seealso cref="Xamarin.Forms.ContentPage" />
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsPage"/> class.
        /// </summary>
        public SettingsPage()
        {
            InitializeComponent();

            this.BindingContext = StaticData.TrackingData.UserSettings;

            StaticData.TrackingData.UserSettings.PropertyChanged += UserSettings_PropertyChanged;
        }

        private void UserSettings_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsAutoTracking")
            {
                if (!StaticData.TrackingData.UserSettings.IsAutoTracking)
                {
                    _DisplayAlertAsync();
                }
            }
        }

        private void RestoreDefaultsButton_Clicked(object sender, System.EventArgs e)
        {
            StaticData.TrackingData.UserSettings.SetDefaults();
        }

        private async void _DisplayAlertAsync()
        {
            await DisplayAlert(DesciptionResource.Notification, string.Format(DesciptionResource.AutoTrackingInformationalMessage, DesciptionResource.AutoTrack, DesciptionResource.DisplayOffTrailAlert, DesciptionResource.TodayFitnessTracking ), "Ok");

        }

    }
}