using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Raven.HabboHotel.Catalog;
using Raven.Communication.Packets.Outgoing.Catalog;
using Raven.Communication.Packets.Outgoing.BuildersClub;
using Raven.HabboHotel.GameClients;
using Raven.Communication.Packets.Outgoing.Rooms.Camera;

namespace Raven.Communication.Packets.Incoming.Catalog
{
    class GetCameraPriceEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new CameraPriceComposer(1, 1, 0));
        }
    }
}
