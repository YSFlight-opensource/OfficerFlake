﻿using System;

namespace Com.OfficerFlake.Libraries.YSFlight.Files.DAT.Properties
{
	public class TURRETNM : DATProperty, IDAT_2_Parameters<UInt32, String>
	{
		public TURRETNM(UInt32 value1, String value2) : base("TURRETNM" + " " + value1 + " " + value2)
		{
			Value1 = value1;
			Value2 = value2;
		}

		public UInt32 Value1 { get; set; }
		public String Value2 { get; set; }
	}
}
