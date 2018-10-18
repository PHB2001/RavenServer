using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Raven.HabboHotel.GameClients;
using Raven.Communication.Packets.Outgoing.Help;

namespace Raven.Communication.Packets.Incoming.Help
{
    class GetSanctionStatusEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new SanctionStatusComposer());
        }
    }
}
