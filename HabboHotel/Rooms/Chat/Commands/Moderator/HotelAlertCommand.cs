using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Raven.Communication.Packets.Outgoing.Moderation;
using Raven.Communication.Packets.Outgoing.Rooms.Notifications;

namespace Raven.HabboHotel.Rooms.Chat.Commands.Moderator
{
    class HotelAlertCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_hotel_alert"; }
        }

        public string Parameters
        {
            get { return "%message%"; }
        }

        public string Description
        {
            get { return "Envia un mensaje a todo el Hotel"; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Por favor escribe el mensaje a enviar");
                return;
            }
            string Message = CommandManager.MergeParams(Params, 1);
            if (RavenEnvironment.GetDBConfig().DBData["hotel.name"] == "Mabbi")
               
            RavenEnvironment.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("Mensaje de " + Session.GetHabbo().Username + ":", "<font size =\"11\">Querido usuario de Mabbi, el usuario " + Session.GetHabbo().Username + " tiene un mensaje para todo el hotel:</font><br><br><font size =\"11\" color=\"#B40404\">" + Message + "</font><br><br><font size =\"10\" color=\"#0B4C5F\">Recuerda estar atent@ a las redes sociales para mantenerte siempre al día de las actualizaciones en Mabbi Hotel:<br><br><b>FACEBOOK</b>: @EsMabbi<br><b>TWITTER</b>: @EsMabbi<br><b>INSTAGRAM:</b> @EsMabbi</font>", "alertz", ""));
                  else
                RavenEnvironment.GetGame().GetClientManager().SendMessage(new BroadcastMessageAlertComposer(RavenEnvironment.GetGame().GetLanguageLocale().TryGetValue("hotelalert_text") + Message + "\r\n- " + Session.GetHabbo().Username, ""));
            return;
        }
    }
}
