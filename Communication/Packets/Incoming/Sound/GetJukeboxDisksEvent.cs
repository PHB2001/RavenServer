﻿using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Raven.Communication.Packets.Outgoing.Rooms.Music;

namespace Raven.Communication.Packets.Incoming.Rooms.Music
{
    class GetJukeboxDisksEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session != null && (Session.GetHabbo() != null))
            {
                Session.SendMessage(new GetJukeboxDisksComposer(Session.GetHabbo().GetInventoryComponent().songDisks));
            }
        }
    }
}
