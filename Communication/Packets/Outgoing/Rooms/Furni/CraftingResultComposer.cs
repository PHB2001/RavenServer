using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Raven.HabboHotel.Items.Crafting;

namespace Raven.Communication.Packets.Outgoing.Rooms.Furni
{
    class CraftingResultComposer : ServerPacket
    {
        public CraftingResultComposer(CraftingRecipe recipe, bool success)
            : base(ServerPacketHeader.CraftingResultMessageComposer)
        {
            base.WriteBoolean(success);
            base.WriteString(recipe.Result);
            base.WriteString(recipe.Result);
        }
    }
}