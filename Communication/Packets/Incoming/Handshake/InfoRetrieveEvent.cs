using System;

using Raven.Communication.Packets.Incoming;
using Raven.HabboHotel.Groups;
using Raven.HabboHotel.GameClients;
using Raven.Communication.Packets.Outgoing.Handshake;

namespace Raven.Communication.Packets.Incoming.Handshake
{
    public class InfoRetrieveEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new UserObjectComposer(Session.GetHabbo()));
            Session.SendMessage(new UserPerksComposer(Session.GetHabbo()));
        }
    }
}