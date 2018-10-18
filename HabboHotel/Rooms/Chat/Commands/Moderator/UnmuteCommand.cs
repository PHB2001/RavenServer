using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Raven.Database.Interfaces;
using Raven.Utilities;
using Raven.HabboHotel.GameClients;


namespace Raven.HabboHotel.Rooms.Chat.Commands.Moderator
{
    class UnmuteCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_unmute"; }
        }

        public string Parameters
        {
            get { return "%username%"; }
        }

        public string Description
        {
            get { return "Desmutear un usuario"; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Introduce el nombre del usuario que deseas desmutear..");
                return;
            }

            GameClient TargetClient = RavenEnvironment.GetGame().GetClientManager().GetClientByUsername(Params[1]);
            if (TargetClient == null || TargetClient.GetHabbo() == null)
            {
                Session.SendWhisper("Ocurrio un error, escribe correctamente el nombre o no se encuentra online.");
                return;
            }

            using (IQueryAdapter dbClient = RavenEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunQuery("UPDATE `users` SET `time_muted` = '0' WHERE `id` = '" + TargetClient.GetHabbo().Id + "' LIMIT 1");
            }

            TargetClient.GetHabbo().TimeMuted = 0;
            TargetClient.SendNotification("Usted ha sido desmuteado por " + Session.GetHabbo().Username + "!");
            Session.SendWhisper("Acabas de desmutear a  " + TargetClient.GetHabbo().Username + "!");
        }
    }
}