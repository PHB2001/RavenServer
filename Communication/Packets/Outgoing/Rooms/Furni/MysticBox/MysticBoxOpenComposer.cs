using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Raven.HabboHotel.Items.Crafting;

namespace Raven.Communication.Packets.Outgoing.Rooms.Furni
{
    class MysticBoxOpenComposer : ServerPacket
    {
        public MysticBoxOpenComposer()
            : base(ServerPacketHeader.MysticBoxOpenComposer)
        {
        }
    }
}