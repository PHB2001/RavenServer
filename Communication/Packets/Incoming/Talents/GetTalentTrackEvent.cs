using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Raven.Communication.Packets.Outgoing.Talents;
using Raven.HabboHotel.Achievements;

namespace Raven.Communication.Packets.Incoming.Talents
{
    class GetTalentTrackEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            string Type = Packet.PopString();

            List<Talent> talents = RavenEnvironment.GetGame().GetTalentManager().GetTalents(Type, -1);

            if (talents == null)
                return;

            Session.SendMessage(new TalentTrackComposer(Session, Type, talents));
        }
    }
}