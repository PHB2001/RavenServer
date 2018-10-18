using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Raven.Communication.Packets.Outgoing.Rooms.Furni.RentableSpaces;
using Raven.HabboHotel.Items;
using Raven.HabboHotel.Rooms;
using Raven.HabboHotel.Items.RentableSpaces;

namespace Raven.Communication.Packets.Incoming.Rooms.Furni.RentableSpaces
{
    class GetRentableSpaceEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int ItemId = Packet.PopInt();

            Room room;
            if (!RavenEnvironment.GetGame().GetRoomManager().TryGetRoom(Session.GetHabbo().CurrentRoomId, out room))
                return;

            Item item = room.GetRoomItemHandler().GetItem(ItemId);

            if (item == null)
                return;

            if (item.GetBaseItem() == null)
                return;

            if (item.GetBaseItem().InteractionType != InteractionType.RENTABLE_SPACE)
                return;

            RentableSpaceItem _rentableSpace;
            if (!RavenEnvironment.GetGame().GetRentableSpaceManager().GetRentableSpaceItem(ItemId, out _rentableSpace))
            {
                _rentableSpace = RavenEnvironment.GetGame().GetRentableSpaceManager().CreateAndAddItem(ItemId, Session);
            }

            if (_rentableSpace.Rented)
            {
                Session.SendMessage(new RentableSpaceComposer(_rentableSpace.OwnerId, _rentableSpace.OwnerUsername, _rentableSpace.GetExpireSeconds()));
            }
            else
            {
                int errorCode = RavenEnvironment.GetGame().GetRentableSpaceManager().GetButtonErrorCode(Session, _rentableSpace);
                Session.SendMessage(new RentableSpaceComposer(false, errorCode, -1, "", 0, _rentableSpace.Price));
            }
        }
    }
}