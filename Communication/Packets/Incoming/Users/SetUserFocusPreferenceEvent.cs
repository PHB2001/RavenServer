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
    class SetUserFocusPreferenceEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            bool FocusPreference = Packet.PopBoolean();

            Session.GetHabbo().FocusPreference = FocusPreference;
            using (IQueryAdapter dbClient = RavenEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("UPDATE `users` SET `focus_preference` = @focusPreference WHERE `id` = '" + Session.GetHabbo().Id + "' LIMIT 1");
                dbClient.AddParameter("focusPreference", RavenEnvironment.BoolToEnum(FocusPreference));
                dbClient.RunQuery();
            }
        }
    }
}
