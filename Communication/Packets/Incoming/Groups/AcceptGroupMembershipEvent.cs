using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Raven.HabboHotel.Rooms;
using Raven.HabboHotel.Groups;
using Raven.Communication.Packets.Outgoing.Groups;
using Raven.Communication.Packets.Outgoing.Rooms.Permissions;


using Raven.HabboHotel.Users;
using Raven.HabboHotel.Cache;
using Raven.Communication.Packets.Outgoing.Messenger;

namespace Raven.Communication.Packets.Incoming.Groups
{
    class AcceptGroupMembershipEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int GroupId = Packet.PopInt();
            int UserId = Packet.PopInt();

            Group Group = null;
            if (!RavenEnvironment.GetGame().GetGroupManager().TryGetGroup(GroupId, out Group))
                return;

            if ((Session.GetHabbo().Id != Group.CreatorId && !Group.IsAdmin(Session.GetHabbo().Id)) && !Session.GetHabbo().GetPermissions().HasRight("fuse_group_accept_any"))
                return;

            if (!Group.HasRequest(UserId))
                return;

            Habbo Habbo = RavenEnvironment.GetHabboById(UserId);
            if (Habbo == null)
            {
                Session.SendNotification("Oops, ha recibido un error mientras recibe la busqueda de este usuario.");
                return;
            }

            Group.HandleRequest(UserId, true);

            if (Group.HasChat)
            {
                var Client = RavenEnvironment.GetGame().GetClientManager().GetClientByUserID(UserId);
                if (Client != null)
                {
                    Client.SendMessage(new FriendListUpdateComposer(Group, 1));
                }
            }

            Session.SendMessage(new GroupMemberUpdatedComposer(GroupId, Habbo, 4));
        }
    }
 }