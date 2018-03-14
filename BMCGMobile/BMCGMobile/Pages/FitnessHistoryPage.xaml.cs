// ***********************************************************************
// Assembly         : BMCGMobile
// Author           : Derek Maciak
// Created          : 01-23-2018
//
// Last Modified By : Derek Maciak
// Last Modified On : 10-12-2017
// ***********************************************************************
// <copyright file="FitnessHistoryPage.xaml.cs" company="">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections.ObjectModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Linq;
using BMCGMobile.Entities;

namespace BMCGMobile
{
    /// <summary>
    /// Class FitnessHistoryPage.
    /// </summary>
    /// <seealso cref="Xamarin.Forms.ContentPage" />
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FitnessHistoryPage : ContentPage
    {
       

        /// <summary>
        /// Initializes a new instance of the <see cref="FitnessHistoryPage"/> class.
        /// </summary>
        public FitnessHistoryPage()
        {
            InitializeComponent();

            listViewFitness.ItemsSource = StaticData.TrackingData.FitnessHistory.OrderByDescending(o => o.FitnessDate); 
        }

        protected override void OnAppearing()
        {
            // Rebind in case the user removed a fitness history
            listViewFitness.ItemsSource = null;
            listViewFitness.ItemsSource = StaticData.TrackingData.FitnessHistory.OrderByDescending(o => o.FitnessDate); 
            
            base.OnAppearing();
        }

        /// <summary>
        /// Handles the ItemTapped event of the Handle control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ItemTappedEventArgs"/> instance containing the event data.</param>
        private async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            var currentFitnessEntity = e.Item as FitnessEntity;

            //await DisplayAlert("Item Tapped", "An item was tapped.", "OK");

            var nextPage = new FitnessPage(currentFitnessEntity.FitnessDate);
            await this.Navigation.PushAsync(nextPage);


            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        private void listViewFitness_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }
    }
}