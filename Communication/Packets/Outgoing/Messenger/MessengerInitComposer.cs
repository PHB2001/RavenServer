using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Raven.HabboHotel.Users;
using Raven.HabboHotel.Users.Messenger;
using Raven.HabboHotel.Users.Relationships;

namespace Raven.Communication.Packets.Outgoing.Messenger
{
    class MessengerInitComposer : ServerPacket
    {
        public MessengerInitComposer()
            : base(ServerPacketHeader.MessengerInitMessageComposer)
        {
            base.WriteInteger(RavenStaticGameSettings.MessengerFriendLimit);//Friends max.
            base.WriteInteger(300);
            base.WriteInteger(800);
            base.WriteInteger(0); // category count
        }
    }
}
