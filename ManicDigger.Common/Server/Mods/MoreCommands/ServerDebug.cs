﻿/*
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
         }

        public void Start(ModManager manager)
        {
            m = manager;

            m.RegisterOnCommand(TeleportToID);
            m.RegisterCommandHelp("SetModSpeed", "Teleports you to the player with the given ID");


        }
 
        bool TeleportToID(int player, string command, string argument)
        {
            if (command.Equals("SetModSpeed", StringComparison.InvariantCultureIgnoreCase))
            {
                //  if (!m.PlayerHasPrivilege(player, "tp"))
                //{
                //  m.SendMessage(player, chatPrefix + m.colorError() + GetLocalizedString(0));
                //  System.Console.WriteLine(string.Format("[ServerUtilities] {0} tried to teleport by ID (no permission)", m.GetPlayerName(player)));
                //  return true;
                //}
                int targetID;
                try
                {
                    targetID = Convert.ToInt32(argument);
                }
                catch (FormatException)
                {
                    return true;
                }
                catch (OverflowException)
                {
                    return true;
                }
                m.VIPDEBUGTEST("spmod", targetID);

            }
            return false;
        }




    }
}