using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Raven.HabboHotel.Quests;
using Raven.HabboHotel.Rooms;
using Raven.Communication.Packets.Outgoing.Rooms.Avatar;

namespace Raven.Communication.Packets.Incoming.Rooms.Avatar
{
    class DanceEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (!Session.GetHabbo().InRoom)
                return;

            Room Room;

            if (!RavenEnvironment.GetGame().GetRoomManager().TryGetRoom(Session.GetHabbo().CurrentRoomId, out Room))
                return;

            RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (User == null)
                return;

            User.UnIdle();

            int DanceId = Packet.PopInt();
            if (DanceId < 0 || DanceId > 4)
                DanceId = 0;

            if (DanceId > 0 && User.CarryItemID > 0)
                User.CarryItem(0);

            if (Session.GetHabbo().Effects().CurrentEffect > 0)
                Room.SendMessage(new AvatarEffectComposer(User.VirtualId, 0));

            User.DanceId = DanceId;

            Room.SendMessage(new DanceComposer(User, DanceId));

            RavenEnvironment.GetGame().GetQuestManager().ProgressUserQuest(Session, QuestType.SOCIAL_DANCE);
            if (Room.GetRoomUserManager().GetRoomUsers().Count > 19)
                RavenEnvironment.GetGame().GetQuestManager().ProgressUserQuest(Session, QuestType.MASS_DANCE);
        }
    }
}