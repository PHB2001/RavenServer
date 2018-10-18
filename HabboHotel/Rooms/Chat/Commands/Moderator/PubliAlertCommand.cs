using Raven.Communication.Packets.Outgoing.Moderation;
using Raven.Communication.Packets.Outgoing.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Communication.Packets.Outgoing.Rooms.Notifications;

namespace Raven.HabboHotel.Rooms.Chat.Commands.Moderator
{
    class PubliAlertCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_info"; }
        }

        public string Parameters
        {
            get { return "%message%"; }
        }

        public string Description
        {
            get { return "Enviale un mensaje de alerta a todos los staff online."; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Session.GetHabbo().TeamRank != 4)
            {
                Session.SendWhisper("No puedes enviar oleadas para Publicistas si no lo eres.");
                return;

            }

            string Message = CommandManager.MergeParams(Params, 1);
            RavenEnvironment.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("Se ha abierto oleada de publicidad..",
                 "¡Hay una nueva oleada de publicidad en activo! Si quieres ganar <b>distintas recompensas</b> por participar acude a la sala de publicidad.<br><br>¿Quién ha abierto la oleada? <b> <font color=\"#58ACFA\">  "
                 + Session.GetHabbo().Username + "</font></b><br>Si quieres participar haz click en el botón inferior de <b>Ir a la sala del evento</b>, y ahí dentro podrás participar.<br><br>¿De qué trata este evento?<br><br><font color='#084B8A'><b>Trata de seguir las instrucciones de los guías de la oleada para participar y así ganar tu premio!</b></font><br><br>¡Te esperamos!", "zpam", "Ir a la sala de la oleada", "event:navigator/goto/" + Session.GetHabbo().CurrentRoomId));
        }
    }
}