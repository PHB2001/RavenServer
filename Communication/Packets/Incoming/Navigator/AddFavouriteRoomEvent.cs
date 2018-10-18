
using Raven.Communication.Packets.Outgoing.Navigator;

using Raven.Database.Interfaces;
using Raven.HabboHotel.GameClients;
using Raven.HabboHotel.Rooms;
using Raven.HabboHotel.Users;
using Raven.Communication.Packets.Incoming;

namespace Raven.Communication.Packets.Incoming.Navigator
{
    public class AddFavouriteRoomEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            if (Session == null)
                return;

            int RoomId = Packet.PopInt();

            RoomData Data = RavenEnvironment.GetGame().GetRoomManager().GenerateRoomData(RoomId);

            if (Data == null || Session.GetHabbo().FavoriteRooms.Count >= 30 || Session.GetHabbo().FavoriteRooms.Contains(RoomId))
            {
                // send packet that favourites is full.
                return;
            }

            Session.GetHabbo().FavoriteRooms.Add(RoomId);
            Session.SendMessage(new UpdateFavouriteRoomComposer(RoomId, true));

            using (IQueryAdapter dbClient = RavenEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunQuery("INSERT INTO user_favorites (user_id,room_id) VALUES (" + Session.GetHabbo().Id + "," + RoomId + ")");
            }
        }
    }
}