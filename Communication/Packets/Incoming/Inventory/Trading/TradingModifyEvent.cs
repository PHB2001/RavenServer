﻿using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Raven.HabboHotel.Rooms;
using Raven.HabboHotel.Rooms.Trading;

namespace Raven.Communication.Packets.Incoming.Inventory.Trading
{
    class TradingModifyEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null || !Session.GetHabbo().InRoom)
                return;

            Room Room;

            if (!RavenEnvironment.GetGame().GetRoomManager().TryGetRoom(Session.GetHabbo().CurrentRoomId, out Room))
                return;
            
            if (!Room.CanTradeInRoom)
                return;

            Trade Trade = Room.GetUserTrade(Session.GetHabbo().Id);
            if (Trade == null)
                return;

            Trade.Unaccept(Session.GetHabbo().Id);
        }
    }
}