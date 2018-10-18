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


namespace Raven.HabboHotel.Rooms.Chat.Commands.Events
{
    internal class SpecialEvent : IChatCommand
    {
        public string PermissionRequired
        {
            get
            {
                return "command_addpredesigned";
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
                return "Manda un evento a todo el hotel.";
            }
        }
        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            string Message = CommandManager.MergeParams(Params, 1);

            RavenEnvironment.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("¿Qué está pasando en " + RavenEnvironment.GetDBConfig().DBData["hotel.name"] + "...?",
                 "Algo está ocurriendo en Mabbi, Andre, M00nlight y Pussy han desaparecido en medio de la ceremonia...<br><br>Un ente susurra y pide ayuda a todo Mabbi, parece que los espíritus reclaman la presencia de todos nuestros usuarios.<br></font></b><br>Si quieres colaborar haz click en el botón inferior y sigue las instrucciones.<br><br></font>", "2mesex", "¡A la aventura!", "event:navigator/goto/" + Session.GetHabbo().CurrentRoomId));

        }
    }
}

