using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Raven.HabboHotel.GameClients;

namespace Raven.Communication.Packets.Outgoing.Users
{
    class RespectNotificationComposer : ServerPacket
    {
        public RespectNotificationComposer(int userID, int Respect)
            : base(ServerPacketHeader.RespectNotificationMessageComposer)
        {
            base.WriteInteger(userID);
            base.WriteInteger(Respect);
        }
    }
}
