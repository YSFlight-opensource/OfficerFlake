﻿namespace Com.OfficerFlake.Libraries.Interfaces
{
	#region Ranks
	public interface IRank
	{
		int Index { get; set; }
		IRichTextString Name { get; set; }
		ILocalPermissions LocalPermissions { get; set; }
		IGlobalPermissions GlobalPermissions { get; set; }

		string ToString();
	}
	#endregion
}
