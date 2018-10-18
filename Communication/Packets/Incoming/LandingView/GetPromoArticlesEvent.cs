using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Raven.HabboHotel.LandingView;
using Raven.HabboHotel.LandingView.Promotions;
using Raven.Communication.Packets.Outgoing.LandingView;

namespace Raven.Communication.Packets.Incoming.LandingView
{
    class GetPromoArticlesEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            ICollection<Promotion> LandingPromotions = RavenEnvironment.GetGame().GetLandingManager().GetPromotionItems();

            Session.SendMessage(new PromoArticlesComposer(LandingPromotions));
        }
    }
}
