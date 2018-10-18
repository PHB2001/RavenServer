using Raven.Communication.Packets.Outgoing.Rooms.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.HabboHotel.Rooms.Chat.Commands.User
{
    class CasinoCommand : IChatCommand
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
            get { return "start/pl. Mantiene la cuenta de tu juego de da2."; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Oops, debes especificar si quieres comenzar modo casino o dar pl! Escribe :casino start o :casino pl", 34);
                return;
            }
            string query = Params[1];

            RoomUser roomUser = Room?.GetRoomUserManager()?.GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (roomUser == null)
            {
                return;
            }

            List<Items.Item> userBooth = Room.GetRoomItemHandler().GetFloor.Where(x => x != null && Gamemap.TilesTouching(
                x.Coordinate, roomUser.Coordinate) && x.Data.InteractionType == Items.InteractionType.DICE).ToList();

            if (userBooth.Count != 5)
            {
                Session.SendWhisper("Debes tener 5 dados cerca para iniciar un juego de da2", 34);
                return;
            }

            if (query == "pl" || query == "PL")
            {
                Room.SendMessage(RoomNotificationComposer.SendBubble("ganador", "El usuario " + Session.GetHabbo().Username + " ha sacado " + Session.GetHabbo().casinoCount + " en los dados (PL Automatico)", ""));
                Session.GetHabbo().casinoEnabled = false;
                Session.GetHabbo().casinoCount = 0;
            }
            else if (query == "start" || query == "START")
            {
                Session.SendWhisper("Has iniciado el modo casino. El contador de dados esta activado", 34);
                Session.GetHabbo().casinoEnabled = true;

            }
            else
            {
                Session.SendWhisper("Oops, debes especificar si quieres comenzar modo casino o dar pl! Escribe :casino start o :casino pl");
            }


        }
    }
}