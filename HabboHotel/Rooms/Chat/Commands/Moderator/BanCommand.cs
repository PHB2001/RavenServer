using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Raven.Utilities;
using Raven.HabboHotel.Users;
using Raven.HabboHotel.GameClients;


using Raven.HabboHotel.Moderation;

using Raven.Database.Interfaces;

namespace Raven.HabboHotel.Rooms.Chat.Commands.Moderator
{
    class BanCommand : IChatCommand
    {

        public string PermissionRequired
        {
            get { return "command_ban"; }
        }

        public string Parameters
        {
            get { return "%usuario% %duración% %razón% "; }
        }

        public string Description
        {
            get { return "Realiza una petición de baneo."; ; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Por favor introduzca el nombre del usuario.");
                return;
            }

            Habbo Habbo = RavenEnvironment.GetHabboByUsername(Params[1]);
            if (Habbo == null)
            {
                Session.SendWhisper("El usuario " + Params[1] + " no existe.");
                return;
            }

            if (Habbo.GetPermissions().HasRight("mod_soft_ban") && !Session.GetHabbo().GetPermissions().HasRight("mod_ban_any") && Session.GetHabbo().Username != "Anegrado" && Session.GetHabbo().Username != "lKarus")
            {
                Session.SendWhisper("Vaya... al parecer no puedes banear a " + Params[1] +".");
                return;
            }

            Double Expire = 0;
            string Hours = Params[2];
            if (String.IsNullOrEmpty(Hours) || Hours == "perm")
                Expire = RavenEnvironment.GetUnixTimestamp() + 78892200;
            else
                Expire = (RavenEnvironment.GetUnixTimestamp() + (Convert.ToDouble(Hours) * 3600));

            string Reason = null;
            if (Params.Length >= 4)
                Reason = CommandManager.MergeParams(Params, 3);
            else
                Reason = "Sin razón.";

            string Username = Habbo.Username;
            using (IQueryAdapter dbClient = RavenEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunQuery("UPDATE `user_info` SET `bans` = `bans` + '1' WHERE `user_id` = '" + Habbo.Id + "' LIMIT 1");
            }

            RavenEnvironment.GetGame().GetModerationManager().BanUser(Session.GetHabbo().Username, ModerationBanType.USERNAME, Habbo.Username, Reason, Expire);

            GameClient TargetClient = RavenEnvironment.GetGame().GetClientManager().GetClientByUsername(Username);
            if (TargetClient != null)
                TargetClient.Disconnect();

            Session.SendWhisper("Excelente, ha sido baneado el usuario '" + Username + "' por " + Hours + " hhora(s) con la razon '" + Reason + "'!");
        }
    }
}