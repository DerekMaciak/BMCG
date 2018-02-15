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
using System;

using Xamarin.Forms;
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
        /// <summary>
        /// Initializes a new instance of the <see cref="FitnessPage"/> class.
        /// </summary>
        public FitnessPage()
        {
            InitializeComponent();

            this.BindingContext = StaticData.TrackingData;
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
    }
}