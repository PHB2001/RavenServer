using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Raven.Communication.Packets.Outgoing.Rooms.Notifications;
using Raven.HabboHotel.Global;
using Raven.HabboHotel.Catalog;
using Raven.Communication.Packets.Outgoing;

namespace Raven.Communication.Packets.Incoming.Inventory.Purse
{
   class GetHabboClubWindowEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int _page = 5;

            if (Session.GetHabbo().lastLayout.Equals("loyalty_vip_buy"))
                _page = int.Parse(RavenEnvironment.GetDBConfig().DBData["catalog.hcbuy.id"]);

            CatalogPage page = null;
            if (!RavenEnvironment.GetGame().GetCatalog().TryGetPage(_page, out page))
            
                return;

            ServerPacket Message = new ServerPacket(ServerPacketHeader.GetClubComposer);
            Message.WriteInteger(page.Items.Values.Count);

            foreach (CatalogItem catalogItem in page.Items.Values)
            {
                catalogItem.SerializeClub(Message, Session);
            }

            Message.WriteInteger(Packet.PopInt());

            Session.SendMessage(Message);
        }
    }
}
