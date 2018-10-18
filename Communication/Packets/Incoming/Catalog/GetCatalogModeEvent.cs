using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Raven.HabboHotel.Catalog;
using Raven.Communication.Packets.Outgoing.Catalog;
using Raven.Communication.Packets.Outgoing.BuildersClub;

namespace Raven.Communication.Packets.Incoming.Catalog
{
    class GetCatalogModeEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            string PageMode = Packet.PopString();

            if (PageMode == "NORMAL")
                Session.SendMessage(new CatalogIndexComposer(Session, RavenEnvironment.GetGame().GetCatalog().GetPages(), PageMode));//, Sub));
            else if (PageMode == "BUILDERS_CLUB")
                Session.SendMessage(new CatalogIndexComposer(Session, RavenEnvironment.GetGame().GetCatalog().GetBCPages(), PageMode));
        }
    }
}
