﻿using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Raven.HabboHotel.Rooms;
using Raven.HabboHotel.Groups;
using Raven.Communication.Packets.Outgoing.Groups;
using Raven.Communication.Packets.Outgoing.Rooms.Permissions;


using Raven.HabboHotel.Users;
using Raven.HabboHotel.Cache;

namespace Raven.Communication.Packets.Incoming.Groups
{
    class DeclineGroupMembershipEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int GroupId = Packet.PopInt();
            int UserId = Packet.PopInt();

            Group Group = null;
            if (!RavenEnvironment.GetGame().GetGroupManager().TryGetGroup(GroupId, out Group))
                return;

            if (Session.GetHabbo().Id != Group.CreatorId && !Group.IsAdmin(Session.GetHabbo().Id))
                return;

            if (!Group.HasRequest(UserId))
                return;

            Group.HandleRequest(UserId, false);
            Session.SendMessage(new UnknownGroupComposer(Group.Id, UserId));
        }
    }
}