﻿using System;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.netty.packet;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Networking;

namespace NettyBaseReloaded.Game.controllers
{
    class MovementController
    {
        // TODO: Send local movement sent with MoveHero Command in order to *remove* lag
        public static void Move(Character character, Vector destination)
        {
            var player = character as Player;
            if (player != null) if (player.Controller.Jumping) return;

            //Gets the movement time
            character.MovementTime = GetTime(character, destination);

            //Gets the system time when the movement starts
            character.MovementStartTime = DateTime.Now;
            character.Moving = true;

            //sends the movement to the rest of the players in range if both are on the same map
            GameClient.SendToSpacemap(character.Spacemap, netty.commands.new_client.MoveCommand.write(character.Id, destination.X, destination.Y, character.MovementTime));
            GameClient.SendToSpacemap(character.Spacemap, netty.commands.old_client.MoveCommand.write(character.Id, destination.X, destination.Y, character.MovementTime));

            //but the this is fucked up what? the way I send commands? yap yeah its only for range / selected commands the rest I send thru packet builder
            //alright but you're movement controller class looks good, well done =3 I did some hard work on this emulator even the objects
            // how is it going w/ UberOrbit?
            // i'm not working on uo anymore oh :c btw take a quick look on my objects
        }

        public static int GetTime(Character character, Vector destination)
        {
            //Sets the position before the movement
            character.OldPosition = ActualPosition(character);

            //And the destination position
            var destinationPosition = destination;
            character.Destination = destinationPosition;

            //Same with the direction, will be used to calculate the position
            character.Direction = new Vector(destinationPosition.X - character.OldPosition.X, destinationPosition.Y - character.OldPosition.Y);

            var distance = destinationPosition.DistanceTo(character.OldPosition);

            var time = Math.Round(distance / character.Speed * 1000);

            //Console.WriteLine("Character: {0} => Position = {1} , Destination = {2} , Distance = {3} , Speed = {4} , Time = {5}", character.Name, character.Position, character.Destination, distance, character.Speed, time);

            return (int)time;
        }

        public static Vector ActualPosition(Character character)
        {
            Vector actualPosition;

            if (character.Moving)
            {
                var timeElapsed = (DateTime.Now - character.MovementStartTime).TotalMilliseconds;

                //if the character continues moving
                if (timeElapsed < character.MovementTime)
                {
                    //maths time, it returns the actual position while flying
                    actualPosition = new Vector((int)Math.Round(character.OldPosition.X + (character.Direction.X * (timeElapsed / character.MovementTime))),
                            (int)Math.Round(character.OldPosition.Y + (character.Direction.Y * (timeElapsed / character.MovementTime))));
                }
                else
                {
                    //the character should be on the destination position
                    character.Moving = false;
                    actualPosition = character.Destination;
                }
            }
            else
            {
                //the character is not moving
                actualPosition = character.Position;
            }

            //updates the actual position into the character
            character.Position = actualPosition;
            return actualPosition;
        }
    }
}