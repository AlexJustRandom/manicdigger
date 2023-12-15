/*
 * ServerUtilities Mod - Version 1.3
 * Last changed: 2015-02-18
 * Author: croxxx
 * 
 * This mod adds a lot of useful commands to your server.
 */
using System;

namespace ManicDigger.Mods
{
    public class ServerDebug : IMod
	{
        ModManager m;
        public void PreStart(ModManager m)
		{
			m.RequireMod("CoreBlocks");
		}

		public void Start(ModManager manager)
		{
			m = manager;
			iRestartInterval = m.GetAutoRestartInterval();

			m.RegisterOnCommand(TeleportToID);
			m.RegisterCommandHelp("SetModSpeed", "Teleports you to the player with the given ID");
			
			m.RegisterTimer(Uptime_Tick, (double)1);
			

		}
	
		

			
		 
		bool TeleportToID(int player, string command, string argument)
		{
			if (command.Equals("SetModSpeed", StringComparison.InvariantCultureIgnoreCase))
			{
			//	if (!m.PlayerHasPrivilege(player, "tp"))
				//{
				//	m.SendMessage(player, chatPrefix + m.colorError() + GetLocalizedString(0));
				//	System.Console.WriteLine(string.Format("[ServerUtilities] {0} tried to teleport by ID (no permission)", m.GetPlayerName(player)));
				//	return true;
				//}
				int targetID;
				try
				{
					targetID = Convert.ToInt32(argument);
				}
				catch (FormatException)
				{
					m.SendMessage(player, chatPrefix + m.colorError() + GetLocalizedString(1));
					return true;
				}
				catch (OverflowException)
				{
					m.SendMessage(player, chatPrefix + m.colorError() + GetLocalizedString(2));
					return true;
				}
                m.VIPDEBUGTEST("spmod", val);
 				
			}
			return false;
		}

	 
	 
		
	}
}
