using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMCGMobile
{
    public interface IGeomagneticField
    {
        
        float GetGeomagneticField(float latitude, float longitude, float altitude, long timeMillis);
        
    }
}
