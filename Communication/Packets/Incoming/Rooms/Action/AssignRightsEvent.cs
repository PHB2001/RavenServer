using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Raven.Core;
using Raven.HabboHotel.Rooms;

using Raven.Communication.Packets.Outgoing.Rooms.Permissions;
using Raven.Communication.Packets.Outgoing.Rooms.Settings;
using Raven.HabboHotel.Users;

using Raven.Database.Interfaces;
using Raven.HabboHotel.Cache;

namespace Raven.Communication.Packets.Incoming.Rooms.Action
{
    class AssignRightsEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null)
                return;

            int UserId = Packet.PopInt();

            Room Room = null;
            if (!RavenEnvironment.GetGame().GetRoomManager().TryGetRoom(Session.GetHabbo().CurrentRoomId, out Room))
                return;

            if (!Room.CheckRights(Session, true))
                return;

            if (Room.UsersWithRights.Contains(UserId))
            {
                Session.SendNotification(RavenEnvironment.GetGame().GetLanguageLocale().TryGetValue("room_rights_has_rights_error"));
                return;
            }

            Room.UsersWithRights.Add(UserId);

            using (IQueryAdapter dbClient = RavenEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunQuery("INSERT INTO `room_rights` (`room_id`,`user_id`) VALUES ('" + Room.RoomId + "','" + UserId + "')");
            }

            RoomUser RoomUser = Room.GetRoomUserManager().GetRoomUserByHabbo(UserId);
            if (RoomUser != null && !RoomUser.IsBot)
            {
                RoomUser.SetStatus("flatctrl", "1");
                RoomUser.UpdateNeeded = true;
                if (RoomUser.GetClient() != null)
                    RoomUser.GetClient().SendMessage(new YouAreControllerComposer(1));

                Session.SendMessage(new FlatControllerAddedComposer(Room.RoomId, RoomUser.GetClient().GetHabbo().Id, RoomUser.GetClient().GetHabbo().Username));
            }
            else
            {
                UserCache User =  RavenEnvironment.GetGame().GetCacheManager().GenerateUser(UserId);
                if (User != null)
                    Session.SendMessage(new FlatControllerAddedComposer(Room.RoomId, User.Id, User.Username));
            }
        }
    }
}
