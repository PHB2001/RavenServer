using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Raven.Communication.Packets.Outgoing.Catalog;

namespace Raven.Communication.Packets.Incoming.Catalog
{
    class GetCatalogRoomPromotionEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new GetCatalogRoomPromotionComposer(Session.GetHabbo().UsersRooms));
        }
    }
}
