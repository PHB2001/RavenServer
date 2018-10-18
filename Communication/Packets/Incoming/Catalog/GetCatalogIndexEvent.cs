using System;
using Raven.Communication.Packets.Incoming;

using Raven.HabboHotel.GameClients;
using Raven.Communication.Packets.Outgoing.Catalog;
using Raven.Communication.Packets.Outgoing.BuildersClub;

namespace Raven.Communication.Packets.Incoming.Catalog
{
    public class GetCatalogIndexEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {

            Session.SendMessage(new CatalogIndexComposer(Session, RavenEnvironment.GetGame().GetCatalog().GetPages(), "NORMAL"));
            Session.SendMessage(new CatalogIndexComposer(Session, RavenEnvironment.GetGame().GetCatalog().GetBCPages(), "BUILDERS_CLUB"));

            Session.SendMessage(new CatalogItemDiscountComposer());
            Session.SendMessage(new BCBorrowedItemsComposer());
        }
    }
}