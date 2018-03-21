﻿using Com.OfficerFlake.Libraries.Interfaces;

namespace Com.OfficerFlake.Libraries.YSFlight.Files.DAT.Properties
{
	public class MACHNGUN : DATProperty, IDAT_1_Parameter<ICoordinate3>
	{
		public MACHNGUN(ICoordinate3 value) : base("MACHNGUN" + " " + string.Join(" ", value))
		{
			Value = value;
		}

		public ICoordinate3 Value { get; set; }
	}
}
