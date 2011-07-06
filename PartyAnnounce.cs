﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria_Server.Plugin;
using Terraria_Server;
using Terraria_Server.Events;
using System.Xml;
using System.IO;

namespace PartyAnnounce
{
    public class PartyAnnounce : Plugin
    {
        public bool isEnabled = true;
        string state = "on";
        string partycolor;
        string user;
        string used = "off";
        string pluginFolder = Statics.PluginPath + Path.DirectorySeparatorChar + "PartyAnnounce";

        public override void Load()
        {
            Name = "PartyAnnounce";
            Description = "Announces who changes to what party";
            Author = "The Prodigy";
            Version = "1";
            TDSMBuild = 16;

            string pluginFolder = Statics.PluginPath + Path.DirectorySeparatorChar + "PartyAnnounce";
            //Create folder if it doesn't exist
            CreateDirectory(pluginFolder);
        }

        public override void Enable()
        {
            Console.WriteLine("[PARTYANNOUNCE] Ready and Waiting!");
            isEnabled = true;
            this.registerHook(Hooks.PLAYER_PARTYCHANGE);
            this.registerHook(Hooks.PLAYER_COMMAND);
        }
        public override void Disable()
        {
            Console.WriteLine("[PARTYANNOUNCE] Powering Down!");
            isEnabled = false;
        }
        public override void onPlayerCommand(PlayerCommandEvent Event)
        {
            if (isEnabled == false) { return; }
            if (Event.Message == "/partyannounce" && Event.Sender.Op)
            {
                Event.Sender.sendMessage("PartyAnnounce Help");
                Event.Sender.sendMessage("/partyannounce - Shows this help.");
                Event.Sender.sendMessage("/partytoggle - Toggles the state of Party Announce. (On/Off)");
            }
            if (Event.Message == "/partytoggle" && Event.Sender.Op && state == "on" && used == "off")
            {
                state = "off";
                Event.Sender.sendMessage("PartyAnnounce is now turned OFF.");
                used = "on";
            }
            if (Event.Message == "/partytoggle" && Event.Sender.Op && state == "off" && used == "off")
            {
                state = "on";
                Event.Sender.sendMessage("PartyAnnounce is now turned ON.");
                used = "on";
            }
            used = "off";
        }
        public override void onPlayerPartyChange(PartyChangeEvent Event)
        {
            if (isEnabled == false) { return; }
            user = Event.Player.getName();
            partycolor = Event.PartyType.ToString();
            if (state == "on")
            {
                if (partycolor == "NONE")
                {
                    NetMessage.SendData(25, -1, -1, user + " is no longer in a party.", 255, 50f, 255f, 130f);
                }
                else
                {
                    NetMessage.SendData(25, -1, -1, user + " has switched to the " + Event.PartyType + " party!", 255, 50f, 255f, 130f);

                }
            }


        }
        private static void CreateDirectory(string dirPath)
        {
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
        }
    }
}
