using System;
using Raven.Communication.Packets.Outgoing.Rooms.Furni;
using Raven.HabboHotel.Items.Crafting;

namespace Raven.Communication.Packets.Incoming.Rooms.Furni
{
    class SetCraftingRecipeEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            var result = Packet.PopString();

            CraftingRecipe recipe = null;
            foreach (CraftingRecipe Receta in RavenEnvironment.GetGame().GetCraftingManager().CraftingRecipes.Values)
            {
                if (Receta.Result.Contains(result))
                {
                    recipe = Receta;
                    break;
                }
            }

            var Final = RavenEnvironment.GetGame().GetCraftingManager().GetRecipe(recipe.Id);
            if (Final == null) return;
            Session.SendMessage(new CraftingRecipeComposer(Final));
        }

    }
}