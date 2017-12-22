﻿using Com.OfficerFlake.Libraries.YSFlight.Types;
using static Com.OfficerFlake.Libraries.YSFlight.Files.DAT.PropertyTypes;

namespace Com.OfficerFlake.Libraries.YSFlight.Files.DAT.Properties
{
    public class WEAPONCH : DATProperty, IDAT_1_Parameter<WeaponCategory>
	{
		public WEAPONCH(WeaponCategory value) : base("WEAPONCH" + " " + string.Join(" ", value))
		{
			Value = value;
		}

		public WeaponCategory Value { get; set; }
	}
}