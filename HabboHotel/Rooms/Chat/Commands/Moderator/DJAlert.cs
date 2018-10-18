using Raven.HabboHotel.GameClients;
using Raven.Communication.Packets.Outgoing.Rooms.Notifications;
using Raven.Communication.Packets.Outgoing.Rooms.Chat;
using System;
using Raven.Database.Interfaces;

namespace Raven.HabboHotel.Rooms.Chat.Commands.Moderator
{
    internal class DJAlert : IChatCommand
    {
        public string PermissionRequired
        {
            get
            {
                return "command_djalert";
            }
        }
        public string Parameters
        {
            get { return "%message%"; }
        }
        public string Description
        {
            get
            {
                return "Abre un evento de DJ en todo el hotel.";
            }
        }
        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            

            string Message = CommandManager.MergeParams(Params, 1);

            Session.GetHabbo()._eventsopened++;

            RavenEnvironment.GetGame().GetClientManager().SendEventType1(new RoomNotificationComposer("¡DJ EN VIVO!", "¡<b><font color=\"#2E9AFE\">" + Session.GetHabbo().Username + "</font></b> está transmitiendo en vivo en este momento! Si quieres ganar <font color=\"#f18914\"><b> Premios </b></font> participa ahora mismo.<br><br>¿Quieres participar? ¡Haz click en el botón inferior de <b> Ir a la sala de la transmisión</b>, y dentro podrás participar, sigue las instrucciones!<br><br>¿De qué trata esta transmisión?<br><br><font color='#FF0040'><b>"
              + Message + "</b></font><br><br>¡Te esperamos con los brazos abiertos!<br><br>Recuerda que puedes cambiar esta alerta con el comando <b> :eventtype events 1</b> .", "djmabbi", "¡Ir a la sala de la transmisión!", "event:navigator/goto/" + Session.GetHabbo().CurrentRoomId));

            RavenEnvironment.GetGame().GetClientManager().SendEventType2(new WhisperComposer(-1, "Hay un nuevo evento, haz <font color=\"#2E9AFE\"><a href='event:navigator/goto/" + Session.GetHabbo().CurrentRoomId + "'><b>click aquí</b></a></font> para ir a la transmisión.", 0, 33));
            RavenEnvironment.GetGame().GetClientManager().SendEventType2(new WhisperComposer(-1, Message, 0, 33));
            RavenEnvironment.GetGame().GetClientManager().SendEventType2(new WhisperComposer(-1, "Transmisión en directo con: " + Session.GetHabbo().Username + ".", 0, 33));

            LogEvent(Session.GetHabbo().Id, Room.Id, Message);
        }

        public void LogEvent(int MasterID, int RoomID, string Message)
        {
            DateTime Now = DateTime.Now;
            using (IQueryAdapter dbClient = RavenEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("INSERT INTO event_logs VALUES (NULL, " + MasterID + ", " + RoomID + ", @message, UNIX_TIMESTAMP())");
                dbClient.AddParameter("message", Message);
                dbClient.RunQuery();
            }
        }


    }
}

