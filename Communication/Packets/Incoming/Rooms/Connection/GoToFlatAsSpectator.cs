using System;
using System.Linq;
using System.Text;

using Raven.Communication.Packets.Incoming;
using Raven.HabboHotel.GameClients;
using Raven.Communication.Packets.Outgoing.Rooms.Session;
using Raven.Communication.Packets.Outgoing.Rooms.Engine;
using Raven.Communication.Packets.Outgoing.Nux;
using Raven.HabboHotel.Rooms;
using Raven.HabboHotel.Users;

namespace Raven.Communication.Packets.Incoming.Rooms.Connection
{
    class GoToFlatAsSpectatorEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            if (!Session.GetHabbo().InRoom)
                return;

            //Session.GetHabbo().Spectating = true;
            //Session.SendMessage(new RoomSpectatorComposer());

            //Room roomToSpec = Session.GetHabbo().CurrentRoom;
            
            //roomToSpec.QueueingUsers.Remove(Session.GetHabbo());
            //foreach (Habbo user in roomToSpec.QueueingUsers)
            //{
            //    if (roomToSpec.QueueingUsers.First().Id == user.Id)
            //    {
            //        user.PrepareRoom(roomToSpec.Id, "");
            //    }
            //    else
            //    {
            //        user.GetClient().SendMessage(new RoomQueueComposer(roomToSpec.QueueingUsers.IndexOf(user)));
            //    }
            //}
            
            if (!Session.GetHabbo().EnterRoom(Session.GetHabbo().CurrentRoom))
                Session.SendMessage(new CloseConnectionComposer());
        }
    }
}
