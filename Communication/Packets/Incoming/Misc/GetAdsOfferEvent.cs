using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Raven.HabboHotel.Users;
using Raven.Communication.Packets.Outgoing.Rooms.Notifications;
using Raven.HabboHotel.Rooms;
using Raven.HabboHotel.GameClients;
using Raven.Communication.Packets.Outgoing.Handshake;

namespace Raven.Communication.Packets.Incoming.Misc
{
    class GetAdsOfferEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new VideoOffersRewardsComposer());
        }
    }
}
