using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Raven.Communication.Packets.Outgoing.Rooms.Notifications;
using Raven.HabboHotel.Global;
using Plus.Communication.Packets.Outgoing.Inventory.Purse;

namespace Raven.Communication.Packets.Incoming.Inventory.Purse
{
    class GetHabboClubCenterInfoMessageEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new GetHabboClubCenterInfoMessageComposer(Session));
        }
    }
}