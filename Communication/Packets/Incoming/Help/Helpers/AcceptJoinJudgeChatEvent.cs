using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.HabboHotel.GameClients;
using Raven.HabboHotel.Helpers;
using Raven.Communication.Packets.Outgoing.Help;
using Raven.Communication.Packets.Outgoing;

namespace Raven.Communication.Packets.Incoming.Help.Helpers
{
    class AcceptJoinJudgeChatEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            //bool request = Packet.PopBoolean();

            //switch(request)
            //{
            //    case true:
            //        var response = new ServerPacket(ServerPacketHeader.GuardianSendChatCaseMessageComposer);
            //        response.WriteInteger(60);
            //        response.WriteString("");
            //        Session.SendMessage(response);
            //        break;
            //    case false:

            //        break;
            //}
        }
    }
}
