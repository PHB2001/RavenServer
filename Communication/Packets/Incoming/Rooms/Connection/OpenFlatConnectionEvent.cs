using System;

using Raven.Communication.Packets.Incoming;
using Raven.HabboHotel.GameClients;
using Raven.Communication.Packets.Outgoing;
using Raven.Communication.Packets.Outgoing.Nux;
using Raven.Communication.Packets.Outgoing.Rooms.Notifications;

namespace Raven.Communication.Packets.Incoming.Rooms.Connection
{
    public class OpenFlatConnectionEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null)
                return;

            int RoomId = Packet.PopInt();
            string Password = Packet.PopString();

            if (Session.GetHabbo().Rank > 3 && !Session.GetHabbo().StaffOk)
                Session.SendMessage(new RoomCustomizedAlertComposer("No te has autentificado como Staff del hotel."));

            Session.GetHabbo().PrepareRoom(RoomId, Password);
            
        }
    }
}