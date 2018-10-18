using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Raven.Communication.Packets.Incoming;
using Raven.HabboHotel.Rooms;
using Raven.HabboHotel.GameClients;
using Raven.Communication.Packets.Outgoing.Catalog;

namespace Raven.Communication.Packets.Incoming.Catalog
{
    class GetPromotableRoomsEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            List<RoomData> Rooms = Session.GetHabbo().UsersRooms;
            Rooms = Rooms.Where(x => (x.Promotion == null || x.Promotion.TimestampExpires < RavenEnvironment.GetUnixTimestamp())).ToList();
            Session.SendMessage(new PromotableRoomsComposer(Rooms));
        }
    }
}
