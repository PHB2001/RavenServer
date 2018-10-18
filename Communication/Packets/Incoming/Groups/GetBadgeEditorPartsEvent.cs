using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Raven.Communication.Packets.Outgoing.Groups;

namespace Raven.Communication.Packets.Incoming.Groups
{
    class GetBadgeEditorPartsEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new BadgeEditorPartsComposer(
                RavenEnvironment.GetGame().GetGroupManager().Bases,
                RavenEnvironment.GetGame().GetGroupManager().Symbols,
                RavenEnvironment.GetGame().GetGroupManager().BaseColours,
                RavenEnvironment.GetGame().GetGroupManager().SymbolColours,
                RavenEnvironment.GetGame().GetGroupManager().BackGroundColours));

        }
    }
}
