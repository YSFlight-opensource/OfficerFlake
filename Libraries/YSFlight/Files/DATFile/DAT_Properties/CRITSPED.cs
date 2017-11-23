﻿using Com.OfficerFlake.Libraries.UnitsOfMeasurement;
using static Com.OfficerFlake.Libraries.YSFlight.Files.DAT.PropertyTypes;

namespace Com.OfficerFlake.Libraries.YSFlight.Files.DAT.Properties
{
    public class CRITSPED : DAT_Speed
    {
        public CRITSPED(Speed value) : base("CRITSPED", value)
        {
        }
    }
}