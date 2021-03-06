﻿using System;

namespace Com.OfficerFlake.Libraries.YSFlight.Files.DAT.Properties
{
	public class RETRGEAR : DATProperty, IDAT_1_Parameter<Boolean>
	{
		public RETRGEAR(Boolean value) : base("RETRGEAR" + " " + string.Join(" ", value))
		{
			Value = value;
		}

		public Boolean Value { get; set; }
	}
}
