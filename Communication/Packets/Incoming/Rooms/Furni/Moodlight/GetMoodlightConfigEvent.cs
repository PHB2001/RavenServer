﻿using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Raven.HabboHotel.Rooms;
using Raven.HabboHotel.Items;
using Raven.HabboHotel.Items.Data.Moodlight;

using Raven.Communication.Packets.Outgoing.Rooms.Furni.Moodlight;

namespace Raven.Communication.Packets.Incoming.Rooms.Furni.Moodlight
{
    class GetMoodlightConfigEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (!Session.GetHabbo().InRoom)
                return;

            Room Room;

            if (!RavenEnvironment.GetGame().GetRoomManager().TryGetRoom(Session.GetHabbo().CurrentRoomId, out Room))
                return;

            if (!Room.CheckRights(Session, true))
                return;

            if (Room.MoodlightData == null)
            {
                foreach (Item item in Room.GetRoomItemHandler().GetWall.ToList())
                {
                    if (item.GetBaseItem().InteractionType == InteractionType.MOODLIGHT)
                        Room.MoodlightData = new MoodlightData(item.Id);
                }
            }

            if (Room.MoodlightData == null)
                return;

            Session.SendMessage(new MoodlightConfigComposer(Room.MoodlightData));
        }
    }
}