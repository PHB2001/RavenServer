﻿using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Raven.HabboHotel.GameClients;

namespace Raven.Communication.Packets.Outgoing.Rooms.Furni
{
    class MysteryBoxDataComposer : ServerPacket
    {
        public MysteryBoxDataComposer(GameClient Session)
            : base(ServerPacketHeader.MysteryBoxDataComposer)
        {
            foreach (string box in Session.GetHabbo().MysticBoxes.ToArray())
            {
                base.WriteString(box);
            }
            foreach (string key in Session.GetHabbo().MysticKeys.ToArray())
            {
                base.WriteString(key);
            }
        }
    }
}
