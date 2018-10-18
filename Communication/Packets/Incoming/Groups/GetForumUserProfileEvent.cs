using System.Collections.Generic;

using Raven.HabboHotel.Users;
using Raven.HabboHotel.Groups;
using Raven.HabboHotel.GameClients;

using Raven.Database.Interfaces;
using Raven.Communication.Packets.Outgoing.Users;

namespace Raven.Communication.Packets.Incoming.Groups.Forums
{
    class GetForumUserProfileEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            string username = Packet.PopString();

            Habbo targetData = RavenEnvironment.GetHabboByUsername(username);
            if (targetData == null)
            {
                Session.SendNotification("Ha ocurrido un error buscando el perfil del usuario.");
                return;
            }

            List<Group> groups = RavenEnvironment.GetGame().GetGroupManager().GetGroupsForUser(targetData.Id);

            int friendCount = 0;
            using (IQueryAdapter dbClient = RavenEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT COUNT(0) FROM `messenger_friendships` WHERE (`user_one_id` = @userid OR `user_two_id` = @userid)");
                dbClient.AddParameter("userid", targetData.Id);
                friendCount = dbClient.getInteger();
            }

            Session.SendMessage(new ProfileInformationComposer(targetData, Session, groups, friendCount));
        }
    }
}
