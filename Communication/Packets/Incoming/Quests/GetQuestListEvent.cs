using System.Collections.Generic;
using Raven.HabboHotel.GameClients;
using Raven.HabboHotel.Quests;
using Raven.Communication.Packets.Incoming;

namespace Raven.Communication.Packets.Incoming.Quests
{
    public class GetQuestListEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            RavenEnvironment.GetGame().GetQuestManager().GetList(Session, null);
        }
    }
}