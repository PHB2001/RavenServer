﻿using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Raven.HabboHotel.Items;

namespace Raven.Communication.Packets.Outgoing.Rooms.Engine
{
    class RoomSpectatorComposer : ServerPacket
    {
        public RoomSpectatorComposer()
            : base(ServerPacketHeader.RoomSpectatorComposer)
        {
        }
    }
}
