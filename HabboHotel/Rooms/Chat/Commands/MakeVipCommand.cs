using Raven.Communication.Packets.Outgoing.Rooms.Notifications;
using Raven.HabboHotel.GameClients;
using Raven.Database.Interfaces;

namespace Raven.HabboHotel.Rooms.Chat.Commands
{
    internal class MakeVipCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_makevip"; }
        }

        public string Parameters
        {
            get { return "%username% %days%"; }
        }

        public string Description
        {
            get { return "Dale VIP a un usuario"; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Por favor introduce el nombre del usuario al que le enviaras la alerta");
                return;
            }

            GameClient TargetClient = RavenEnvironment.GetGame().GetClientManager().GetClientByUsername(Params[1]);
            if (TargetClient == null)
            {
                Session.SendWhisper("Ocurrio un error, al parecer no se consigue el usuario o no se encuentra online");
                return;
            }

            int Days = int.Parse(CommandManager.MergeParams(Params, 2));

            if (Days > 31)
            {
                Session.SendWhisper("Ocurrio un error, no puedes entregar tantos días al mismo usuario.");
                return;
            }

            using (IQueryAdapter dbClient = RavenEnvironment.GetDatabaseManager().GetQueryReactor())
            {

                dbClient.RunQuery("UPDATE `users` SET `rank` = '2' WHERE `id` = '" + TargetClient.GetHabbo().Id + "' LIMIT 1");
                dbClient.RunQuery("UPDATE `users` SET `rank_vip` = '1' WHERE `id` = '" + TargetClient.GetHabbo().Id + "' LIMIT 1");
               
            }
            TargetClient.GetHabbo().GetClubManager().AddOrExtendSubscription("club_vip", Days * 24 * 3600, Session);
            TargetClient.SendMessage(new AlertNotificationHCMessageComposer(5));
            TargetClient.SendMessage(RoomNotificationComposer.SendBubble("eventoxx", "Reinicia para aplicar los cambios respectivos (F5).", ""));
            TargetClient.GetHabbo().GetBadgeComponent().GiveBadge("DVIP", true, TargetClient);
        }

    }
}