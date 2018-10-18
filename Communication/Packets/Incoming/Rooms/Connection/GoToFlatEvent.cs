using System;
using System.Linq;
using System.Text;

using Raven.Communication.Packets.Incoming;
using Raven.HabboHotel.GameClients;
using Raven.Communication.Packets.Outgoing.Rooms.Session;
using Raven.Communication.Packets.Outgoing;
using Raven.Communication.Packets.Outgoing.Nux;

namespace Raven.Communication.Packets.Incoming.Rooms.Connection
{
    class GoToFlatEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            if (!Session.GetHabbo().InRoom)
                return;

            if (!Session.GetHabbo().EnterRoom(Session.GetHabbo().CurrentRoom))
                Session.SendMessage(new CloseConnectionComposer());
        }
    }
}
