using System;
using System.Linq;
using System.Collections.Generic;

using Raven.HabboHotel.Items;
using Raven.Communication.Packets.Outgoing.Inventory.Furni;

using Raven.Communication.Packets.Outgoing.Rooms.Furni;
using Raven.HabboHotel.Items.Crafting;
using Raven.HabboHotel.Rooms;

namespace Raven.Communication.Packets.Incoming.Rooms.Furni
{
    class ExecuteCraftingRecipeEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int craftingTable = Packet.PopInt();
            string RecetaFinal = Packet.PopString();

            CraftingRecipe recipe = RavenEnvironment.GetGame().GetCraftingManager().GetRecipeByPrize(RecetaFinal);

            if (recipe == null) return;
            ItemData resultItem = RavenEnvironment.GetGame().GetItemManager().GetItemByName(recipe.Result);
            if (resultItem == null) return;
            bool success = true;
            foreach (var need in recipe.ItemsNeeded)
            {
                for (var i = 1; i <= need.Value; i++)
                {
                    ItemData item = RavenEnvironment.GetGame().GetItemManager().GetItemByName(need.Key);
                    if (item == null)
                    {
                        success = false;
                        continue;
                    }

                    var inv = Session.GetHabbo().GetInventoryComponent().GetFirstItemByBaseId(item.Id);
                    if (inv == null)
                    {
                        success = false;
                        continue;
                    }

                    using (var dbClient = RavenEnvironment.GetDatabaseManager().GetQueryReactor()) dbClient.RunQuery("DELETE FROM `items` WHERE `id` = '" + inv.Id + "' AND `user_id` = '" + Session.GetHabbo().Id + "' LIMIT 1");
                    Session.GetHabbo().GetInventoryComponent().RemoveItem(inv.Id);
                }
            }

            Session.GetHabbo().GetInventoryComponent().UpdateItems(true);

            if (success)
            {
                Session.GetHabbo().GetInventoryComponent().AddNewItem(0, resultItem.Id, "", 0, true, false, 0, 0);
                Session.GetHabbo().GetInventoryComponent().UpdateItems(true);
                Session.SendMessage(new FurniListUpdateComposer());

                switch (recipe.Type)
                {
                    case 1:
                        RavenEnvironment.GetGame().GetAchievementManager().ProgressAchievement(Session, "ACH_CrystalCracker", 1);
                        break;

                    case 2:
                        RavenEnvironment.GetGame().GetAchievementManager().ProgressAchievement(Session, "ACH_PetLover", 1);
                        break;

                    case 3:
                        RavenEnvironment.GetGame().GetAchievementManager().ProgressAchievement(Session, "ACH_PetLover", 1);
                        break;
                }
            }

            Session.SendMessage(new CraftingResultComposer(recipe, success));

            Room room = Session.GetHabbo().CurrentRoom;
            Item table = room.GetRoomItemHandler().GetItem(craftingTable);

            Session.SendMessage(new CraftableProductsComposer(table));
            return;
        }
    }
}