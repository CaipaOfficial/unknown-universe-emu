﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.netty.commands.old_client;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.map.collectables;
using NettyBaseReloaded.Game.objects.world.map.objects.assets;
using NettyBaseReloaded.Game.objects.world.map.objects.stations;
using NettyBaseReloaded.Game.objects.world.players.extra.techs;

namespace NettyBaseReloaded.Game.controllers.login
{
    abstract class ILogin
    {
        public GameSession GameSession;


        protected ILogin(GameSession gameSession)
        {
            GameSession = gameSession;
        }

        /// <summary>
        /// Executable for login
        /// </summary>
        public abstract void Execute();

        public void SendSettings()
        {
            Packet.Builder.HotkeysCommand(GameSession);
            Packet.Builder.UserSettingsCommand(GameSession);
            Packet.Builder.SendUserSettings(GameSession);
        }

        public void SendLegacy()
        {
            Console.WriteLine("Legacy anals me");
            SendLegacy(GameSession);
            AddTempDroneFormations();
            AddTempTechs();
        }

        public static void SendLegacy(GameSession GameSession)
        {
            Packet.Builder.DronesCommand(GameSession, GameSession.Player);
            //Packet.Builder.LegacyModule(GameSession, "0|n|t|" + GameSession.Player.Id + "|222|most_wanted");

            Packet.Builder.LegacyModule(GameSession, "0|A|BK|0"); //green booty
            Packet.Builder.LegacyModule(GameSession, "0|A|BKR|0"); //red booty
            Packet.Builder.LegacyModule(GameSession, "0|A|BKB|0"); //blue booty
            Packet.Builder.LegacyModule(GameSession, "0|TR");
            Packet.Builder.LegacyModule(GameSession, "0|A|CC|" + GameSession.Player.CurrentConfig);
            Packet.Builder.LegacyModule(GameSession, "0|ps|nüscht|");
            Packet.Builder.LegacyModule(GameSession, "0|ps|blk|0");
            Packet.Builder.LegacyModule(GameSession, "0|g|a|b,1000,1,10000.0,C,2,500.0,U,3,1000.0,U,5,4000.0,U|r,100,1,10000,C,2,50000,C,3,500.0,U,4,700.0,");
            GameSession.Player.LoadExtras();
            //Packet.Builder.VideoWindowCreateCommand(GameSession, 1, "c", true, new List<string> { "login_dialog_1", "login_dialog_2" }, 0, 1);
            //Packet.Builder.MineCreateCommand(GameSession, "asdf", 6, GameSession.Player.Position, false);
            Packet.Builder.PetInitializationCommand(GameSession, GameSession.Player.Pet);
            Packet.Builder.HellstormStatusCommand(GameSession);

            Packet.Builder.LegacyModule(GameSession, "0|n|w|0");

            //MBA -> MenuButtonAccess
            //DB -> Disable button
            //EB -> Enable button
            //Packet.Builder.LegacyModule(GameSession, "0|UI|MBA|DB|7");
            Packet.Builder.LegacyModule(GameSession, "0|UI|MBA|DB|6");
            //Packet.Builder.LegacyModule(GameSession, "0|UI|MBA|DB|2");
            Packet.Builder.LegacyModule(GameSession, "0|UI|MBA|DB|4");
            //Packet.Builder.LegacyModule(GameSession, "0|UI|MBA|DB|5");

            //Fix for 0 credits
            Packet.Builder.LegacyModule(GameSession, "0|A|C|" + GameSession.Player.Information.Credits.Get() + "|" + GameSession.Player.Information.Uridium.Get());

            Packet.Builder.LegacyModule(GameSession
                , "0|A|CC|" + GameSession.Player.CurrentConfig);

            if (GameSession.Player.Group != null)
                Packet.Builder.GroupInitializationCommand(GameSession);
        }

        private void AddTempDroneFormations()
        {
            Packet.Builder.DroneFormationAvailableFormationsCommand(GameSession);
        }

        private void AddTempTechs()
        {
            GameSession.Player.Techs.Add(new RocketPrecission(GameSession.Player));
            GameSession.Player.Techs.Add(new ShieldBuff(GameSession.Player));
            GameSession.Player.Techs.Add(new BattleRepairRobot(GameSession.Player));
            GameSession.Player.Techs.Add(new EnergyLeech(GameSession.Player));
            GameSession.Player.Techs.Add(new ChainImpulse(GameSession.Player));

            Packet.Builder.TechStatusCommand(GameSession);

            /**
            var elaStatus = 0; // not equipped
            var elaCount = 0; // count
            var elaTimeLeft = 0;
            var eciStatus = 0; // not equipped
            var eciCount = 0;
            var eciTimeLeft = 0;
            var rpmStatus = 1; // not equipped
            var rpmCount = 99;
            var rpmTimeLeft = 0;
            var sbuStatus = 1;
            var sbuCount = 99;
            var sbuTimeLeft = 0;
            var brbStatus = 1;
            var brbCount = 99;
            var brbTimeLeft = 0;
            //{rpmStatus}|{rpmCount}|{rpmTimeLeft}|{sbuStatus}|{sbuCount}|{sbuTimeLeft}|{brbStatus}|{brbCount}|{brbTimeLeft}
            string techPacket = $"{elaStatus}|{elaCount}|{elaTimeLeft}|{eciStatus}|{eciCount}|{eciTimeLeft}|";
            foreach (var tech in GameSession.Player.Techs)
            {
                techPacket += $"{tech.GetStatus()}|{99}|{tech.TimeLeft}";
            }

            Packet.Builder.LegacyModule(GameSession, $"0|TX|S|{techPacket}");
            */
        }
    } 
}
