﻿using Com.OfficerFlake.Libraries.Extensions;
using Com.OfficerFlake.Libraries.Interfaces;
using Com.OfficerFlake.Libraries.Logger;

namespace Com.OfficerFlake.Libraries.Networking
{
	public static partial class PacketProcessor
	{
		public static partial class Server
		{
			private static bool Process_Type_32_ChatMessage(IConnection thisConnection, IPacket_32_ChatMessage ChatMessagePacket)
			{
				Console.AddUserMessage(ChatMessagePacket.User, ChatMessagePacket.Message);
				foreach (IConnection connection in Connections.AllConnections)
				{
					connection.SendMessageAsync("(" + ChatMessagePacket.User.UserName.ToUnformattedSystemString() + ")" + ChatMessagePacket.Message).ConfigureAwait(false);
				}
				return true;
			}
		}
	}
}
