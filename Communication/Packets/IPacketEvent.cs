using Raven.Communication.Packets.Incoming;
using Raven.HabboHotel.GameClients;

namespace Raven.Communication.Packets
{
    public interface IPacketEvent
    {
        void Parse(GameClient Session, ClientPacket Packet);
    }
}