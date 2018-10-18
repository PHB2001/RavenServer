using Raven.Communication.Packets.Outgoing;
using Raven.HabboHotel.GameClients;

namespace Raven.Communication.Packets.Incoming.Rooms.Camera
{
    public class RenderRoomMessageComposer : ServerPacket
    {
        public RenderRoomMessageComposer()
            : base(ServerPacketHeader.TakedRoomPhoto)
        {

        }
    }

    public class RenderRoomMessageComposerEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket paket)
        {
            Session.SendMessage(new RenderRoomMessageComposer());
        }
    }
}