using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Raven.HabboHotel.Users;
using Raven.HabboHotel.Rooms;
using Raven.HabboHotel.Groups;
using Raven.Communication.Packets.Outgoing.Groups;
using Raven.Communication.Packets.Outgoing.Rooms.Permissions;
using Raven.HabboHotel.Cache;



namespace Raven.Communication.Packets.Incoming.Groups
{
    class TakeAdminRightsEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int GroupId = Packet.PopInt();
            int UserId = Packet.PopInt();

            Group Group = null;
            if (!RavenEnvironment.GetGame().GetGroupManager().TryGetGroup(GroupId, out Group))
                return;

            if (!Group.IsMember(UserId) || !Group.IsAdmin(Session.GetHabbo().Id))
            return;

            Habbo Habbo = RavenEnvironment.GetHabboById(UserId);
            if (Habbo == null)
            {
                Session.SendNotification("Oops, ocurrio un error mientras se realizaba la busqueda de este usuario.");
                return;
            }

            Group.TakeAdmin(UserId);

            Room Room = null;
            if (RavenEnvironment.GetGame().GetRoomManager().TryGetRoom(Group.RoomId, out Room))
            {
                RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(UserId);
                if (User != null)
                {
                    if (User.Statusses.ContainsKey("flatctrl 3"))
                        User.RemoveStatus("flatctrl 3");
                    User.UpdateNeeded = true;
                    if (User.GetClient() != null)
                        User.GetClient().SendMessage(new YouAreControllerComposer(0));
                }
            }

            Session.SendMessage(new GroupMemberUpdatedComposer(GroupId, Habbo, 2));
        }
    }
}
