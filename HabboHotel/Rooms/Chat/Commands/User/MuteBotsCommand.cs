﻿using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Raven.Database.Interfaces;


namespace Raven.HabboHotel.Rooms.Chat.Commands.User
{
    class MuteBotsCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_mute_bots"; }
        }

        public string Parameters
        {
            get { return ""; }
        }

        public string Description
        {
            get { return "Silenciar todo lo que digan los BOTs."; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            Session.GetHabbo().AllowBotSpeech = !Session.GetHabbo().AllowBotSpeech;
            using (IQueryAdapter dbClient = RavenEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunQuery("UPDATE `users` SET `bots_muted` = '" + ((Session.GetHabbo().AllowBotSpeech) ? 1 : 0) + "' WHERE `id` = '" + Session.GetHabbo().Id + "' LIMIT 1");
            }

            if (Session.GetHabbo().AllowBotSpeech)
                Session.SendWhisper("Cambio realizado, ahora no puedes escuchar lo que dicen los Bots");
            else
                Session.SendWhisper("Cambio realizado, ahora puedes escuchar lo que dicen los Bots.");                       
        }
    }
}
