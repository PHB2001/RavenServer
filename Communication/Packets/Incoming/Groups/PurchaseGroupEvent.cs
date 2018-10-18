using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Raven.HabboHotel.Rooms;
using Raven.HabboHotel.Groups;
using Raven.Communication.Packets.Outgoing.Groups;
using Raven.Communication.Packets.Outgoing.Catalog;
using Raven.Communication.Packets.Outgoing.Rooms.Session;
using Raven.Communication.Packets.Outgoing.Inventory.Purse;
using Raven.Communication.Packets.Outgoing.Moderation;

namespace Raven.Communication.Packets.Incoming.Groups
{
    class PurchaseGroupEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient session, ClientPacket packet)
        {
            string word;
            string Name = packet.PopString();
            Name = RavenEnvironment.GetGame().GetChatManager().GetFilter().IsUnnaceptableWord(Name, out word) ? "Spam" : Name;
            string Description = packet.PopString();
            Description = RavenEnvironment.GetGame().GetChatManager().GetFilter().IsUnnaceptableWord(Description, out word) ? "Spam" : Description;
            int RoomId = packet.PopInt();
            int Colour1 = packet.PopInt();
            int Colour2 = packet.PopInt();
            int Unknown = packet.PopInt();
            if (session.GetHabbo().Credits < RavenStaticGameSettings.GroupPurchaseAmount)
            {
                session.SendMessage(new BroadcastMessageAlertComposer("A group costs " + RavenStaticGameSettings.GroupPurchaseAmount + " credits! You only have " + session.GetHabbo().Credits + "!"));
                return;
            }
            else
            {
                session.GetHabbo().Credits -= RavenStaticGameSettings.GroupPurchaseAmount;
                session.SendMessage(new CreditBalanceComposer(session.GetHabbo().Credits));
            }
            RoomData Room = RavenEnvironment.GetGame().GetRoomManager().GenerateRoomData(RoomId);
            if (Room == null || Room.OwnerId != session.GetHabbo().Id || Room.Group != null)
                return;
            string Badge = string.Empty;
            for (int i = 0; i < 5; i++)
            {
                Badge += BadgePartUtility.WorkBadgeParts(i == 0, packet.PopInt().ToString(), packet.PopInt().ToString(), packet.PopInt().ToString());
            }
            Group Group = null;
            if (!RavenEnvironment.GetGame().GetGroupManager().TryCreateGroup(session.GetHabbo(), Name, Description, RoomId, Badge, Colour1, Colour2, out Group))
            {
                session.SendNotification("An error occured whilst trying to create this group.\n\nTry again. If you get this message more than once, report it at the link below.\r\rhttp://boonboards.com");
                return;
            }
            session.SendMessage(new PurchaseOKComposer());
            Room.Group = Group;
            if (session.GetHabbo().CurrentRoomId != Room.Id)
                session.SendMessage(new RoomForwardComposer(Room.Id));
            session.SendMessage(new NewGroupInfoComposer(RoomId, Group.Id));
        }
    }
}