using System;
using Raven.HabboHotel.GameClients;
using Raven.Communication.Packets.Outgoing.Rooms.Notifications;
using Raven.Communication.Packets.Outgoing.Nux;
using Raven.Communication.Packets.Outgoing.Rooms.Furni.RentableSpaces;
using Raven.Communication.Packets.Outgoing.Moderation;
using Raven.Communication.Packets.Outgoing.Catalog;
using Raven.Communication.Packets.Outgoing.Users;
using Raven.Database.Interfaces;

namespace Raven.HabboHotel.Rooms.Chat.Commands.User
{
    class ControlCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_control"; }
        }

        public string Parameters
        {
            get { return "<usuario>"; }
        }

        public string Description
        {
            get { return "Controla al usuario que selecciones."; }
        }

        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            if (Params.Length != 2)
            {
                Session.SendWhisper("Introduce el nombre del usuario a quien deseas enviar una placa!");
                return;
            }

            if (Params.Length == 2 && Params[1] == "end")
            {
                Session.SendWhisper("Has dejado de controlar a " + Session.GetHabbo().Opponent +".");
                Session.GetHabbo().isControlling = false;
                return;
            }

            GameClient TargetClient = RavenEnvironment.GetGame().GetClientManager().GetClientByUsername(Params[1]);
            if (TargetClient != null)
            {
                Session.GetHabbo().Opponent = TargetClient.GetHabbo().Username;
                Session.GetHabbo().isControlling = true;
                Session.SendMessage(RoomNotificationComposer.SendBubble("definitions", "Ahora estás controlando a " + TargetClient.GetHabbo().Username + ". Para parar di :control end."));
                return;
            }

            else Session.SendMessage(RoomNotificationComposer.SendBubble("definitions", "No se ha encontrado el usuario " + Params[1] + ".", ""));
        }
    }
}
