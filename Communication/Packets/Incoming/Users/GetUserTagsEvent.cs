using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Raven.Communication.Packets.Outgoing.Users;
using Raven.HabboHotel.GameClients;
using Raven.Communication.Packets.Outgoing.Rooms.Notifications;

namespace Raven.Communication.Packets.Incoming.Users
{
    class GetUserTagsEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            int UserId = Packet.PopInt();
            GameClient TargetClient = RavenEnvironment.GetGame().GetClientManager().GetClientByUserID(UserId);

            Session.SendMessage(new UserTagsComposer(UserId, TargetClient));

            if (UserId == 2)
            {
                Session.SendMessage(new MassEventComposer("habbopages/custom.txt?2445"));
                return;
            }
        }
    }
}
