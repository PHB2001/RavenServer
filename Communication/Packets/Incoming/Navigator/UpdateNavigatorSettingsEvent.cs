using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Raven.HabboHotel.Rooms;
using Raven.Communication.Packets.Outgoing.Navigator;

namespace Raven.Communication.Packets.Incoming.Navigator
{
    class UpdateNavigatorSettingsEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int roomID = Packet.PopInt();
            if (roomID == 0)
                return;

            RoomData Data = RavenEnvironment.GetGame().GetRoomManager().GenerateRoomData(roomID);
            if (Data == null)
                return;

            Session.GetHabbo().HomeRoom = roomID;
            Session.SendMessage(new NavigatorSettingsComposer(roomID));
        }
    }
}
