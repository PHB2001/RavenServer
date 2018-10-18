using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Raven.HabboHotel.Rooms;
using Raven.HabboHotel.Rooms.Games;
using Raven.HabboHotel.Rooms.Games.Teams;

namespace Raven.HabboHotel.Rooms.Chat.Commands.User.Fun
{
    class EnableCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_enable"; }
        }

        public string Parameters
        {
            get { return "%EffectId%"; }
        }

        public string Description
        {
            get { return "Habilitar un efecto en tu personaje."; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Usted debe escribir un ID Efecto");
                return;
            }

            RoomUser ThisUser = Session.GetHabbo().CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Username);
            if (ThisUser == null)
                return;

            if (ThisUser.RidingHorse)
            {
                Session.SendWhisper("No se puede activar un efecto mientras montas un caballo");
                return;
            }
            else if (ThisUser.Team != TEAM.NONE)
                return;
            else if (ThisUser.isLying)
                return;

            int EffectId = 0;
            if (!int.TryParse(Params[1], out EffectId))
                return;

            if (EffectId > int.MaxValue || EffectId < int.MinValue)
                return;
            if (Session.GetHabbo().Rank > 8)
            {
                Session.GetHabbo().LastEffect = EffectId;
                Session.GetHabbo().Effects().ApplyEffect(EffectId);
                return;
            }

            // Staff Effects
            if (EffectId == 102 && Session.GetHabbo().Rank < 5 || EffectId == 602 && Session.GetHabbo().Rank < 5 || EffectId == 596 && Session.GetHabbo().Rank < 5 || EffectId == 598 && Session.GetHabbo().Rank < 5)
            { Session.SendWhisper("Lo sentimos, lamentablemente sólo los staff pueden activar este efecto."); return; }

            // Guide Effects
            if (EffectId == 592 && Session.GetHabbo().TeamRank != 3 || EffectId == 595 && Session.GetHabbo().TeamRank != 2 || EffectId == 597 && Session.GetHabbo().TeamRank != 1)
            { Session.SendWhisper("Lo sentimos, no perteneces al equipo guía, es por ello que no puedes usar este efecto."); return; }

            // Croupier Effect
            if (EffectId == 594 && Session.GetHabbo().TeamRank != 8 || EffectId == 777 && Session.GetHabbo().TeamRank != 8)
            { Session.SendWhisper("Lo sentimos, este enable es sólo para el equipo Croupier de " + RavenEnvironment.GetDBConfig().DBData["hotel.name"] + "."); return; }

            // BAW Effect
            if (EffectId == 599 && Session.GetHabbo().TeamRank != 7)
            { Session.SendWhisper("Lo sentimos, este enable es sólo para el equipo BAW de " + RavenEnvironment.GetDBConfig().DBData["hotel.name"] + "."); return; }

            // Publicista Effect
            if (EffectId == 600 && Session.GetHabbo().TeamRank != 4 || EffectId == 601 && Session.GetHabbo().TeamRank != 4)
            { Session.SendWhisper("Lo sentimos, este enable es sólo para los publicistas."); return; }

            // VIP Effect
            if (EffectId == 593 && Session.GetHabbo().Rank < 2)
            { Session.SendWhisper("Lo sentimos, este enable es sólo para los VIP."); return; }

            // Ambassador & Rookies Effect
            if (EffectId == 178 && Session.GetHabbo().Rank < 3 || EffectId == 187 && Session.GetHabbo().Rank < 3)
            { Session.SendWhisper("Lo sentimos, este enable es sólo para los embajadores y rookies."); return; }

      
            Session.GetHabbo().LastEffect = EffectId;
            Session.GetHabbo().Effects().ApplyEffect(EffectId);
        }
    }
}
