using System;
using Raven.Communication.Packets.Incoming;
using Raven.HabboHotel.GameClients;
using Raven.Communication.Packets.Outgoing.Users;
using Raven.Communication.Packets.Outgoing.Handshake;
using Raven.HabboHotel.Rooms;

namespace Raven.Communication.Packets.Incoming.Users
{
    class ScrGetUserInfoEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new ScrSendUserInfoComposer(Session.GetHabbo()));
            Session.SendMessage(new UserRightsComposer(Session.GetHabbo()));

        }
    }
}
