using BMCGMobile;
using BMCGMobile.Entities;
using BMCGMobile.iOS;
using CoreAnimation;
using CoreGraphics;
using CoreLocation;
using Google.Maps;
using MapKit;
using ObjCRuntime;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.GoogleMaps.iOS;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]

namespace BMCGMobile.iOS
{
    public class CustomMapRenderer : MapRenderer
    {

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName.Equals("VisibleRegion"))
            {
                //  NativeMap.InfoTapped += InfoTappedFunct;
                NativeMap.TappedMarker = TapperMarkerFunct;
            }
        }


        bool TapperMarkerFunct(MapView map, Marker marker)
        {



            map.MarkerInfoWindow = new GMSInfoFor(markerInfoWindow);

            return false;
        }


        UIView markerInfoWindow(UIView view, Marker marker)
        {
           
            var v = new UIView(new RectangleF(0, 0, 260, 85));
            v.BackgroundColor = UIColor.Clear;
            v.Layer.CornerRadius = 10;


            var path = new UIBezierPath();
            path.MoveTo(new CGPoint(0, 0));
            path.AddLineTo(new CGPoint(260, 0));
            // Make y 15 less than UIView height
            path.AddLineTo(new CGPoint(260, 70));

            // Draw arrow
            // Make y 15 less than UIView height
            path.AddLineTo(new CGPoint(150, 70));
            // Make y 3 less than UIView height
            path.AddLineTo(new CGPoint(130, 82));
            // Make y 15 less than UIView height
            path.AddLineTo(new CGPoint(110, 70));

            path.AddLineTo(new CGPoint(0, 70));

            path.ClosePath();

            var shape = new CAShapeLayer();
            //shape.backgroundColor = UIColor.blue.cgColor
            shape.FillColor = UIColor.White.CGColor;
            shape.Path = path.CGPath;
            //shape.BorderColor = UIColor.Black.CGColor;
            //shape.BorderWidth = 2;
            //shape.CornerRadius = 10;


            var headerLabel = new UILabel(new RectangleF(27, 3, 230, 30));
            // headerLabel.LineBreakMode = UILineBreakMode.WordWrap;
            //headerLabel.Font = UIFont.PreferredSubheadline;
            headerLabel.LineBreakMode = UILineBreakMode.TailTruncation;
            headerLabel.Font = UIFont.BoldSystemFontOfSize(14);
            headerLabel.Text = marker.Title;
            //headerLabel.AdjustsFontSizeToFitWidth = true;
           


            var secondLabel = new UILabel(new RectangleF(27, 25, 230, 20));
            secondLabel.LineBreakMode = UILineBreakMode.TailTruncation;
            secondLabel.Font = secondLabel.Font.WithSize(11);
            secondLabel.Text = marker.Snippet;
            //secondLabel.AdjustsFontSizeToFitWidth = true;
           

            var img1 = new UIImageView(new RectangleF(5, 5, 16, 30));
            img1.BackgroundColor = UIColor.White;
            img1.Image = marker.Icon;
            img1.ClipsToBounds = true;

            var img2 = new UIImageView(new RectangleF(3, 45, 20, 20));
            img2.BackgroundColor = UIColor.White;
            img2.Image = UIImage.FromBundle("information.png");    
            img2.ClipsToBounds = true;

            var infoLabel = new UILabel(new RectangleF(27, 45, 230, 20));
            infoLabel.LineBreakMode = UILineBreakMode.TailTruncation;
            infoLabel.Font = secondLabel.Font.WithSize(12);
            infoLabel.Text = "Tap to view photos and information";
  

            v.Layer.AddSublayer(shape);


            v.AddSubview(headerLabel);
            v.AddSubview(secondLabel);
            v.AddSubview(img1);
            v.AddSubview(img2);
            v.AddSubview(infoLabel);
            

            return v;
        }




        //protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        //{
        //    base.OnElementChanged(e);

        //    if (e.OldElement != null)
        //    {
        //        var nativeMap = Control as MKMapView;
        //        if (nativeMap != null)
        //        {
        //            // Overlay
        //            nativeMap.RemoveOverlays(nativeMap.Overlays);
        //            nativeMap.OverlayRenderer = null;
        //            polylineRenderer = null;

        //            //Pin Annotation
        //            nativeMap.RemoveAnnotations(nativeMap.Annotations);
        //            nativeMap.GetViewForAnnotation = null;
        //            nativeMap.CalloutAccessoryControlTapped -= OnCalloutAccessoryControlTapped;
        //            nativeMap.DidSelectAnnotationView -= OnDidSelectAnnotationView;
        //            nativeMap.DidDeselectAnnotationView -= OnDidDeselectAnnotationView;
        //        }
        //    }

        //    if (e.NewElement != null)
        //    {
        //        var formsMap = (CustomMap)e.NewElement;
        //        var nativeMap = Control as GMSMapView;

        //        if (nativeMap == null)
        //        {
        //            return;
        //        }
        //        // Overlay
        //        nativeMap.OverlayRenderer = GetOverlayRenderer;

        //        CLLocationCoordinate2D[] coords = new CLLocationCoordinate2D[StaticData.WayfindingCoordinates.Count];

        //        int index = 0;
        //        foreach (var position in StaticData.WayfindingCoordinates)
        //        {
        //            coords[index] = new CLLocationCoordinate2D(position.Latitude, position.Longitude);
        //            index++;
        //        }

        //        var routeOverlay = MKPolyline.FromCoordinates(coords);
        //        nativeMap.AddOverlay(routeOverlay);

        //        // Pin Annotation
        //        customPins = new List<CustomPinEntity>(StaticData.CustomPins); 

        //        nativeMap.GetViewForAnnotation = GetViewForAnnotation;
        //        nativeMap.CalloutAccessoryControlTapped += OnCalloutAccessoryControlTapped;
        //        nativeMap.DidSelectAnnotationView += OnDidSelectAnnotationView;
        //        nativeMap.DidDeselectAnnotationView += OnDidDeselectAnnotationView;
        //    }
        //}

        //private MKOverlayRenderer GetOverlayRenderer(MKMapView mapView, IMKOverlay overlayWrapper)
        //{
        //    if (polylineRenderer == null && !Equals(overlayWrapper, null))
        //    {
        //        var overlay = Runtime.GetNSObject(overlayWrapper.Handle) as IMKOverlay;
        //        polylineRenderer = new MKPolylineRenderer(overlay as MKPolyline)
        //        {
        //            FillColor = UIColor.Red,
        //            StrokeColor = UIColor.Red,
        //            LineWidth = 6,
        //            Alpha = 0.4f
        //        };
        //    }
        //    return polylineRenderer;
        //}

        //#region Pin Annotation

        //private MKAnnotationView GetViewForAnnotation(MKMapView mapView, IMKAnnotation annotation)
        //{
        //    MKAnnotationView annotationView = null;

        //    if (annotation is MKUserLocation)
        //        return null;

        //    var anno = annotation as MKPointAnnotation;
        //    var customPin = GetCustomPin(anno);
        //    if (customPin == null)
        //    {
        //        return null;
        //        //throw new Exception("Custom pin not found");
        //    }

        //    annotationView = mapView.DequeueReusableAnnotation(customPin.Id);
        //    if (annotationView == null)
        //    {
        //        annotationView = new CustomMKAnnotationView(annotation, customPin.Id)
        //        {
        //            Image = UIImage.FromFile(customPin.PinImageName),
        //            CalloutOffset = new CGPoint(0, 0),
        //            LeftCalloutAccessoryView = new UIImageView(UIImage.FromFile(customPin.PinImageName)),
        //            RightCalloutAccessoryView = UIButton.FromType(UIButtonType.DetailDisclosure),
        //        };

        //        ((CustomMKAnnotationView)annotationView).Id = customPin.Id;
        //        ((CustomMKAnnotationView)annotationView).Url = customPin.Url;
        //    }
        //    annotationView.CanShowCallout = true;
        //    return annotationView;
        //}

        //private void OnCalloutAccessoryControlTapped(object sender, MKMapViewAccessoryTappedEventArgs e)
        //{
        //    var customView = e.View as CustomMKAnnotationView;
        //    if (!string.IsNullOrWhiteSpace(customView.Url))
        //    {
        //        UIApplication.SharedApplication.OpenUrl(new Foundation.NSUrl(customView.Url));
        //    }
        //}

        //private void OnDidSelectAnnotationView(object sender, MKAnnotationViewEventArgs e)
        //{
        //    var customView = e.View as CustomMKAnnotationView;
        //    customPinView = new UIView();

        //    //if (customView.Id == "Morris Canal Park")
        //    //{
        //    //    customPinView.Frame = new CGRect(0, 0, 200, 84);
        //    //    var image = new UIImageView(new CGRect(0, 0, 200, 84));
        //    //    image.Image = UIImage.FromFile("schedule.png");
        //    //    customPinView.AddSubview(image);
        //    //    customPinView.Center = new CGPoint(0, -(e.View.Frame.Height + 75));
        //    //    e.View.AddSubview(customPinView);
        //    //}
        //}

        //private void OnDidDeselectAnnotationView(object sender, MKAnnotationViewEventArgs e)
        //{
        //    if (!e.View.Selected)
        //    {
        //        customPinView.RemoveFromSuperview();
        //        customPinView.Dispose();
        //        customPinView = null;
        //    }
        //}

        //private CustomPinEntity GetCustomPin(MKPointAnnotation annotation)
        //{
        //    if (annotation != null)
        //    {
        //        var position = new Position(annotation.Coordinate.Latitude, annotation.Coordinate.Longitude);
        //        return customPins.FirstOrDefault(pin => pin.Pin.Position == position);
        //    }

        //    return null;
        //}

        //#endregion Pin Annotation
    }
}