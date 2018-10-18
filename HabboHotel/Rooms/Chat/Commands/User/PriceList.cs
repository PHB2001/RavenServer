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
    class PriceList : IChatCommand
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
            get { return "Ver la lista de precios de raros."; }
        }

        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            StringBuilder List = new StringBuilder("");
            List.AppendLine("                          ¥ LISTA DE PRECIOS DE MABBI¥");
            List.AppendLine("   SOFÁ VIP: Duckets   »   SOFÁ VIP: Duckets   »   SOFÁ VIP: Duckets");
            List.AppendLine("   SOFÁ VIP: Duckets   »   SOFÁ VIP: Duckets   »   SOFÁ VIP: Duckets");
            List.AppendLine("   SOFÁ VIP: Duckets   »   SOFÁ VIP: Duckets   »   SOFÁ VIP: Duckets");
            List.AppendLine("   SOFÁ VIP: Duckets   »   SOFÁ VIP: Duckets   »   SOFÁ VIP: Duckets");
            List.AppendLine("   SOFÁ VIP: Duckets   »   SOFÁ VIP: Duckets   »   SOFÁ VIP: Duckets");
            List.AppendLine("   SOFÁ VIP: Duckets   »   SOFÁ VIP: Duckets   »   SOFÁ VIP: Duckets");
            List.AppendLine("Esta lista todavía está en construcción por andre, su última actualización fue el día 28 de Julio de 2016.");
            Session.SendMessage(new MOTDNotificationComposer(List.ToString()));


        }
    }
}