using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.HabboHotel.GameClients;
using Raven.Communication.Packets.Incoming;
using Raven.Communication.Packets.Outgoing.Navigator;
using Raven.HabboHotel.Navigator;

namespace Raven.Communication.Packets.Incoming.Navigator
{
    class GetNavigatorFlatsEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            ICollection<SearchResultList> Categories = RavenEnvironment.GetGame().GetNavigator().GetEventCategories();

            Session.SendMessage(new NavigatorFlatCatsComposer(Categories, Session.GetHabbo().Rank));
        }
    }
}