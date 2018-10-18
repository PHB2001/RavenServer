using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Raven.HabboHotel.Games;
using Raven.Communication.Packets.Outgoing.GameCenter;

namespace Raven.Communication.Packets.Incoming.GameCenter
{
    class GetGameListingEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            ICollection<GameData> Games = RavenEnvironment.GetGame().GetGameDataManager().GameData;

            Session.SendMessage(new GameListComposer(Games));
        }
    }
}
