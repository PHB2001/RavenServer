using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Raven.HabboHotel.Users;
using Raven.HabboHotel.Cache;

namespace Raven.Communication.Packets.Outgoing.Messenger
{
    class NewBuddyRequestComposer : ServerPacket
    {
        public NewBuddyRequestComposer(UserCache Habbo)
            : base(ServerPacketHeader.NewBuddyRequestMessageComposer)
        {
           base.WriteInteger(Habbo.Id);
           base.WriteString(Habbo.Username);
           base.WriteString(Habbo.Look);
        }
    }
}
