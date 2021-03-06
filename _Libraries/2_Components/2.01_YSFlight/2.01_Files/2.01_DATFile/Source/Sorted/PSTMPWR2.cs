﻿using System;

namespace Com.OfficerFlake.Libraries.YSFlight.Files.DAT.Properties
{
	public class PSTMPWR2 : DATProperty, IDAT_1_Parameter<Single>
	{
		public PSTMPWR2(Single value) : base("PSTMPWR2" + " " + string.Join(" ", value))
		{
			Value = value;
		}

		public Single Value { get; set; }
	}
}
