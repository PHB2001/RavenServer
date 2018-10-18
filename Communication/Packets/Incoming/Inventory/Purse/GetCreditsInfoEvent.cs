using System;
using System.Linq;
using System.Text;

using Raven.Communication.Packets.Incoming;
using Raven.HabboHotel.GameClients;
using Raven.Communication.Packets.Outgoing.Inventory.Purse;

namespace Raven.Communication.Packets.Incoming.Inventory.Purse
{
    class GetCreditsInfoEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new CreditBalanceComposer(Session.GetHabbo().Credits));
            Session.SendMessage(new ActivityPointsComposer(Session.GetHabbo().Duckets, Session.GetHabbo().Diamonds, Session.GetHabbo().GOTWPoints));
        }
    }
}
