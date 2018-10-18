﻿using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Raven.Database.Interfaces;
using Raven.HabboHotel.Groups;
using Raven.Communication.Packets.Outgoing.Rooms.Notifications;

using Raven.HabboHotel.Rooms;

namespace Raven.Communication.Packets.Incoming.Groups
{
    class DeleteGroupEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            Group Group = null;
            if (!RavenEnvironment.GetGame().GetGroupManager().TryGetGroup(Packet.PopInt(), out Group))
            {
                Session.SendMessage(new RoomNotificationComposer("Oops!",
                 "¡No se ha encontrado este grupo!", "nothing", ""));
                return;
            }

            if (Group.CreatorId != Session.GetHabbo().Id && !Session.GetHabbo().GetPermissions().HasRight("group_delete_override"))
            {
                Session.SendMessage(new RoomNotificationComposer("Oops!",
                 "¡Sólo el dueño del grupo puede eliminarlo!", "nothing", ""));
                return;
            }

            if (Group.MemberCount >= RavenStaticGameSettings.GroupMemberDeletionLimit && !Session.GetHabbo().GetPermissions().HasRight("group_delete_limit_override"))
            {
                Session.SendMessage(new RoomNotificationComposer("Sucesso",
                 "El grupo sobre pasa el límite de miembros permitido (" + RavenStaticGameSettings.GroupMemberDeletionLimit + "), contacta con uno de los miembros del equipo administrativo.", "nothing", ""));
                return;
            }

            Room Room = RavenEnvironment.GetGame().GetRoomManager().LoadRoom(Group.RoomId);

            if (Room != null)
            {
                Room.Group = null;
                Room.RoomData.Group = null;//Eu não tenho certeza se isso é necessário ou não, por causa da herança, mas tudo bem.
            }

            //Removê-lo do cache.
            RavenEnvironment.GetGame().GetGroupManager().DeleteGroup(Group.Id);

            //Agora as coisas.
            using (IQueryAdapter dbClient = RavenEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunQuery("DELETE FROM `groups` WHERE `id` = '" + Group.Id + "'");
                dbClient.RunQuery("DELETE FROM `group_memberships` WHERE `group_id` = '" + Group.Id + "'");
                dbClient.RunQuery("DELETE FROM `group_requests` WHERE `group_id` = '" + Group.Id + "'");
                dbClient.RunQuery("UPDATE `rooms` SET `group_id` = '0' WHERE `group_id` = '" + Group.Id + "' LIMIT 1");
                dbClient.RunQuery("UPDATE `user_stats` SET `groupid` = '0' WHERE `groupid` = '" + Group.Id + "' LIMIT 1");
                dbClient.RunQuery("DELETE FROM `items_groups` WHERE `group_id` = '" + Group.Id + "'");
            }

            //Descarregá-lo pela última vez.
            RavenEnvironment.GetGame().GetRoomManager().UnloadRoom(Room, true);

            //Wulles Rainha
            Session.SendMessage(new RoomNotificationComposer("Sucesso",
                 "¡Has borrado satisfactoriamente tu grupo!", "nothing", ""));
            return;
        }
    }
}
