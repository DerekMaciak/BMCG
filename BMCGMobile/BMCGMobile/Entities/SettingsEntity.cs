﻿using System;
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
            AutoTrackingMaximumDistanceFromTrailInFeet = 50; // 50 Feet
            IsAutoTracking = true;
            HeightInInches = 70;
        }

        public double AutoTrackingMaximumDistanceFromTrailInFeet { set; get; }

        public bool IsAutoTracking { set; get; }

        public int HeightInInches { set; get; }
    }
}