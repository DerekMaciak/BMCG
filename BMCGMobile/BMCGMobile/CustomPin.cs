using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;

namespace BMCGMobile
{
    public class CustomPin
    {
        public Pin Pin { get; set; }

        public string Id { get; set; }

        public string Url { get; set; }

        public PinTypes PinType { get; set; }

        public string PinImageName { get { return PinType == PinTypes.Kiosk ? "pin-kiosk.png" : "pin.png"; } }

        public enum PinTypes
        {
            Kiosk,
            Sign
        }
    }
}
