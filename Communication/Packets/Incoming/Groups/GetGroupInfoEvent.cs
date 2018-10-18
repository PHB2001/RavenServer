using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Raven.HabboHotel.Groups;
using Raven.Communication.Packets.Outgoing.Groups;

namespace Raven.Communication.Packets.Incoming.Groups
{
    class GetGroupInfoEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int GroupId = Packet.PopInt();
            bool NewWindow = Packet.PopBoolean();

            Group Group = null;
            if (!RavenEnvironment.GetGame().GetGroupManager().TryGetGroup(GroupId, out Group))
                return;

            Session.SendMessage(new GroupInfoComposer(Group, Session, NewWindow));     
        }
    }
}
