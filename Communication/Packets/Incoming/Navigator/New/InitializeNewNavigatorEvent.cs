using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Raven.HabboHotel.Rooms;
using Raven.HabboHotel.Navigator;
using Raven.Communication.Packets.Outgoing.Navigator;

namespace Raven.Communication.Packets.Incoming.Navigator
{
    class InitializeNewNavigatorEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            ICollection<TopLevelItem> TopLevelItems = RavenEnvironment.GetGame().GetNavigator().GetTopLevelItems();
            ICollection<SearchResultList> SearchResultLists = RavenEnvironment.GetGame().GetNavigator().GetSearchResultLists();

            Session.SendMessage(new NavigatorMetaDataParserComposer(TopLevelItems));
            Session.SendMessage(new NavigatorLiftedRoomsComposer());
            Session.SendMessage(new NavigatorCollapsedCategoriesComposer());
            Session.SendMessage(new NavigatorPreferencesComposer());
        }
    }
}
