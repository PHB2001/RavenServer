using Raven.Communication.Packets.Outgoing.Inventory.Purse;
using Raven.Database.Interfaces;
using Raven.HabboHotel.GameClients;
using Raven.HabboHotel.Users;

namespace Raven.HabboHotel.Rooms.Chat.Commands.User
{
    class BuyRoomCommand : IChatCommand
    {
        public string Description
        {
            get { return "Compra una sala en venta de cualquier usuario."; }
        }

        public string Parameters
        {
            get { return ""; }
        }

        public string PermissionRequired
        {
            get { return "command_buy_room"; }
        }

        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            Room _Room = Session.GetHabbo().CurrentRoom;
            RoomUser RoomOwner = _Room.GetRoomUserManager().GetRoomUserByHabbo(_Room.OwnerName);
            if (_Room == null)
            {
                return;
            }
            if (_Room.OwnerName == Session.GetHabbo().Username)
            {
                Session.SendWhisper("Estás intentando comprar una sala que ya es tuya.", 34);
                return;
            }

            if (!Room.ForSale)
            {
                Session.SendWhisper("¡Esta sala no está en venta!", 34);
                return;
            }

            if (Session.GetHabbo().Diamonds < _Room.SalePrice)
            {
                Session.SendWhisper("¡No tiene suficientes Diamantes para comprar esta sala!", 34);
                return;
            }

            if (RoomOwner == null || RoomOwner.GetClient() == null)
            {
                Session.SendWhisper("Se ha producido un error. Esta sala no está en venta.", 34);
                _Room.ForSale = false;
                _Room.SalePrice = 0;
                return;
            }

            GameClient Owner = RoomOwner.GetClient();

            Owner.GetHabbo().Diamonds += _Room.SalePrice;
            Owner.SendMessage(new HabboActivityPointNotificationComposer(Owner.GetHabbo().Diamonds, _Room.SalePrice));
            Session.GetHabbo().Diamonds -= _Room.SalePrice;
            Session.SendMessage(new HabboActivityPointNotificationComposer(Session.GetHabbo().Diamonds, _Room.SalePrice));

            _Room.OwnerName = Session.GetHabbo().Username;
            _Room.OwnerId = (int)Session.GetHabbo().Id;
            _Room.RoomData.OwnerName = Session.GetHabbo().Username;
            _Room.RoomData.OwnerId = (int)Session.GetHabbo().Id;
            int RoomId = _Room.RoomId;



            using (IQueryAdapter dbClient = RavenEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunQuery("UPDATE rooms SET owner='" + Session.GetHabbo().Id + "' WHERE id='" + Room.RoomId + "' LIMIT 1");
                dbClient.RunQuery("UPDATE items SET user_id='" + Session.GetHabbo().Id + "' WHERE room_id='" + Room.RoomId + "'");
            }

            Session.GetHabbo().UsersRooms.Add(_Room.RoomData);
            Owner.GetHabbo().UsersRooms.Remove(_Room.RoomData);
            RavenEnvironment.GetGame().GetRoomManager().UnloadRoom(_Room);

            RoomData Data = RavenEnvironment.GetGame().GetRoomManager().GenerateRoomData(RoomId);
            Session.GetHabbo().PrepareRoom(Session.GetHabbo().CurrentRoom.RoomId, "");

        }
    }
}
