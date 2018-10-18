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
    internal class CatalogUpdateAlert : IChatCommand
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
                return "Avisar de una actualización en el catálogo del hotel.";
            }
        }
        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            string Message = CommandManager.MergeParams(Params, 1);
            RavenEnvironment.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("¡Actualización en el catálogo!",
              "¡El catálogo de <font color=\"#2E9AFE\"><b>Mabbi</b></font> acaba de ser actualizado! Si quieres observar <b>las novedades</b> sólo debes hacer click en el botón de abajo.<br>", "cata", "Ir a la página", "event:catalog/open/" + Message));

            Session.SendWhisper("Catalogo actualizado satisfactoriamente.");
        }
    }
}

