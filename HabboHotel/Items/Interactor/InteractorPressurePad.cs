using System;

using Raven.HabboHotel.Items;
using Raven.HabboHotel.Rooms;
using Raven.HabboHotel.Rooms.Games;
using Raven.HabboHotel.GameClients;
using Raven.HabboHotel.Rooms.Games.Teams;
using Raven.Communication.Packets.Outgoing.Rooms.Notifications;

namespace Raven.HabboHotel.Items.Interactor
{
    class InteractorPressurePad : IFurniInteractor
    {
        public void OnPlace(GameClient Session, Item Item)
        {
        }

        public void OnRemove(GameClient Session, Item Item)
        {
            if (Session == null || Session.GetHabbo() == null || Item == null)
                return;

            Room Room = Session.GetHabbo().CurrentRoom;
            if (Room == null)
                return;

            RoomUser Actor = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (Actor == null)
                return;

            if (Gamemap.TileDistance(Actor.X, Actor.Y, Item.GetX, Item.GetY) > 2)
                return;

            int count = int.Parse(Item.ExtraData);
            count++;
            Item.ExtraData = count + "";
            Item.UpdateState(true, true);
        }

        public void OnTrigger(GameClients.GameClient Session, Item Item, int Request, bool HasRights)
        {
            if (Session == null || Session.GetHabbo() == null || Item == null)
                return;

            Room Room = Session.GetHabbo().CurrentRoom;
            if (Room == null)
                return;

            RoomUser Actor = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (Actor == null)
                return;

            if (Gamemap.TileDistance(Actor.X, Actor.Y, Item.GetX, Item.GetY) > 2)
                return;

            int count = int.Parse(Item.ExtraData);
            count++;
            Item.ExtraData = count + "";
            Item.UpdateState(true, true);
        }

        public void OnWiredTrigger(Item Item)
        {

        }
    }
}