using Raven.Communication.Packets.Outgoing.Catalog;
using Raven.HabboHotel.GameClients;
using Raven.Communication.Packets.Incoming;

namespace Raven.Communication.Packets.Incoming.Catalog
{
    public class GetHCCatalogGiftsEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new ClubGiftsComposer(Session));
        }
    }
}