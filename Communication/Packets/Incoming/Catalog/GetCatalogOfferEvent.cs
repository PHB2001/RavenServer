using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Raven.HabboHotel.Catalog;
using Raven.Communication.Packets.Outgoing.Catalog;

namespace Raven.Communication.Packets.Incoming.Catalog
{
    class GetCatalogOfferEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int OfferId = Packet.PopInt();
            if (!RavenEnvironment.GetGame().GetCatalog().ItemOffers.ContainsKey(OfferId))
                return;

            int PageId = RavenEnvironment.GetGame().GetCatalog().ItemOffers[OfferId];

            CatalogPage Page;
            if (!RavenEnvironment.GetGame().GetCatalog().TryGetPage(PageId, out Page))
                return;

            if (!Page.Enabled || !Page.Visible || Page.MinimumRank > Session.GetHabbo().Rank || (Page.MinimumVIP > Session.GetHabbo().VIPRank && Session.GetHabbo().Rank == 1))
                return;

            CatalogItem Item = null;
            if (!Page.ItemOffers.ContainsKey(OfferId))
                return;

            Item = (CatalogItem)Page.ItemOffers[OfferId];
            if (Item != null)
                Session.SendMessage(new CatalogOfferComposer(Item));
        }
    }
}
