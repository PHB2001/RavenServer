using Raven.Communication.Packets.Outgoing.Catalog;
using Raven.HabboHotel.GameClients;
using Raven.HabboHotel.Rooms.AI;
using Raven.Communication.Packets.Incoming;
using Raven.HabboHotel.Items;
using Raven.Communication.Packets.Outgoing.Inventory.Furni;
using Raven.Database.Interfaces;

namespace Raven.Communication.Packets.Incoming.Catalog
{
    public class RedeemHCGiftEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            string item = Packet.PopString();

            ItemData gift = RavenEnvironment.GetGame().GetItemManager().GetItemByName(item);

            Session.GetHabbo().GetInventoryComponent().AddNewItem(0, gift.Id, "", 0, true, false, 0, 0);
            Session.SendMessage(new FurniListUpdateComposer());
            Session.GetHabbo().GetInventoryComponent().UpdateItems(true);

            Session.GetHabbo().GetStats().vipGifts--;

            using (IQueryAdapter dbClient = RavenEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunQuery("UPDATE `user_stats` SET `vip_gifts` = '" + Session.GetHabbo().GetStats().vipGifts + "' WHERE `id` = '" + Session.GetHabbo().Id + "' LIMIT 1");
            }
        }
    }
}