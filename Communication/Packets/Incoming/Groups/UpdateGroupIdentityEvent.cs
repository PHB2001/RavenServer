using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Raven.HabboHotel.Groups;
using Raven.Database.Interfaces;
using Raven.Communication.Packets.Outgoing.Groups;

namespace Raven.Communication.Packets.Incoming.Groups
{
    class UpdateGroupIdentityEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int GroupId = Packet.PopInt();
            string word;
            string Name = Packet.PopString();
            Name = RavenEnvironment.GetGame().GetChatManager().GetFilter().IsUnnaceptableWord(Name, out word) ? "Spam" : Name;
            string Desc = Packet.PopString();
            Desc = RavenEnvironment.GetGame().GetChatManager().GetFilter().IsUnnaceptableWord(Desc, out word) ? "Spam" : Desc;

            Group Group = null;
            if (!RavenEnvironment.GetGame().GetGroupManager().TryGetGroup(GroupId, out Group))
                return;

            if (Group.CreatorId != Session.GetHabbo().Id)
                return;

            using (IQueryAdapter dbClient = RavenEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("UPDATE `groups` SET `name`= @name, `desc` = @desc WHERE `id` = '" + GroupId + "' LIMIT 1");
                dbClient.AddParameter("name", Name);
                dbClient.AddParameter("desc", Desc);
                dbClient.RunQuery();
            }

            Group.Name = Name;
            Group.Description = Desc;

            Session.SendMessage(new GroupInfoComposer(Group, Session));
        }
    }
}
