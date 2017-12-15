using BMCGMobile.Droid;
using Plugin.Compass;

[assembly: Xamarin.Forms.Dependency(typeof(Compass))]
namespace BMCGMobile.Droid
{
    public class Compass : ICompass
    {
        private double _Heading;
        public double Heading { get { return _Heading; } }

        public void CompassStart()
        {
            CrossCompass.Current.CompassChanged += (s, e) =>
            {
                // Debug.WriteLine("*** Compass Heading = {0}", e.Heading);

                _Heading = e.Heading;
            };

            CrossCompass.Current.Start();
        }

        public void CompassStop()
        {
            CrossCompass.Current.Stop();
        }

        public double GetHeading()
        {
            return _Heading;
        }
    }
}