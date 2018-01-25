using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMCGMobile.Entities
{
    public class SettingsEntity : EntityBase
    {
        public SettingsEntity()
        {
            AutoTrackingMaximumDistanceFromTrail = 10; // 10 Feet
            IsAutoTracking = true;
        }

        public double AutoTrackingMaximumDistanceFromTrail { set; get; }

        public bool IsAutoTracking { set; get; }
    }
}
