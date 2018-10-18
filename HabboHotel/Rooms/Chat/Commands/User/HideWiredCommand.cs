using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Raven.Communication.Packets.Outgoing.Inventory.Furni;
using Raven.HabboHotel.Global;
using System.Globalization;
using Raven.Database.Interfaces;
using Raven.Communication.Packets.Outgoing;
using Raven.HabboHotel.Items;
using Raven.Communication.Packets.Outgoing.Rooms.Engine;
using Raven.Communication.Packets.Outgoing.Rooms.Chat;
using Raven.HabboHotel.GameClients;

namespace Raven.HabboHotel.Rooms.Chat.Commands.User
{
    class HideWiredCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return ""; }
        }

        public string Parameters
        {
            get { return ""; }
        }

        public string Description
        {
            get { return "Esconde los furnis Wired de tu sala."; }
        }

        public void Execute(GameClient Session, Room Room, string[] Params)
        {

            if (!Room.CheckRights(Session, false, false))
            {
                Session.SendWhisper("No tienes permisos en esta sala.");
                return;
            }

            Room.HideWired = !Room.HideWired;
            if (Room.HideWired)
                Session.SendWhisper("Has escondido todos los Wired de la sala.");
            else
                Session.SendWhisper("Has mostrado todos los Wired de la sala.");

            using (IQueryAdapter con = RavenEnvironment.GetDatabaseManager().GetQueryReactor())
           {
               con.SetQuery("UPDATE `rooms` SET `hide_wired` = @enum WHERE `id` = @id LIMIT 1");
              con.AddParameter("enum", RavenEnvironment.BoolToEnum(Room.HideWired));
              con.AddParameter("id", Room.Id);
               con.RunQuery();
           }

            List<ServerPacket> list = new List<ServerPacket>();

            list = Room.HideWiredMessages(Room.HideWired);

            Room.SendMessage(list);


        }
    }
}
