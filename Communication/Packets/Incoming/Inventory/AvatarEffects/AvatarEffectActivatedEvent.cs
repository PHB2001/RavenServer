using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Raven.HabboHotel.Users.Effects;
using Raven.Communication.Packets.Outgoing.Inventory.AvatarEffects;

namespace Raven.Communication.Packets.Incoming.Inventory.AvatarEffects
{
    class AvatarEffectActivatedEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int EffectId = Packet.PopInt();

            AvatarEffect Effect = Session.GetHabbo().Effects().GetEffectNullable(EffectId, false, true);

            if (Effect == null || Session.GetHabbo().Effects().HasEffect(EffectId, true))
            {
                return;
            }

            if (Effect.Activate())
            {
                Session.SendMessage(new AvatarEffectActivatedComposer(Effect));
            }
        }
    }
}
