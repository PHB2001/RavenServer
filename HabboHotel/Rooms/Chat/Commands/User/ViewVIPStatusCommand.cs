using Raven.HabboHotel.GameClients;

using Raven.Communication.Packets.Outgoing.Rooms.Notifications;
using Raven.Database.Interfaces;
using System.Data;
using System;
using Raven.Communication.Packets.Outgoing.Rooms.Engine;

namespace Raven.HabboHotel.Rooms.Chat.Commands.User
{
    class ViewVIPStatusCommand : IChatCommand
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
            get { return "Información de tu suscripción VIP."; }
        }

        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            Session.SendMessage(RoomNotificationComposer.SendBubble("abuse", "No eres miembro del Club VIP de Mabbi, haz click aquí para abonarte.", ""));
        }
    }
}