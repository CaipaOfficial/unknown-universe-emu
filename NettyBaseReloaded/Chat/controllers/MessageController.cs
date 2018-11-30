﻿using System;
using System.Diagnostics;
using System.Linq;
using NettyBaseReloaded.Chat.objects;
using NettyBaseReloaded.Chat.objects.chat;
using NettyBaseReloaded.Chat.packet;
using NettyBaseReloaded.Main;
using NettyBaseReloaded.Networking;

namespace NettyBaseReloaded.Chat.controllers
{
    class MessageController
    {
        public static void Send(Character character, int roomId, string message)
        {
            if (message.StartsWith("/") && character is Player player)
            {
                //throw new NotImplementedException();
                ChatCommands.Handle(player.GetSession(), message);
                return;
            }
            Room(character, roomId, message);
        }

        public static void Room(Character character, int roomId, string message)
        {
            try
            {
                var room = Chat.StorageManager.Rooms[roomId];
                if (room == null) return;

                if (character is Moderator)
                    ChatClient.SendToRoom(character,
                        "j%" + roomId + "@" + character.Name + "@" + message + "@" +
                        (int) ((Moderator) character).AdminLevel + "#", room);
                if (character is Bot)
                    throw new NotImplementedException();
                if (character is Player)
                {
                    var packet = "a%" + roomId + "@" + character.Name + "@" + message + "#";
                    if (character.Clan.Id != 0)
                    {
                        packet = packet.Replace("#", "@" + character.Clan.Tag + "#");
                    }

                    ChatClient.SendToRoom(character,
                        packet, room);
                }
                Chat.DatabaseManager.InsertChatLog(character, roomId, message, MessageType.CHAT);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error sending chat message, " + e.Message);
            }
        }

        public static void System(Player player, string message)
        {
            var session = player.GetSession();
            if (session != null)
            {
                Packet.Builder.SystemMessage(session, message);
            }
        }

        public static void SystemMessageToAll(string message)
        {
            foreach (var chatSession in Chat.StorageManager.ChatSessions)
            {
                Packet.Builder.SystemMessage(chatSession.Value, message);
            }
        }
        
        public static void Whisper(string target, string from, string msg)
        {
            var foundTarget = Chat.StorageManager.ChatSessions.FirstOrDefault(x => x.Value.Player.Name == target).Value;
            var foundAuthor = Chat.StorageManager.ChatSessions.FirstOrDefault(x => x.Value.Player.Name == from).Value;
            if (foundTarget == null)
            {
                Packet.Builder.UserNotExist(foundAuthor);
                return;
            }
            if (foundTarget.Player.Name == from)
            {
                return;
            }
            Packet.Builder.Whisper(foundTarget, from, msg);
            Packet.Builder.WhisperTo(foundAuthor, target, msg);
            Debug.WriteLine($"CHAT: Whisper from {from} - {msg} to {target}");
        }
    }
}