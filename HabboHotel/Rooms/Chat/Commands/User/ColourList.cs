using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;

using Raven.Communication.Packets.Outgoing.Users;
using Raven.Communication.Packets.Outgoing.Notifications;


using Raven.Communication.Packets.Outgoing.Handshake;
using Raven.Communication.Packets.Outgoing.Quests;
using Raven.HabboHotel.Items;
using Raven.Communication.Packets.Outgoing.Inventory.Furni;
using Raven.Communication.Packets.Outgoing.Catalog;
using Raven.HabboHotel.Quests;
using Raven.HabboHotel.Rooms;
using System.Threading;
using Raven.HabboHotel.GameClients;
using Raven.Communication.Packets.Outgoing.Rooms.Avatar;
using Raven.Communication.Packets.Outgoing.Pets;
using Raven.Communication.Packets.Outgoing.Messenger;
using Raven.HabboHotel.Users.Messenger;
using Raven.Communication.Packets.Outgoing.Rooms.Polls;
using Raven.Communication.Packets.Outgoing.Rooms.Notifications;
using Raven.Communication.Packets.Outgoing.Availability;
using Raven.Communication.Packets.Outgoing;
using Raven.Communication.Packets.Outgoing.Nux;

namespace Raven.HabboHotel.Rooms.Chat.Commands.User
{
    class ColourList : IChatCommand
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
            Session.SendMessage(new RoomNotificationComposer("Lista de colores:",
                 "<font color='#FF8000'><b>COLORES:</b>\n" +
                 "<font size=\"12\" color=\"#1C1C1C\">El comando :color te permitirá fijar un color que tu desees en tu bocadillo de chat, para poder seleccionar el color deberás especificarlo después de hacer el comando, como por ejemplo:<br><i>:color red</i></font>" +
                 "<font size =\"13\" color=\"#0B4C5F\"><b>Stats:</b></font>\n" +
                 "<font size =\"11\" color=\"#1C1C1C\">  <b> · Users: </b> \r  <b> · Rooms: </b> \r  <b> · Uptime: </b>minutes.</font>\n\n" +
                 "", "quantum", ""));
        }
    }
}