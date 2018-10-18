using Raven.Communication.Packets.Outgoing.Rooms.Notifications;
using Raven.Database.Interfaces;
using Raven.HabboHotel.GameClients;

namespace Raven.HabboHotel.Rooms.Chat.Commands.User
{
    class SellRoomCommand : IChatCommand
    {
        public string Description
        {
            get { return "Poner en venta la sala en que te encuentras."; }
        }

        public string Parameters
        {
            get { return "%precio%"; }
        }

        public string PermissionRequired
        {
            get { return "command_sell_room"; }
        }


        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Debes poner un precio.");
                return;
            }

            if (!Room.CheckRights(Session, true))
                return;

            if (Room == null)

                if (Params.Length == 1)
                {
                    Session.SendWhisper("Oops, Se olvido de elegir un precio para vender esta sala.");
                    return;
                }
                else if (Room.Group != null)
                {
                    Session.SendWhisper("Oops, al parecer esta sala tiene un grupo, asi no se podra vender, primero debe eliminar el grupo.");
                    return;
                }

            int Value = 0;
            if (!int.TryParse(Params[1], out Value))
            {
                Session.SendWhisper("Oops, estas introduciendo un valor que no es correcto");
                return;
            }

            if (Value < 0)
            {
                Session.SendWhisper("No puede vender una sala con un valor numerico Negativo.");
                return;
            }

            if (Room.ForSale)
            {
                Room.SalePrice = Value;
            }
            else
            {
                Room.ForSale = true;
                Room.SalePrice = Value;
            }

            foreach (RoomUser User in Room.GetRoomUserManager().GetRoomUsers())
            {
                if (User == null || User.GetClient() == null)
                    continue;

                Session.SendWhisper("¡Esta sala está en venta, su precio actual es de:  " + Value + " Diamantes! Cómprala escribiendo :comprarsala.");
            }

            Session.SendNotification("Si usted quiere vender su sala, debe incluir un valor numerico. \n\nPOR FAVOR NOTA:\nSi usted vende una sala, no la puede Recuperar de nuevo.!\n\n" +
        "Usted puede cancelar la venta de una habitación escribiendo ':unload' (sin las '')");

            return;
        }
    }
}