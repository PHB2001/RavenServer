using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Raven.HabboHotel.Users;
using Raven.HabboHotel.Users.Messenger;
using Raven.HabboHotel.Cache;

namespace Raven.Communication.Packets.Outgoing.Messenger
{
    class BuddyRequestsComposer : ServerPacket
    {
        public BuddyRequestsComposer(ICollection<MessengerRequest> Requests)
            : base(ServerPacketHeader.BuddyRequestsMessageComposer)
        {
            base.WriteInteger(Requests.Count);
            base.WriteInteger(Requests.Count);

            foreach (MessengerRequest Request in Requests)
            {
                base.WriteInteger(Request.From);
               base.WriteString(Request.Username);

                UserCache User = RavenEnvironment.GetGame().GetCacheManager().GenerateUser(Request.From);
               base.WriteString(User != null ? User.Look : "");
            }
        }
    }
}
