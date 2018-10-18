using Raven.HabboHotel.GameClients;
using Raven.Communication.Packets.Incoming;

namespace Raven.Communication.Packets.Incoming.Handshake
{
    public class GetClientVersionEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            string Build = Packet.PopString();

            if (RavenEnvironment.SWFRevision != Build)
                RavenEnvironment.SWFRevision = Build;
        }
    }
}