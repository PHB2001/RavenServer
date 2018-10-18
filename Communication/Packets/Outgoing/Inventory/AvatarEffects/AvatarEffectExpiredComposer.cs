using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Raven.HabboHotel.Users.Effects;

namespace Raven.Communication.Packets.Outgoing.Inventory.AvatarEffects
{
    class AvatarEffectExpiredComposer : ServerPacket
    {
        public AvatarEffectExpiredComposer(AvatarEffect Effect)
            : base(ServerPacketHeader.AvatarEffectExpiredMessageComposer)
        {
            base.WriteInteger(Effect.SpriteId);
        }
    }
}
