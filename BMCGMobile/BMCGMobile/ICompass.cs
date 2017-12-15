using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMCGMobile
{
    public interface ICompass
    {
        double GetHeading();
        void CompassStart();
        void CompassStop();
    }
}
