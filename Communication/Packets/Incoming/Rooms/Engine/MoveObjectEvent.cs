﻿using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Raven.HabboHotel.Rooms;
using Raven.HabboHotel.Items;
using Raven.HabboHotel.Quests;
using Raven.Communication.Packets.Outgoing.Rooms.Engine;



namespace Raven.Communication.Packets.Incoming.Rooms.Engine
{
    class MoveObjectEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (!Session.GetHabbo().InRoom)
                return;
            if (Session.GetHabbo().Rank > 3 && !Session.GetHabbo().StaffOk || RavenStaticGameSettings.IsGoingToBeClose)
                return;

            int ItemId = Packet.PopInt();
            if (ItemId == 0)
                return;

            Room Room;

            if (!RavenEnvironment.GetGame().GetRoomManager().TryGetRoom(Session.GetHabbo().CurrentRoomId, out Room))
                return;

            Item Item;
            if (Room.Group != null)
            {
                if (!Room.CheckRights(Session, false, true))
                {
                    Item = Room.GetRoomItemHandler().GetItem(ItemId);
                    if (Item == null)
                        return;

                    Session.SendMessage(new ObjectUpdateComposer(Item, Room.OwnerId));
                    return;
                }
            }
            else
            {
                if (!Room.CheckRights(Session))
                {
                    return;
                }
            }

            Item = Room.GetRoomItemHandler().GetItem(ItemId);

            if (Item == null)
                return;

            int x = Packet.PopInt();
            int y = Packet.PopInt();
            int Rotation = Packet.PopInt();

            if (x != Item.GetX || y != Item.GetY)
                RavenEnvironment.GetGame().GetQuestManager().ProgressUserQuest(Session, QuestType.FURNI_MOVE);

            if (Rotation != Item.Rotation)
                RavenEnvironment.GetGame().GetQuestManager().ProgressUserQuest(Session, QuestType.FURNI_ROTATE);

            if (!Room.GetRoomItemHandler().SetFloorItem(Session, Item, x, y, Rotation, false, false, true))
            {
                Room.SendMessage(new ObjectUpdateComposer(Item, Room.OwnerId));
                return;
            }

            if (Item.GetZ >= 0.1)
                RavenEnvironment.GetGame().GetQuestManager().ProgressUserQuest(Session, QuestType.FURNI_STACK);
        }
    }
}