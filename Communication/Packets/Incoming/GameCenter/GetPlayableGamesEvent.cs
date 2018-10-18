using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Raven.Communication.Packets.Outgoing.GameCenter;

namespace Raven.Communication.Packets.Incoming.GameCenter
{
    class GetPlayableGamesEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int GameId = Packet.PopInt();

            Session.SendMessage(new GameAccountStatusComposer(GameId));
            Session.SendMessage(new PlayableGamesComposer(GameId));
            Session.SendMessage(new GameAchievementListComposer(Session, RavenEnvironment.GetGame().GetAchievementManager().GetGameAchievements(GameId), GameId));
        }
    }
}