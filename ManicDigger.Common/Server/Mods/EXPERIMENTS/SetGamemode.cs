/*
 * Gamemode Mod - Version 1.2
 * Last change: 2024-03-28
 * Author: Alex
 * 
 * Following command is added:
 * /location
 * Command options are:
 *    add [startx] [starty] [startz] [endx] [endy] [endz] [name]
 *    delete [name]
 *    list
 *    clear
 *    help
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ManicDigger.Mods
{
    public class SetGamemode : IMod
    {
        //Enter the desired language code here. Currently supported are EN and DE.
        string languageCode = "EN";

        public void PreStart(ModManager m) { }

        public void Start(ModManager m)
        {
            this.m = m;
            m.RegisterPrivilege("gamemode");
            m.RegisterCommandHelp("gamemode", "Allows you to change gamemode.");
            m.RegisterOnCommand(ManageAreas);
        }
        ModManager m;
        string chatPrefix = "&8[&6Locations&8] ";

        bool ManageAreas(int player, string command, string argument)
        {
            if (command.Equals("gamemode", StringComparison.InvariantCultureIgnoreCase))
            {
                string[] args;
                string option = "";
                try
                {
                    args = argument.Split(' ');
                    option = args[0];
                }
                catch
                {
                    m.SendMessage(player, chatPrefix + m.colorError() + "invalid_args");
                    return true;
                }

                if (!m.PlayerHasPrivilege(player, "gamemode"))
                {
                    m.SendMessage(player, chatPrefix + m.colorError() +"privilege_missing");
                    System.Console.WriteLine(string.Format("[Locations] {0} tried to add a location (no permission)", m.GetPlayerName(player)));
                    return true;
                }
                if (option.ToLower() == "creative") {
                    m.SendMessage(player, chatPrefix + "Gamemode set creative");

                    m.SetGamemode(true);
                    return true;
                }
                if (option.ToLower() == "survival")
                {
                    m.SendMessage(player, chatPrefix + "Gamemode set survival");

                    m.SetGamemode(false);
                    return true;
                }

                m.SendMessage(player, chatPrefix + m.colorError() + "Gamemode not found");

                return true;
            }
            return false;
        }
    }
}
