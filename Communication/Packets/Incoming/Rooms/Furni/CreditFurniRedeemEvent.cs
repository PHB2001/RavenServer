using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Raven.HabboHotel.Rooms;
using Raven.HabboHotel.Items;
using Raven.Communication.Packets.Outgoing.Inventory.Purse;
using Raven.Communication.Packets.Outgoing.Inventory.Furni;

using Raven.Database.Interfaces;
using Raven.Communication.Packets.Outgoing.Rooms.Notifications;

namespace Raven.Communication.Packets.Incoming.Rooms.Furni
{
    class CreditFurniRedeemEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (!Session.GetHabbo().InRoom)
                return;

            Room Room;

            if (!RavenEnvironment.GetGame().GetRoomManager().TryGetRoom(Session.GetHabbo().CurrentRoomId, out Room))
                return;

            if (!Room.CheckRights(Session, true))
                return;

            if (RavenEnvironment.GetDBConfig().DBData["exchange_enabled"] != "1")
            {
                Session.SendNotification("De momento no puedes canjear tus monedas para llevarlo a su monedero.");
                return;
            }

            Item Exchange = Room.GetRoomItemHandler().GetItem(Packet.PopInt());
            if (Exchange == null)
                return;

            if (Exchange.GetBaseItem().ItemName.StartsWith("CF_"))
            {

                string[] Split = Exchange.GetBaseItem().ItemName.Split('_');
                int Value = int.Parse(Split[1]);

                if (Value > 0)
                {
                    Session.GetHabbo().Credits += Value;
                    Session.SendMessage(new CreditBalanceComposer(Session.GetHabbo().Credits));
                }
            }

            if (Exchange.GetBaseItem().ItemName.StartsWith("DF_"))
            {

                string[] Split = Exchange.GetBaseItem().ItemName.Split('_');
                int Value = int.Parse(Split[1]);

                if (Value > 0)
                {
                    Session.GetHabbo().Diamonds += Value;
                    Session.SendMessage(new HabboActivityPointNotificationComposer(Session.GetHabbo().Diamonds, Value, 5));
                    Session.SendMessage(RoomNotificationComposer.SendBubble("diamonds", "Has convertido correctamente " + Value + " Diamonds, haz click aquí para abrir la tienda.", "catalog/open"));
                }
            }

            if (Exchange.GetBaseItem().ItemName.StartsWith("CFC_"))
            {

                string[] Split = Exchange.GetBaseItem().ItemName.Split('_');
                int Value = int.Parse(Split[1]);

                if (Value > 0)
                {
                    Session.GetHabbo().Duckets += Value;
                    Session.SendMessage(new HabboActivityPointNotificationComposer(Session.GetHabbo().Duckets, Value));
                    Session.SendMessage(RoomNotificationComposer.SendBubble("duckets", "Has convertido correctamente " + Value + " Duckets, haz click aquí para abrir la tienda.", "catalog/open"));
                }
            }

            if (Exchange.GetBaseItem().ItemName.StartsWith("CFG_"))
            {

                string[] Split = Exchange.GetBaseItem().ItemName.Split('_');
                int Value = int.Parse(Split[1]);

                if (Value > 0)
                {
                    Session.GetHabbo().GOTWPoints += Value;
                    Session.SendMessage(new HabboActivityPointNotificationComposer(Session.GetHabbo().GOTWPoints, Value, 103));
                    Session.SendMessage(RoomNotificationComposer.SendBubble("gotwpoints", "Has convertido correctamente " + Value + " Puntos de juego, haz click aquí para abrir la tienda.", "catalog/open"));
                }
            }

            using (IQueryAdapter dbClient = RavenEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunQuery("DELETE FROM `items` WHERE `id` = '" + Exchange.Id + "' LIMIT 1");
            }

            Session.SendMessage(new FurniListUpdateComposer());
            Room.GetRoomItemHandler().RemoveFurniture(null, Exchange.Id, false);
            Session.GetHabbo().GetInventoryComponent().RemoveItem(Exchange.Id);

        }
    }
}
