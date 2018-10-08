﻿using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NettyBaseReloaded.Game;
using NettyBaseReloaded.Main.objects;
using NettyBaseReloaded.Properties;
using NettyBaseReloaded.Utils;
using Console = System.Console;

namespace NettyBaseReloaded.Main
{
    public class ConsoleMonitor
    {
        public static void Update(bool writer)
        {
            if (!Program.ServerUp) return;
            if (writer)
            {
                var oldCursorPosX = Console.CursorLeft;
                var oldCursorPosY = Console.CursorTop;

                if (oldCursorPosY > 50)
                {
                    Draw.Logo(true);
                    oldCursorPosY = 19;
                }

                var oldColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.SetCursorPosition(0, 17);
                Console.WriteLine("\r Version = {0} // Errors = {1} / Online = {2}", Program.GetVersion(),
                    -1,
                    "W:" + World.StorageManager.GameSessions.Count + "/C:" +
                    Chat.Chat.StorageManager.ChatSessions.Count);
                Console.SetCursorPosition(oldCursorPosX, oldCursorPosY);
                Console.ForegroundColor = oldColor;
            }

            Console.Title = Global.State + " ";
            Console.Title += Program.GetVersion() + " // " + World.StorageManager.GameSessions.Count + " - " + (DateTime.Now - Server.RUNTIME).ToString(@"dd\.hh\:mm\:ss");
        }
    }
}