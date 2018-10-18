﻿using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Raven.HabboHotel.Groups;

namespace Raven.Communication.Packets.Outgoing.Groups
{
    class GroupFurniSettingsComposer : ServerPacket
    {
        public GroupFurniSettingsComposer(Group Group, int ItemId, int UserId)
            : base(ServerPacketHeader.GroupFurniSettingsMessageComposer)
        {
            base.WriteInteger(ItemId);//Item Id
            base.WriteInteger(Group.Id);//Group Id?
            base.WriteString(Group.Name);
            base.WriteInteger(Group.RoomId);//RoomId
            base.WriteBoolean(Group.IsMember(UserId));//Member?
            base.WriteBoolean(Group.ForumEnabled);//Has a forum
        }
    }
}