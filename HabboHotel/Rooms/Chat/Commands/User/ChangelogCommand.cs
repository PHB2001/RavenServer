using System;
using Raven.HabboHotel.GameClients;
using Raven.Communication.Packets.Outgoing.Rooms.Notifications;

namespace Raven.HabboHotel.Rooms.Chat.Commands.User
{
    class ChangelogCommand : IChatCommand
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
            get { return "Últimas actualizaciones de Raven."; }
        }

        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            var _cache = new Random().Next(0, 300);
            Session.SendMessage(new MassEventComposer("habbopages/changelogs.txt?" + _cache));
        }
    }
}