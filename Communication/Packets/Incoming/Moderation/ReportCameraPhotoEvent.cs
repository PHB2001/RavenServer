using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Raven.HabboHotel.Support;
using Raven.HabboHotel.Rooms.Chat.Moderation;
using Raven.Communication.Packets.Outgoing.Moderation;
using Raven.HabboHotel.Moderation;

namespace Raven.Communication.Packets.Incoming.Moderation
{
    class ReportCameraPhotoEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null)
                return;

            //if (RavenEnvironment.GetGame().GetModerationManager().(Session.GetHabbo().Id))
            //{
            //    Session.SendMessage(new BroadcastMessageAlertComposer("You currently already have a pending ticket, please wait for a response from a moderator."));
            //    return;
            //}

            int photoId;

            if (!int.TryParse(Packet.PopString(), out photoId))
            {
                return;
            }

            int roomId = Packet.PopInt();
            int creatorId = Packet.PopInt();
            int categoryId = Packet.PopInt();

           // RavenEnvironment.GetGame().GetModerationTool().SendNewTicket(Session, categoryId, creatorId, "", new List<string>(), (int) ModerationSupportTicketType.PHOTO, photoId);
            RavenEnvironment.GetGame().GetClientManager().ModAlert("A new support ticket has been submitted!");
        }
    }
}