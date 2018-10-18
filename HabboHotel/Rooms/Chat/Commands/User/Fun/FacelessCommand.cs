using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Raven.HabboHotel.Rooms;
using Raven.Communication.Packets.Outgoing.Rooms.Engine;
using Raven.Database.Interfaces;


namespace Raven.HabboHotel.Rooms.Chat.Commands.User.Fun
{
    class FacelessCommand :IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_faceless"; }
        }

        public string Parameters
        {
            get { return ""; }
        }

        public string Description
        {
            get { return "Eliminar la cara de tu personaje."; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
    
            RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (User == null || User.GetClient() == null)
                return;

            string[] headParts;
            string[] figureParts = Session.GetHabbo().Look.Split('.');
            foreach (string Part in figureParts)
            {
                if (Part.StartsWith("hd"))
                {
                    headParts = Part.Split('-');
                    if (!headParts[1].Equals("99999"))
                        headParts[1] = "99999";
                    else
                        return;

                    Session.GetHabbo().Look = Session.GetHabbo().Look.Replace(Part, "hd-" + headParts[1] + "-" + headParts[2]);
                    break;
                }
            }
            Session.GetHabbo().Look = RavenEnvironment.GetGame().GetAntiMutant().RunLook(Session.GetHabbo().Look);
            using (IQueryAdapter dbClient = RavenEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunQuery("UPDATE `users` SET `look` = '" + Session.GetHabbo().Look + "' WHERE `id` = '" + Session.GetHabbo().Id + "' LIMIT 1");
            }

            Session.SendMessage(new UserChangeComposer(User, true));
            Session.GetHabbo().CurrentRoom.SendMessage(new UserChangeComposer(User, false));
            return;
        }
    }
}
