﻿using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Raven.HabboHotel.Games;
using Raven.Communication.Packets.Outgoing.GameCenter;
using System.Data;

using Raven.HabboHotel.Users;
using Raven.HabboHotel.GameClients;
using Raven.Communication.Packets.Outgoing.Rooms.Notifications;

namespace Raven.Communication.Packets.Incoming.GameCenter
{
    class UnknownGameCenterEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            int GameId = Packet.PopInt();
            int UserId = Packet.PopInt();

            GameData GameData = null;
            if (RavenEnvironment.GetGame().GetGameDataManager().TryGetGame(GameId, out GameData))
            {
               // Session.SendMessage(new Game2WeeklyLeaderboardComposer(GameId)); Comentado y funciona
            }
        }
    }
}
