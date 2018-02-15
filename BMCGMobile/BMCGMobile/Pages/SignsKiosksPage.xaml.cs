using BMCGMobile.Entities;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BMCGMobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignsKiosksPage : ContentPage
    {
        public SignsKiosksPage()
        {
            InitializeComponent();

          
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (listViewPins.ItemsSource == null)
            {
                listViewPins.ItemsSource = StaticData.CustomPins;
            }

            StaticData.TrackingData.PropertyChanged += TrackingData_PropertyChanged;
        }

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

        private void Button_Clicked(object sender, EventArgs e)
        {
            var customerPin = (sender as Button).CommandParameter as CustomPinEntity;

            var destAddr = customerPin.Pin.Address.Replace(" ", "+").Replace("\n", "+");

            if (Device.RuntimePlatform == Device.iOS)
            {
                //https://developer.apple.com/library/ios/featuredarticles/iPhoneURLScheme_Reference/MapLinks/MapLinks.html
                Device.OpenUri(new Uri(string.Format("http://maps.apple.com/?daddr={0}&saddr={1}", destAddr, null)));
            }
            else if (Device.RuntimePlatform == Device.Android)
            {
                // opens the 'task chooser' so the user can pick Maps, Chrome or other mapping app
                Device.OpenUri(new Uri(string.Format("http://maps.google.com/?daddr={0}&saddr={1}", destAddr, null)));
            }
            else if (Device.RuntimePlatform == Device.WinPhone)
            {
                Device.OpenUri(new Uri("bingmaps:?rtp=adr.394 Pacific Ave San Francisco CA~adr.One Microsoft Way Redmond WA 98052"));
            }

           
        }

        private void listViewPins_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var selectedCustomPin = e.SelectedItem as CustomPinEntity;
            Device.OpenUri(new Uri(selectedCustomPin.Url));

           
        }
    }
}