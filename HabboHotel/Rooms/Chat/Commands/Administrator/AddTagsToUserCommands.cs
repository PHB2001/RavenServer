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
    class AddTagsToUserCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_addtags"; }
        }

        public string Parameters
        {
            get { return "<usuario> <tag>"; }
        }

        public string Description
        {
            get { return "Añadir TAGs de un usuario."; }
        }

        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            if (Params.Length != 3)
            {
                Session.SendWhisper("Introduce el nombre del usuario a quien deseas enviar una placa!");
                return;
            }

            GameClient TargetClient = RavenEnvironment.GetGame().GetClientManager().GetClientByUsername(Params[1]);
            if (TargetClient != null)
            {
                using (IQueryAdapter dbClient = RavenEnvironment.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.runFastQuery("INSERT INTO `user_tags` (user_id, tag_name) VALUES(" + TargetClient.GetHabbo().Id +", '" + Params[2] + "')");

                    TargetClient.GetHabbo().Tags.Add(Params[2]);
                }

                Session.SendMessage(RoomNotificationComposer.SendBubble("definitions", "Has añadido el tag \"" + Params[2] + "\" a " + TargetClient.GetHabbo().Username + " correctamente.", ""));
                TargetClient.SendMessage(RoomNotificationComposer.SendBubble("definitions", Session.GetHabbo().Username + " te ha añadido el tag " + Params[2] +".", ""));

                foreach (RoomUser RoomUser in Room.GetRoomUserManager().GetRoomUsers())
                {

                    RoomUser.GetClient().SendMessage(new UserTagsComposer(TargetClient.GetHabbo().Id, TargetClient));
                }
            }
        }
    }
}
