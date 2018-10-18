
using Raven.Communication.Packets.Outgoing.Navigator;

using Raven.Database.Interfaces;
using Raven.HabboHotel.GameClients;
using Raven.HabboHotel.Users;
using Raven.Communication.Packets.Incoming;

namespace Raven.Communication.Packets.Incoming.Navigator
{
    public class RemoveFavouriteRoomEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            int Id = Packet.PopInt();

            Session.GetHabbo().FavoriteRooms.Remove(Id);
            Session.SendMessage(new UpdateFavouriteRoomComposer(Id, false));

            using (IQueryAdapter dbClient = RavenEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunQuery("DELETE FROM user_favorites WHERE user_id = " + Session.GetHabbo().Id + " AND room_id = " + Id + " LIMIT 1");
            }
        }
    }
}