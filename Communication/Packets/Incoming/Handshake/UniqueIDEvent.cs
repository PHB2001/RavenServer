using System;
using Raven.Database.Interfaces;
using Raven.HabboHotel.GameClients;
using Raven.Communication.Packets.Outgoing.Handshake;

namespace Raven.Communication.Packets.Incoming.Handshake
{
    public class UniqueIDEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            string Junk = Packet.PopString();
            string MachineId = Packet.PopString();

            Session.MachineId = MachineId;

            Session.SendMessage(new SetUniqueIdComposer(MachineId));
        }
    }
}