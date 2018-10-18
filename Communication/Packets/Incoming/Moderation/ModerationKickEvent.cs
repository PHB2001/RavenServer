using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Raven.Core;
using Raven.HabboHotel.Rooms;
using Raven.HabboHotel.Support;
using Raven.HabboHotel.GameClients;

namespace Raven.Communication.Packets.Incoming.Moderation
{
    class ModerationKickEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null || !Session.GetHabbo().GetPermissions().HasRight("mod_kick"))
                return;

            int UserId = Packet.PopInt();
            string Message = Packet.PopString();

            GameClient Client = RavenEnvironment.GetGame().GetClientManager().GetClientByUserID(UserId);
            if (Client == null || Client.GetHabbo() == null || Client.GetHabbo().CurrentRoomId < 1 || Client.GetHabbo().Id == Session.GetHabbo().Id)
                return;

            if (Client.GetHabbo().Rank >= Session.GetHabbo().Rank)
            {
                Session.SendNotification(RavenEnvironment.GetGame().GetLanguageLocale().TryGetValue("moderation_kick_permissions"));
                return;
            }

            Room Room = null;
            if (!RavenEnvironment.GetGame().GetRoomManager().TryGetRoom(Session.GetHabbo().CurrentRoomId, out Room))
                return;
            
            Room.GetRoomUserManager().RemoveUserFromRoom(Client, true, false);
        }
    }
}
