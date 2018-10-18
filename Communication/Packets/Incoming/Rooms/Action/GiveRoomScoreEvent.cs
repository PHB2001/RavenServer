using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Raven.HabboHotel.Rooms;
using Raven.Communication.Packets.Outgoing.Navigator;

using Raven.Database.Interfaces;


namespace Raven.Communication.Packets.Incoming.Rooms.Action
{
    class GiveRoomScoreEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (!Session.GetHabbo().InRoom)
                return;

            Room Room;

            if (!RavenEnvironment.GetGame().GetRoomManager().TryGetRoom(Session.GetHabbo().CurrentRoomId, out Room))
                return;

            if (Session.GetHabbo().RatedRooms.Contains(Room.RoomId))
                return;

            int Rating = Packet.PopInt();
            switch (Rating)
            {
                case -1:

                    Room.Score--;
                    Room.RoomData.Score--;
                    break;

                case 1:

                    Room.Score++;
                    Room.RoomData.Score++;
                    break;

                default:

                    return;
            }

          
            using (IQueryAdapter dbClient = RavenEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunQuery("UPDATE rooms SET score = '" + Room.Score + "' WHERE id = '" + Room.RoomId + "' LIMIT 1");
            }

            Session.GetHabbo().RatedRooms.Add(Room.RoomId);        
            Session.SendMessage(new RoomRatingComposer(Room.Score, !(Session.GetHabbo().RatedRooms.Contains(Room.RoomId) || Room.CheckRights(Session, true))));
        }
    }
}
