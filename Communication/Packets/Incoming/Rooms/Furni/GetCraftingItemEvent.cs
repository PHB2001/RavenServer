using System;
using System.Linq;
using System.Collections.Generic;

using Raven.HabboHotel.Items;
using Raven.Communication.Packets.Outgoing.Inventory.Furni;

using Raven.Communication.Packets.Outgoing.Rooms.Furni;
using Raven.HabboHotel.Items.Crafting;

namespace Raven.Communication.Packets.Incoming.Rooms.Furni
{
    class GetCraftingItemEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            //var result = Packet.PopString();

            //CraftingRecipe recipe = null;
            //foreach (CraftingRecipe Receta in RavenEnvironment.GetGame().GetCraftingManager().CraftingRecipes.Values)
            //{
            //    if (Receta.Result.Contains(result))
            //    {
            //        recipe = Receta;
            //        break;
            //    }
            //}

            //var Final = RavenEnvironment.GetGame().GetCraftingManager().GetRecipe(recipe.Id);

            //Session.SendMessage(new CraftingResultComposer(recipe, true));
            //Session.SendMessage(new CraftableProductsComposer());
        }
    }
}