using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Raven.Database.Interfaces;
using Raven.HabboHotel.Rooms;
using Raven.HabboHotel.Items;

namespace Raven.Communication.Packets.Incoming.Rooms.Furni.Stickys
{
    class DeleteStickyNoteEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (!Session.GetHabbo().InRoom)
                return;

            Room Room = null;
            if (!RavenEnvironment.GetGame().GetRoomManager().TryGetRoom(Session.GetHabbo().CurrentRoomId, out Room))
                return;

            if (!Room.CheckRights(Session))
                return;

            Item Item = Room.GetRoomItemHandler().GetItem(Packet.PopInt());
            if (Item == null)
                return;

            if (Item.GetBaseItem().InteractionType == InteractionType.POSTIT || Item.GetBaseItem().InteractionType == InteractionType.CAMERA_PICTURE)
            {
                Room.GetRoomItemHandler().RemoveFurniture(Session, Item.Id);
                using (IQueryAdapter dbClient = RavenEnvironment.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.RunQuery("DELETE FROM `items` WHERE `id` = '" + Item.Id + "' LIMIT 1");
                }
            }
        }
    }
}
