﻿using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Raven.Communication.Packets.Outgoing.Moderation;
using Raven.Communication.Packets.Outgoing.Rooms.Notifications;

namespace Raven.HabboHotel.Rooms.Chat.Commands.Moderator
{
    class CustomizedHotelAlert : IChatCommand
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
                Session.SendWhisper("Por favor escribe el mensaje a enviar.");
                return;
            }

            string Message = CommandManager.MergeParams(Params, 1);
            RavenEnvironment.GetGame().GetClientManager().SendMessage(new RoomCustomizedAlertComposer("\n"+ Message + "\n\n - "+ Session.GetHabbo().Username +""));
            return;
        }
    }
}
