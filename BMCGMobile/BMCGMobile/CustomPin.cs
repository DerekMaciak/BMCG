using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace BMCGMobile
{
    public class CustomPin
    {
        public Pin Pin { get; set; }

        public string Id { get; set; }

        public string Url { get; set; }

        public PinTypes PinType { get; set; }

        public string PinImageName
        {
            get
            {
                return GetPinImageName(PinType);
            }
        }

        public enum PinTypes
        {
            Kiosk,
            Wayfinding,
            POI
        }

        public static string GetPinImageName(PinTypes pinType)
        {

            switch (pinType)
            {
                case PinTypes.Kiosk:
                    return "marker26green.png";

                case PinTypes.Wayfinding:
                    return "marker26red.png";

                case PinTypes.POI:
                    return "marker26black.png";

                default:
                    break;
            }

            return "marker26red.png";

        }

        public static Color GetPinImageColor(PinTypes pinType)
        {
            switch (pinType)
            {
                case PinTypes.Kiosk:
                    return Color.Red;

                case PinTypes.Wayfinding:
                    return Color.Green;

                case PinTypes.POI:
                    return Color.Black;

                default:
                    break;
            }

            return Color.Red;

        }
    }
}