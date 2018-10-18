using Raven.Communication.Packets.Outgoing.Catalog;
using Raven.HabboHotel.GameClients;
using Raven.HabboHotel.Rooms.AI;
using Raven.Communication.Packets.Incoming;

namespace Raven.Communication.Packets.Incoming.Catalog
{
    public class GetSellablePetBreedsEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            string Type = Packet.PopString();
            string PacketType = "";
            int PetId = RavenEnvironment.GetGame().GetCatalog().GetPetRaceManager().GetPetId(Type, out PacketType);

            Session.SendMessage(new SellablePetBreedsComposer(PacketType, PetId, RavenEnvironment.GetGame().GetCatalog().GetPetRaceManager().GetRacesForRaceId(PetId)));
        }
    }
}