using System;
using Raven.HabboHotel.GameClients;
using Raven.Communication.Packets.Outgoing.Rooms.Notifications;
using Raven.Communication.Packets.Outgoing.Catalog;
using Raven.Communication.Packets.Outgoing;
using Raven.Communication.Packets.Outgoing.Misc;
using Raven.Communication.Packets.Outgoing.Rooms.Freeze;
using Raven.Communication.Packets.Outgoing.Rooms.Settings;
using System.Text;
using Raven.Communication.Packets.Outgoing.Handshake;
using Raven.HabboHotel.Users;
using Raven.Communication.Packets.Outgoing.Help.Helpers;
using Raven.HabboHotel.Moderation;
using Raven.Utilities;
using System.Collections.Generic;

namespace Raven.HabboHotel.Rooms.Chat.Commands.User
{
    class InfoCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_info"; }
        }

        public string Parameters
        {
            get { return ""; }
        }

        public string Description
        {
            get { return "Información de Raven."; }
        }

        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(RavenEnvironment.GetUnixTimestamp()).ToLocalTime();

            //string Page = Params[1];

            //ServerPacket packet = new ServerPacket(3);
            //packet.WriteString("Youtube");
            //packet.WriteString("<iframe id=\"youtube-player\" frameborder=\"0\" allowfullscreen=\"1\" allow=\"autoplay; encrypted - media\" title=\"YouTube video player\" width=\"480\" height=\"270\" src=\"https://www.youtube.com/embed/2b1TJbLcneM?autoplay=1&amp;fs=0&amp;modestbranding=1&amp;rel=0&amp;enablejsapi=1&amp;origin=http%3A%2F%2Fhabblive.in&amp;widgetid=1\"></iframe>");

            //if (Session.wsSession != null)
            //    Session.wsSession.send(packet);

            TimeSpan Uptime = DateTime.Now - RavenEnvironment.ServerStarted;
            int OnlineUsers = RavenEnvironment.GetGame().GetClientManager().Count;
            int RoomCount = RavenEnvironment.GetGame().GetRoomManager().Count;

            //Random Random = new Random();

            //int RandomNumber = Random.Next(1, 1000000);
            //Session.SendMessage(new MassEventComposer("habbopages/" + Page + ".txt?" + RandomNumber + ""));

            //Habbo ReportedUser = RavenEnvironment.GetHabboById(Session.GetHabbo().Id);
            //List<string> Chats = new List<string>();

            //Chats.Add("Hola que tal soy un delincuente.");
            //ModerationTicket Ticket = new ModerationTicket(1, 4, 1, UnixTimestamp.GetNow(), 1, Session.GetHabbo(), ReportedUser, "Men esto es una prueba de cojones que procede", Session.GetHabbo().CurrentRoom, Chats);
            //if (!RavenEnvironment.GetGame().GetModerationManager().TryAddTicket(Ticket))
            //    return;

            //Session.SendMessage(new NewYearResolutionCompletedComposer("ADM"));
            Session.SendMessage(new RoomNotificationComposer("RAVEN PROJECT r2.01 BUILD 030220:",
                "<font color='#0D0106'><b>About Raven:</b>\n" +
                "<font size=\"11\" color=\"#1C1C1C\">Private project powered by Custom and enhanced by Andre, </font>" +
                "<font size=\"11\" color=\"#1C1C1C\">our main goal is sharing Habbo basics with our customers, adding some untold content.\n\nRaven Project started on July 2016 and keeps up for being one of the most relevant projects over the community.\n\n" +
                "<font size =\"12\" color=\"#0B4C5F\"><b>Stats:</b></font>\n" +
                "<font size =\"11\" color=\"#1C1C1C\">  <b> · Users: </b> " + OnlineUsers + "\r  <b> · Rooms: </b> " + RoomCount + "\r  <b> · Uptime: </b> " + Uptime.Days + " days, " + Uptime.Hours + " hours and " + Uptime.Minutes + " minutes.\r <b>  · Date: </b> " + dtDateTime + ".</font>\n\n" +
                "Last update on  <b>1-03-2018</b>.\n\n<font size =\"12\" color=\"#0B4C5F\">Check out <b> :changelog</b> for last updates.</font>", "raven"));
        }
    }
}