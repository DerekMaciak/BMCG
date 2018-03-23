using Android.Hardware;
using BMCGMobile.Droid;


[assembly: Xamarin.Forms.Dependency(typeof(GeomagneticFieldService))]
namespace BMCGMobile.Droid
{
    public class GeomagneticFieldService : IGeomagneticField
    {

        public float GetGeomagneticField(float latitude, float longitude, float altitude, long timeMillis)
        {
            var geoField = new GeomagneticField(latitude, longitude, altitude, timeMillis);

            return geoField.Declination;
        }
    }
}