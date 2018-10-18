using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Raven.Database.Interfaces;
using Raven.Utilities;
using Raven.HabboHotel.Users;
using Raven.HabboHotel.GameClients;


using Raven.HabboHotel.Moderation;

namespace Raven.HabboHotel.Rooms.Chat.Commands.Moderator
{
    class MIPCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_mip"; }
        }

        public string Parameters
        {
            get { return "%username%"; }
        }

        public string Description
        {
            get { return "Machine Ban, banea a un usuario con IP"; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {

            if (Params.Length == 1)
            {
                Session.SendWhisper("Por favor introduce el Nombre o la IP del usuario a Banear.");
                return;
            }

            Habbo Habbo = RavenEnvironment.GetHabboByUsername(Params[1]);
            if (Habbo == null)
            {
                Session.SendWhisper("Ocurrio un error en la busqueda por la base de datos.");
                return;
            }

            if (Habbo.GetPermissions().HasRight("mod_tool") && !Session.GetHabbo().GetPermissions().HasRight("mod_ban_any") && Session.GetHabbo().Username != "Anegrado" && Session.GetHabbo().Username != "lKarus")
            {
                Session.SendWhisper("Oops, you cannot ban that user.");
                return;
            }

            String IPAddress = String.Empty;
            Double Expire = RavenEnvironment.GetUnixTimestamp() + 78892200;
            string Username = Habbo.Username;

            using (IQueryAdapter dbClient = RavenEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunQuery("UPDATE `user_info` SET `bans` = `bans` + '1' WHERE `user_id` = '" + Habbo.Id + "' LIMIT 1");

                dbClient.SetQuery("SELECT `ip_last` FROM `users` WHERE `id` = '" + Habbo.Id + "' LIMIT 1");
                IPAddress = dbClient.getString();
            }

            string Reason = null;
            if (Params.Length >= 3)
                Reason = CommandManager.MergeParams(Params, 2);
            else
                Reason = "No se especifico la razon";

            if (!string.IsNullOrEmpty(IPAddress))
                RavenEnvironment.GetGame().GetModerationManager().BanUser(Session.GetHabbo().Username, ModerationBanType.IP, IPAddress, Reason, Expire);
            RavenEnvironment.GetGame().GetModerationManager().BanUser(Session.GetHabbo().Username, ModerationBanType.USERNAME, Habbo.Username, Reason, Expire);

            if (!string.IsNullOrEmpty(Habbo.MachineId))
                RavenEnvironment.GetGame().GetModerationManager().BanUser(Session.GetHabbo().Username, ModerationBanType.MACHINE, Habbo.MachineId, Reason, Expire);

            GameClient TargetClient = RavenEnvironment.GetGame().GetClientManager().GetClientByUsername(Username);
            if (TargetClient != null)
                TargetClient.Disconnect();
            Session.SendWhisper("Se ha baneado exitosamente al usuario '" + Username + "' por la siguiente razon: '" + Reason + "'!");
        }
    }
}