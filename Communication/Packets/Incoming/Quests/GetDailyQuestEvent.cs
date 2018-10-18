//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Raven.HabboHotel.GameClients;
//using Raven.Communication.Packets.Outgoing.LandingView;

//namespace Raven.Communication.Packets.Incoming.Quests
//{
//    class GetDailyQuestEvent : IPacketEvent
//    {
//        public void Parse(GameClient Session, ClientPacket Packet)
//        {
//            int UsersOnline = RavenEnvironment.GetGame().GetClientManager().Count;

//            Session.SendMessage(new ConcurrentUsersGoalProgressComposer(UsersOnline));
//        }
//    }
//}
