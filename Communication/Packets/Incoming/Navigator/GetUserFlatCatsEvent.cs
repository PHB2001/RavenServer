using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Raven.Communication.Packets.Incoming;
using Raven.HabboHotel.Users;
using Raven.HabboHotel.Navigator;
using Raven.HabboHotel.GameClients;
using Raven.Communication.Packets.Outgoing.Navigator;

namespace Raven.Communication.Packets.Incoming.Navigator
{
    public class GetUserFlatCatsEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            if (Session == null)
                return;

            ICollection<SearchResultList> Categories = RavenEnvironment.GetGame().GetNavigator().GetFlatCategories();

            Session.SendMessage(new UserFlatCatsComposer(Categories, Session.GetHabbo().Rank));
        }
    }
}