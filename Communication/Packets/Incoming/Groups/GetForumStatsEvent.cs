using Raven.Communication.Packets.Outgoing;
using Raven.Communication.Packets.Outgoing.Groups;
using Raven.HabboHotel.GameClients;
using Raven.HabboHotel.Groups.Forums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Communication.Packets.Incoming.Groups
{
    class GetForumStatsEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            var GroupForumId = Packet.PopInt();

            GroupForum Forum;
            if (!RavenEnvironment.GetGame().GetGroupForumManager().TryGetForum(GroupForumId, out Forum))
            {
                Session.SendNotification("Opss, Forum inexistente!");
                return;
            }

            Session.SendMessage(new GetGroupForumsMessageEvent(Forum, Session));

        }
    }
}
