using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Raven.Communication.Packets.Incoming;
using Raven.HabboHotel.GameClients;
using Raven.Communication.Packets.Outgoing.Sound;
using Raven.Database.Interfaces;


namespace Raven.Communication.Packets.Incoming.Users
{
    class SetChatPreferenceEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            Boolean ChatPreference = Packet.PopBoolean();

            Session.GetHabbo().ChatPreference = ChatPreference;
            using (IQueryAdapter dbClient = RavenEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("UPDATE `users` SET `chat_preference` = @chatPreference WHERE `id` = '" + Session.GetHabbo().Id + "' LIMIT 1");
                dbClient.AddParameter("chatPreference", RavenEnvironment.BoolToEnum(ChatPreference));
                dbClient.RunQuery();
            }
        }
    }
}
