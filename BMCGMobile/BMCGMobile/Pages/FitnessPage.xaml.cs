// ***********************************************************************
// Assembly         : BMCGMobile
// Author           : Derek Maciak
// Created          : 01-23-2018
//
// Last Modified By : Derek Maciak
// Last Modified On : 02-14-2018
// ***********************************************************************
// <copyright file="FitnessPage.xaml.cs" company="">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using BMCGMobile.Resources;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;

namespace BMCGMobile
{
    /// <summary>
    /// Class FitnessPage.
    /// </summary>
    /// <seealso cref="Xamarin.Forms.ContentPage" />
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FitnessPage : ContentPage
    {
        private DateTime _Fitnessdate;
        private bool _IsToday;
        private bool _FirstTime = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="FitnessPage"/> class.
        /// </summary>
        public FitnessPage()
        {
            InitializeComponent();
            StaticData.TrackingData.PropertyChanged += TrackingData_PropertyChanged;

            _Fitnessdate = DateTime.Now;
            _IsToday = true;
            this.BindingContext = StaticData.TrackingData.FitnessToday;
            isOnTrailSwitch.BindingContext = StaticData.TrackingData;
        }

        public FitnessPage(DateTime fitnessDate)
        {
            InitializeComponent();

            _Fitnessdate = fitnessDate;
            this.BindingContext = StaticData.TrackingData.FitnessHistory.Where(w => w.FitnessDate.Date == _Fitnessdate.Date).FirstOrDefault();

            btnFitnessHistory.IsVisible = false;
            btnRemoveFitnessHistory.IsVisible = true;

            Title = _Fitnessdate.ToString("D");
        }

        /// <summary>
        /// Handles the <see cref="E:FitnessHistoryButtonClicked" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private async void OnFitnessHistoryButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FitnessHistoryPage());
        }

        private async void OnRemoveFitnessHistoryButtonClicked(object sender, EventArgs e)
        {
            // Update Binding in case the fitness history was removed
            var currentFitnessEntity = StaticData.TrackingData.FitnessHistory.Where(w => w.FitnessDate.Date == _Fitnessdate.Date).FirstOrDefault();
            if (currentFitnessEntity != null)
            {
                StaticData.TrackingData.FitnessHistory.Remove(currentFitnessEntity);
            }

            await Navigation.PopAsync();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                if (_IsToday)
                {
                    this.BindingContext = StaticData.TrackingData.FitnessToday;

                    StaticData.TrackingData.FitnessToday.PropertyChanged -= FitnessToday_PropertyChanged;
                    StaticData.TrackingData.FitnessToday.PropertyChanged += FitnessToday_PropertyChanged;
                }
                else
                {
                    this.BindingContext = StaticData.TrackingData.FitnessHistory.Where(w => w.FitnessDate.Date == _Fitnessdate.Date).FirstOrDefault();
                }

                if (_FirstTime)
                {
                    if (StaticData.TrackingData.IsGPXDataLoaded)
                    {
                        _FirstTime = false;
                        // If GPS Data is Loaded - then Plot; otherwise subscribe to loaded event
                        customMap.LoadWayFindingCoordinatePins();
                        customMap.PlotPolylineTrack();

                        StaticData.TrackingData.PropertyChanged -= TrackingData_PropertyChanged;
                    }
                    else
                    {
                        // Set initial Map state
                        customMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(40.810657, -74.186378), Distance.FromMiles(1)));

                    }

                }

                customMap.PlotUserOnTrailSegmentsPolylineTrack(_Fitnessdate);

                customMap.CenterMapToUserPositions(_Fitnessdate);

                customMap.IsVisible = true;
            }
            catch (Exception ex)
            {
                await DisplayAlert(DesciptionResource.Error, string.Format(DesciptionResource.ErrorMessage, ex), "Ok");
            }
        }

        private void TrackingData_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsGPXDataLoaded")
            {
                if (StaticData.TrackingData.IsGPXDataLoaded)
                {
                    _FirstTime = false;
                    customMap.LoadWayFindingCoordinatePins();
                    customMap.PlotPolylineTrack();

                    StaticData.TrackingData.PropertyChanged -= TrackingData_PropertyChanged;
                    
                }
            }
        }

        private void FitnessToday_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "TotalStepCount")
            {
                // When steps change - replot user polyline
                customMap.PlotUserOnTrailSegmentsPolylineTrack(_Fitnessdate);
                customMap.CenterMapToUserPositions(_Fitnessdate);
            }
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            if (width > height)
            {
                //Assume Landscape
                statistics.IsVisible = false;
                fitnessToday.IsVisible = false;
                btnRemoveFitnessHistory.IsVisible = false;
            }
            else
            {
                // Assume Portrait
                statistics.IsVisible = true;
                if (_IsToday)
                {
                    fitnessToday.IsVisible = true;
                }
                else
                {
                    btnRemoveFitnessHistory.IsVisible = true;
                }
            }
        }
    }
}