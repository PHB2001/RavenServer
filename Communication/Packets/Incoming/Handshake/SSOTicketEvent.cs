using System;

using Raven.Communication.Packets.Incoming;
using Raven.HabboHotel.GameClients;
using Raven.Communication.Packets.Outgoing.Handshake;

namespace Raven.Communication.Packets.Incoming.Handshake

{
    public class SSOTicketEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.RC4Client == null || Session.GetHabbo() != null)
                return;

            string SSO = Packet.PopString();

            if (string.IsNullOrEmpty(SSO) /*|| SSO.Length < 15*/)
                return;

            Session.TryAuthenticate(SSO);
        }
    }
}
//{
//    public class SSOTicketEvent : IPacketEvent
//    {
//        public void Parse(GameClient Session, ClientPacket Packet)
//        {
//            if (Session == null || Session.RC4Client == null || Session.GetHabbo() != null)
//                return;

//            Session.TryAuthenticate(Packet.PopString());
//        }
//    }
//}