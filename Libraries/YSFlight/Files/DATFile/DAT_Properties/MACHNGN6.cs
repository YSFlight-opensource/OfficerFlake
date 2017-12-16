﻿using Com.OfficerFlake.Libraries.UnitsOfMeasurement;
using static Com.OfficerFlake.Libraries.YSFlight.Files.DAT.PropertyTypes;

namespace Com.OfficerFlake.Libraries.YSFlight.Files.DAT.Properties
{
    public class MACHNGN6 : DAT_Vector3
    {
        public MACHNGN6(Distance x, Distance y, Distance z) : base("MACHNGN6", x, y, z)
        {
        }
    }
}
