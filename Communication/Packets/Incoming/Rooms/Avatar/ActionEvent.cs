using Raven.Communication.Packets.Outgoing.Rooms.Avatar;
using Raven.HabboHotel.GameClients;
using Raven.HabboHotel.Quests;
using Raven.HabboHotel.Rooms;
using Raven.Communication.Packets.Incoming;
using Raven.HabboHotel.Items.Wired;

namespace Raven.Communication.Packets.Incoming.Rooms.Avatar
{
    public class ActionEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            if (!Session.GetHabbo().InRoom)
                return;

            int Action = Packet.PopInt();

            Room Room = null;
            if (!RavenEnvironment.GetGame().GetRoomManager().TryGetRoom(Session.GetHabbo().CurrentRoomId, out Room))
                return;

            RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (User == null)
                return;

            if (User.DanceId > 0)
                User.DanceId = 0;

            User.UnIdle();
            Room.SendMessage(new ActionComposer(User.VirtualId, Action));

            if (Action == 5) // idle
            {
                Room.GetWired().TriggerEvent(WiredBoxType.TriggerSpaceUserIdle, User.GetClient().GetHabbo());
                User.IsAsleep = true;
                Room.SendMessage(new SleepComposer(User, true));
            }

            RavenEnvironment.GetGame().GetQuestManager().ProgressUserQuest(Session, QuestType.SOCIAL_WAVE);
        }
    }
}