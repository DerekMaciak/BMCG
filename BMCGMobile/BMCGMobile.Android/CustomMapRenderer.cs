using System;
using System.Collections.Generic;
using System.ComponentModel;
using Android.App;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Views;
using Android.Widget;
using BMCGMobile;
using BMCGMobile.Droid;

using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.GoogleMaps.Android;
using Xamarin.Forms.Platform.Android;
using static Android.Gms.Maps.GoogleMap;

[assembly: Xamarin.Forms.ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace BMCGMobile.Droid
{
    public class CustomMapRenderer : MapRenderer, IInfoWindowAdapter
    {

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);


            if (e.PropertyName.Equals("VisibleRegion"))
            {

                NativeMap.SetInfoWindowAdapter(this);
            }
        }


        public Android.Views.View GetInfoContents(Marker marker)
        {

            return null;
        }

        public Android.Views.View GetInfoWindow(Marker marker)
        {
            var inflater = Application.Context.GetSystemService(Android.Content.Context.LayoutInflaterService) as LayoutInflater;
            if (inflater != null)
            {

                View view = inflater.Inflate(Resource.Layout.InfoWindow, null);

                var infoTitle = view.FindViewById<TextView>(Resource.Id.InfoWindowTitle);
                var infoSubtitle = view.FindViewById<TextView>(Resource.Id.InfoWindowSubtitle);
                
                if (infoTitle != null)
                {
                    infoTitle.Text = marker.Title;
                }
                if (infoSubtitle != null)
                {
                    infoSubtitle.Text = marker.Snippet;
                }
            

                return view;
            }
            return null;
        }
    }
    
}
