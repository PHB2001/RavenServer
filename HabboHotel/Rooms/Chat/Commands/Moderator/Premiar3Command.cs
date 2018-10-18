using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Raven.HabboHotel.Rooms;
using Raven.HabboHotel.Pathfinding;
using Raven.HabboHotel.GameClients;
using Raven.Communication.Packets.Outgoing.Rooms.Chat;
using Raven.Communication.Packets.Outgoing.Rooms.Notifications;
using Raven.Communication.Packets.Outgoing.Inventory.Purse;
using Raven.Database.Interfaces;
using System.Data;
using Raven.Communication.Packets.Outgoing.Users;
using Raven.HabboHotel.Quests;
using Raven.Core;
using Raven.Communication.Packets.Outgoing.Nux;

namespace Raven.HabboHotel.Rooms.Chat.Commands.Moderator
{
    class Premiar3Command : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_premiar"; }
        }

        public string Parameters
        {
            get { return "%username% %username% %username%"; }
        }

        public string Description
        {
            get { return ""; }
        }


        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            if (Session.GetHabbo().Rank != 6 && Session.GetHabbo().Rank != 8 && Session.GetHabbo().Rank != 9 && Session.GetHabbo().Rank != 11 && Session.GetHabbo().TeamRank != 9)
            {
                Session.SendWhisper("No puedes dar premios por que no eres Manager, Game Master o Ayudante de Juego.");
                return;

            }

            if (Params.Length == 3)
            {
                Session.SendWhisper("Por favor introduce 3 nombres de usuarios para premiar.", 34);
                return;
            }


            GameClient Target = RavenEnvironment.GetGame().GetClientManager().GetClientByUsername(Params[1]);
            GameClient Target2 = RavenEnvironment.GetGame().GetClientManager().GetClientByUsername(Params[2]);
            GameClient Target3 = RavenEnvironment.GetGame().GetClientManager().GetClientByUsername(Params[3]);



            if (Target == null)
            {
                Session.SendWhisper("Oops, No se ha conseguido este usuario " + Params[1] + "!");
                return;
            }

            if (Target2 == null)
            {
                Session.SendWhisper("Oops, No se ha conseguido este usuario " + Params[2] + "!");
                return;
            }

            if (Target3 == null)
            {
                Session.SendWhisper("Oops, No se ha conseguido este usuario " + Params[3] + "!");
                return;
            }


            RoomUser TargetUser = Room.GetRoomUserManager().GetRoomUserByHabbo(Target.GetHabbo().Id);
            RoomUser TargetUser2 = Room.GetRoomUserManager().GetRoomUserByHabbo(Target2.GetHabbo().Id);
            RoomUser TargetUser3 = Room.GetRoomUserManager().GetRoomUserByHabbo(Target3.GetHabbo().Id);

            if (TargetUser == null)
            {
                Session.SendWhisper("Usuario no encontrado! " + Params[1] + " tal vez no esté en línea o en esta sala.");
                return;
            }

            if (TargetUser2 == null)
            {
                Session.SendWhisper("Usuario no encontrado! " + Params[2] + " tal vez no esté en línea o en esta sala.");
                return;
            }

            if (TargetUser3 == null)
            {
                Session.SendWhisper("Usuario no encontrado! " + Params[3] + " tal vez no esté en línea o en esta sala.");
                return;
            }



            if (Target.GetHabbo().Username == Session.GetHabbo().Username)
            {
                Session.SendWhisper("Men no seas estupid@, no puedes premiarte tu mismo!");
                return;
            }

            if (Target2.GetHabbo().Username == Session.GetHabbo().Username)
            {
                Session.SendWhisper("Men no seas estupid@, no puedes premiarte tu mismo!");
                return;
            }

            if (Target3.GetHabbo().Username == Session.GetHabbo().Username)
            {
                Session.SendWhisper("Men no seas estupid@, no puedes premiarte tu mismo!");
                return;
            }


            Target.SendMessage(RavenEnvironment.GetGame().GetNuxUserGiftsManager().NuxUserGifts.Serialize());
            Target2.SendMessage(RavenEnvironment.GetGame().GetNuxUserGiftsManager().NuxUserGifts.Serialize());
            Target3.SendMessage(RavenEnvironment.GetGame().GetNuxUserGiftsManager().NuxUserGifts.Serialize());

            // Notificacion de evento por Andre
            string figure = Target.GetHabbo().Look;
            string figure2 = Target2.GetHabbo().Look;
            string figure3 = Target3.GetHabbo().Look;

            RavenEnvironment.GetGame().GetClientManager().SendMessage(RoomNotificationComposer.SendBubble("fig/" + figure, TargetUser.GetUsername() + " ha ganado un evento en el hotel. Felicitaciones!", ""));
            Session.SendWhisper("Has activado correctamente el premio para " + Target.GetHabbo().Username, 34);

            RavenEnvironment.GetGame().GetClientManager().SendMessage(RoomNotificationComposer.SendBubble("fig/" + figure2, TargetUser2.GetUsername() + " ha ganado un evento en el hotel. Felicitaciones!", ""));
            Session.SendWhisper("Has activado correctamente el premio para " + Target2.GetHabbo().Username, 34);

            RavenEnvironment.GetGame().GetClientManager().SendMessage(RoomNotificationComposer.SendBubble("fig/" + figure3, TargetUser3.GetUsername() + " ha ganado un evento en el hotel. Felicitaciones!", ""));
            Session.SendWhisper("Has activado correctamente el premio para " + Target3.GetHabbo().Username, 34);


            foreach (RoomUser RoomUser in Room.GetRoomUserManager().GetUserList().ToList())
            {
                if (RoomUser == null || RoomUser.IsBot || RoomUser.GetClient() == null || RoomUser.GetClient().GetHabbo() == null || RoomUser.GetClient().GetHabbo().GetPermissions().HasRight("mod_tool") || RoomUser.GetClient().GetHabbo().Id == Session.GetHabbo().Id)
                    continue;

                RoomUser.GetClient().SendNotification("El evento se terminó, muchas gracias por participar, si quieres ganar muchos premios más, participa en otros eventos del hotel.");

                Room.GetRoomUserManager().RemoveUserFromRoom(RoomUser.GetClient(), true, false);
            }
        }
    }
}