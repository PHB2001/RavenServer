using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.HabboHotel.GameClients;
using Raven.Communication.Packets.Outgoing.Catalog;

namespace Raven.Communication.Packets.Incoming.Catalog
{
    class GetClubGiftsEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new ClubGiftsComposer(Session));
        }
    }
}
