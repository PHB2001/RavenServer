using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Raven.Communication.Packets.Outgoing.Rooms.Furni.RentableSpaces;
using Raven.HabboHotel.Rooms;
using Raven.HabboHotel.Items;
using Raven.HabboHotel.Items.RentableSpaces;

namespace Raven.Communication.Packets.Incoming.Rooms.Furni.RentableSpaces
{
    class BuyRentableSpaceEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {

            int itemId = Packet.PopInt();

            Room room;
            if (!RavenEnvironment.GetGame().GetRoomManager().TryGetRoom(Session.GetHabbo().CurrentRoomId, out room))
                return;

            if (room == null || room.GetRoomItemHandler() == null)
                return;

            RentableSpaceItem rsi;
            if (RavenEnvironment.GetGame().GetRentableSpaceManager().GetRentableSpaceItem(itemId, out rsi))
            {
                RavenEnvironment.GetGame().GetRentableSpaceManager().ConfirmBuy(Session, rsi, 3600);
            }


        }
    }
}