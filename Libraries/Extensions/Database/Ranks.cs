﻿using Com.OfficerFlake.Libraries.Interfaces;

namespace Com.OfficerFlake.Libraries.Extensions
{
    public static class Ranks
    {
	    public static IRank None => ObjectFactory.GetRankNone;
	}
}