﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty.commands.old_client.requests;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Game.objects.world.players.ammo;

namespace NettyBaseReloaded.Game.netty.handlers
{
    class HellstormSelectRocketHandler : IHandler
    {
        public void execute(GameSession gameSession, byte[] bytes)
        {
            var request = new HellstormSelectRocketRequest();
            request.readCommand(bytes);
            gameSession.Player.RocketLauncher?.ChangeLoad(Converter.AmmoTypeToString(request.rocketType.type));
        }
    }
}