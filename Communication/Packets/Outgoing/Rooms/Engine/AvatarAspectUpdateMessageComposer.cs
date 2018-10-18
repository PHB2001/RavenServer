using System;
using System.Linq;
using System.Text;

using Raven.HabboHotel.Rooms;
using Raven.HabboHotel.GameClients;

namespace Raven.Communication.Packets.Outgoing.Rooms.Engine
{
    class AvatarAspectUpdateMessageComposer : ServerPacket
    {
        public AvatarAspectUpdateMessageComposer(string Figure, string Gender)
            : base(ServerPacketHeader.AvatarAspectUpdateMessageComposer)
        {
            base.WriteString(Figure);
            base.WriteString(Gender);

        }
    }
}