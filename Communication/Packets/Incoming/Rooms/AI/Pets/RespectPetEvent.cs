﻿using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Raven.HabboHotel.Rooms;
using Raven.HabboHotel.Quests;
using Raven.Communication.Packets.Outgoing.Pets;
using Raven.Communication.Packets.Outgoing.Rooms.Avatar;

namespace Raven.Communication.Packets.Incoming.Rooms.AI.Pets
{
    class RespectPetEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null || !Session.GetHabbo().InRoom || Session.GetHabbo().GetStats() == null || Session.GetHabbo().GetStats().DailyPetRespectPoints == 0)
                return;

            Room Room;

            if (!RavenEnvironment.GetGame().GetRoomManager().TryGetRoom(Session.GetHabbo().CurrentRoomId, out Room))
                return;

            RoomUser ThisUser = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (ThisUser == null)
                return;

            int PetId = Packet.PopInt();

            RoomUser Pet = null;
            if (!Session.GetHabbo().CurrentRoom.GetRoomUserManager().TryGetPet(PetId, out Pet)) 
            {
                RoomUser TargetUser = Session.GetHabbo().CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(PetId);
                if (TargetUser == null)
                    return;

                if (TargetUser.GetClient() == null || TargetUser.GetClient().GetHabbo() == null)
                    return;

                if (TargetUser.GetClient().GetHabbo().Id == Session.GetHabbo().Id)
                {
                    Session.SendWhisper("A ver vale que quieras Duckets, pero respetarte a tí mismo ya es pasarse, ¿no?");
                    return;
                }

                RavenEnvironment.GetGame().GetAchievementManager().ProgressAchievement(Session, "ACH_RespectGiven", 1);
                RavenEnvironment.GetGame().GetAchievementManager().ProgressAchievement(TargetUser.GetClient(), "ACH_RespectEarned", 1);

                Session.GetHabbo().GetStats().DailyPetRespectPoints -= 1;
                Session.GetHabbo().GetStats().RespectGiven += 1;
                TargetUser.GetClient().GetHabbo().GetStats().Respect += 1;

                ThisUser.CarryItemID = 999999999;
                ThisUser.CarryTimer = 5;

                if (Room.RespectNotificationsEnabled)
                Room.SendMessage(new RespectPetNotificationMessageComposer(TargetUser.GetClient().GetHabbo(), TargetUser));
                Room.SendMessage(new CarryObjectComposer(ThisUser.VirtualId, ThisUser.CarryItemID));
                return;
            }

            if (Pet == null || Pet.PetData == null || Pet.RoomId != Session.GetHabbo().CurrentRoomId)
                return;

            Session.GetHabbo().GetStats().DailyPetRespectPoints -= 1;
            RavenEnvironment.GetGame().GetAchievementManager().ProgressAchievement(Session, "ACH_PetRespectGiver", 1, false);

            ThisUser.CarryItemID = 999999999;
            ThisUser.CarryTimer = 5;
            Pet.PetData.OnRespect();
            Room.SendMessage(new CarryObjectComposer(ThisUser.VirtualId, ThisUser.CarryItemID));

            if (RavenEnvironment.GetGame().GetClientManager().GetClientByUserID(Pet.PetData.OwnerId) != null && Session.GetHabbo().Id != Pet.PetData.OwnerId)
            {
                RavenEnvironment.GetGame().GetAchievementManager().ProgressAchievement(RavenEnvironment.GetGame().GetClientManager().GetClientByUserID(Pet.PetData.OwnerId), "ACH_PetRespectReceiver", 1);
            }
        }
    }
}