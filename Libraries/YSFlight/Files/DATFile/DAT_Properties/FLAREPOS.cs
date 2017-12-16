﻿using Com.OfficerFlake.Libraries.UnitsOfMeasurement;
using static Com.OfficerFlake.Libraries.YSFlight.Files.DAT.PropertyTypes;

namespace Com.OfficerFlake.Libraries.YSFlight.Files.DAT.Properties
{
    public class FLAREPOS : DAT_DualVector3
    {
        public FLAREPOS(Distance x1, Distance y1, Distance z1, Distance x2, Distance y2, Distance z2) : base("FLAREPOS", x1, y1, z1, x2, y2, z2)
        {
        }
    }
}
