﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client.requests
{
    class HarvestRequest
    {
        public const short ID = 27140;

        public string itemHash;

        public void readCommand(byte[] bytes)
        {
            var parse = new ByteParser(bytes);
            itemHash = parse.readUTF();
        }
    }
}
