﻿using Raven.Communication.Packets.Outgoing;
using Raven.HabboHotel.GameClients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Communication.Packets.Outgoing.Users
{
    class ClubGiftRecievedComposer : ServerPacket
    {
        public ClubGiftRecievedComposer(GameClient Session) : base(ServerPacketHeader.ClubGiftRecievedComposer)
        {
            base.WriteString("PENE");
            base.WriteInteger(1);
            base.WriteString("b"); // tipo de furni
            base.WriteString("ADMIN"); // nombre
        }
    }
}