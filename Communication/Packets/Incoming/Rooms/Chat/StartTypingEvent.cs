using System;

using Raven.Communication.Packets.Incoming;
using Raven.HabboHotel.Rooms;
using Raven.HabboHotel.GameClients;
using Raven.Communication.Packets.Outgoing.Rooms.Chat;

namespace Raven.Communication.Packets.Incoming.Rooms.Chat
{
    public class StartTypingEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            if (!Session.GetHabbo().InRoom)
                return;

            Room Room = Session.GetHabbo().CurrentRoom;
            if (Room == null)
                return;

            RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Username);
            if (User == null)
                return;

            Session.GetHabbo().CurrentRoom.SendMessage(new UserTypingComposer(User.VirtualId, true));
        }
    }
}