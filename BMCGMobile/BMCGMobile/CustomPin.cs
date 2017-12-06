using Xamarin.Forms.Maps;

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
                switch (PinType)
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
        }

        public enum PinTypes
        {
            Kiosk,
            Wayfinding,
            POI
        }
    }
}