using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Raven.HabboHotel.Users;
using Raven.HabboHotel.Groups;
using Raven.Communication.Packets.Outgoing.Users;
using Raven.Database.Interfaces;
using Raven.Communication.Packets.Outgoing.Rooms.Session;

namespace Raven.Communication.Packets.Incoming.Users
{
    class OpenPlayerProfileEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int userID = Packet.PopInt();
            Boolean IsMe = Packet.PopBoolean();

            Habbo targetData = RavenEnvironment.GetHabboById(userID);
            if (targetData == null)
            {
                Session.SendNotification("Se produjo un error mientras se encontraba el perfil de ese usuario .");
                return;
            }
            
            List<Group> Groups = RavenEnvironment.GetGame().GetGroupManager().GetGroupsForUser(targetData.Id);
            
            int friendCount = 0;
            using (IQueryAdapter dbClient = RavenEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT COUNT(0) FROM `messenger_friendships` WHERE (`user_one_id` = @userid OR `user_two_id` = @userid)");
                dbClient.AddParameter("userid", userID);
                friendCount = dbClient.getInteger();
            }

            Session.SendMessage(new ProfileInformationComposer(targetData, Session, Groups, friendCount));
        }
    }
}
