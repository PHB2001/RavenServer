﻿using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Raven.HabboHotel.Rooms;
using Raven.HabboHotel.Items;
using Raven.Communication.Packets.Outgoing.Inventory.Furni;
using Raven.Database.Interfaces;
using Raven.HabboHotel.Users;
using Raven.HabboHotel.GameClients;
using Raven.Communication.Packets.Outgoing.Notifications;

namespace Raven.HabboHotel.Rooms.Chat.Commands.User
{
    class StaffInfo : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_staffinfo"; }
        }

        public string Parameters
        {
            get { return ""; }
        }

        public string Description
        {
            get { return "Observa una lista de todos los staffs conectados."; }
        }

        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            Dictionary<Habbo, UInt32> clients = new Dictionary<Habbo, UInt32>();

            StringBuilder content = new StringBuilder();
            content.Append("Estado de los Staff conectados en " + RavenEnvironment.GetConfig().data["hotel.name"] + ":\r\n");

            foreach (var client in RavenEnvironment.GetGame().GetClientManager()._clients.Values)
            {
                if (client != null && client.GetHabbo() != null && client.GetHabbo().Rank > 3)
                    clients.Add(client.GetHabbo(), (Convert.ToUInt16(client.GetHabbo().Rank)));
            }

            foreach (KeyValuePair<Habbo, UInt32> client in clients.OrderBy(key => key.Value))
            {
                if (client.Key == null)
                    continue;

                content.Append("¥ " + client.Key.Username + " [Rango: " + client.Key.Rank + "] - Se encuentra en la sala: " + ((client.Key.CurrentRoom == null) ? "En ninguna sala." : client.Key.CurrentRoom.RoomData.Name) + "\r\n");
            }

            Session.SendMessage(new MOTDNotificationComposer(content.ToString()));

            return;
        }
    }
}