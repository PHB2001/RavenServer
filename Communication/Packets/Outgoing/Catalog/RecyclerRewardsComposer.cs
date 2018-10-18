using System;
using System.Collections.Generic;

using Raven.HabboHotel.Catalog;

namespace Raven.Communication.Packets.Outgoing.Catalog
{
    public class RecyclerRewardsComposer : ServerPacket
    {
        public RecyclerRewardsComposer()
            : base(ServerPacketHeader.RecyclerRewardsMessageComposer)
        {
            base.WriteInteger(0);// Count of items
        }
    }
}