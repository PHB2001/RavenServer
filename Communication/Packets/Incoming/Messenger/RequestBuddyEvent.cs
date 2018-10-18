using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Raven.HabboHotel.Quests;

namespace Raven.Communication.Packets.Incoming.Messenger
{
    class RequestBuddyEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null || Session.GetHabbo().GetMessenger() == null)
                return;

            if (Session.GetHabbo().GetMessenger().RequestBuddy(Packet.PopString()))
                RavenEnvironment.GetGame().GetQuestManager().ProgressUserQuest(Session, QuestType.SOCIAL_FRIEND);
        }
    }
}
