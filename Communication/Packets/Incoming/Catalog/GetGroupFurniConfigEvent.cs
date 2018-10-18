using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Raven.Communication.Packets.Outgoing.Catalog;

namespace Raven.Communication.Packets.Incoming.Catalog
{
    class GetGroupFurniConfigEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new GroupFurniConfigComposer(RavenEnvironment.GetGame().GetGroupManager().GetGroupsForUser(Session.GetHabbo().Id)));
        }
    }
}
