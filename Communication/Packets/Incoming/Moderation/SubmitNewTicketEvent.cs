using System.Collections.Generic;

using Raven.Utilities;
using Raven.HabboHotel.Users;
using Raven.Database.Interfaces;
using Raven.HabboHotel.Moderation;
using Raven.Communication.Packets.Outgoing.Moderation;

namespace Raven.Communication.Packets.Incoming.Moderation
{
    class SubmitNewTicketEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null)
                return;

            if (RavenEnvironment.GetGame().GetModerationManager().UserHasTickets(Session.GetHabbo().Id))
            {
                ModerationTicket PendingTicket = RavenEnvironment.GetGame().GetModerationManager().GetTicketBySenderId(Session.GetHabbo().Id);
                if (PendingTicket != null)
                {
                    Session.SendMessage(new CallForHelpPendingCallsComposer(PendingTicket));
                    return;
                }
            }

            List<string> Chats = new List<string>();

            string Message = StringCharFilter.Escape(Packet.PopString().Trim());
            int Category = Packet.PopInt();
            int ReportedUserId = Packet.PopInt();
            int Type = Packet.PopInt();

            Habbo ReportedUser = RavenEnvironment.GetHabboById(ReportedUserId);
            if (ReportedUser == null)
            {
                return;
            }

            int Messagecount = Packet.PopInt();
            for (int i = 0; i < Messagecount; i++)
            {
                Packet.PopInt();
                Chats.Add(Packet.PopString());
            }

            ModerationTicket Ticket = new ModerationTicket(1, Type, Category, UnixTimestamp.GetNow(), 1, Session.GetHabbo(), ReportedUser, Message, Session.GetHabbo().CurrentRoom, Chats);
            if (!RavenEnvironment.GetGame().GetModerationManager().TryAddTicket(Ticket))
                return;

            using (IQueryAdapter dbClient = RavenEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunQuery("UPDATE `user_info` SET `cfhs` = `cfhs` + '1' WHERE `user_id` = '" + Session.GetHabbo().Id + "' LIMIT 1");
            }

            RavenEnvironment.GetGame().GetClientManager().ModAlert("A new support ticket has been submitted!");
            RavenEnvironment.GetGame().GetClientManager().SendMessage(new ModeratorSupportTicketComposer(Session.GetHabbo().Id, Ticket), "mod_tool");
        }
    }
}
