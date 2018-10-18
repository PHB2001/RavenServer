using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Raven.HabboHotel.Rooms;
using Raven.HabboHotel.GameClients;

namespace Raven.HabboHotel.Rooms.Chat.Commands.Moderator.Fun
{
    class FreezeCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_freeze"; }
        }

        public string Parameters
        {
            get { return "%username%"; }
        }

        public string Description
        {
            get { return "Congela a un usuario y no podra moverse"; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Por favor introduce el nombre de quien quieres congelar.");
                return;
            }

            GameClient TargetClient = RavenEnvironment.GetGame().GetClientManager().GetClientByUsername(Params[1]);
            if (TargetClient == null)
            {
                Session.SendWhisper("Ocurrio un error, al parecer no se consigue el usuario o no se encuentra online");
                return;
            }

            RoomUser TargetUser = Session.GetHabbo().CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(Params[1]);
            if (TargetUser != null)
                TargetUser.Frozen = true;

            Session.SendWhisper("Congelado correctamente " + TargetClient.GetHabbo().Username + "!");
        }
    }
}
