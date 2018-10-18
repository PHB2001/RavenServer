using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace Raven.Communication.Packets.Incoming.Quests
{
    class CancelQuestEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            RavenEnvironment.GetGame().GetQuestManager().CancelQuest(Session, Packet);
        }
    }
}
