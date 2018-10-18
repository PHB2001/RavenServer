using Raven.Communication.Packets.Incoming.Rooms.Camera;
using Raven.Communication.Packets.Outgoing.Inventory.Furni;
using Raven.Communication.Packets.Outgoing.Inventory.Purse;
using Raven.Communication.Packets.Outgoing.Rooms.Camera;
using Raven.Communication.Packets.Outgoing.Rooms.Notifications;
using Raven.Database.Interfaces;
using Raven.HabboHotel.GameClients;
using Raven.HabboHotel.Items;

namespace Raven.Communication.Packets.Incoming.Catalog
{
    public class BuyServerCameraPhoto : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket paket)
        {
            if (!Session.GetHabbo().lastPhotoPreview.Contains("-"))
                return;

            if (Session.GetHabbo().Duckets < 1)
            {
                Session.SendMessage(RoomNotificationComposer.SendBubble("camera", "Necesitas tener al menos 1 Ducket para adquirir una foto de Mabbi.", ""));
                return;
            }

            string roomId = Session.GetHabbo().lastPhotoPreview.Split('-')[0];
            string timestamp = Session.GetHabbo().lastPhotoPreview.Split('-')[1];
            string md5image = URLPost.GetMD5(Session.GetHabbo().lastPhotoPreview);
            ItemData Item = null;
            if (!RavenEnvironment.GetGame().GetItemManager().GetItem(8763, out Item))
                return;
            if (Item == null)
                return;


            Item photoPoster = ItemFactory.CreateSingleItemNullable(Item, Session.GetHabbo(), "{\"timestamp\":\"" + timestamp + "\", \"id\":\"" + md5image + "\"}", "");

            if (photoPoster != null)
            {
                Session.GetHabbo().GetInventoryComponent().TryAddItem(photoPoster);

                Session.SendMessage(new FurniListAddComposer(photoPoster));
                Session.SendMessage(new FurniListUpdateComposer());
                Session.SendMessage(new FurniListNotificationComposer(photoPoster.Id, 1));
                Session.GetHabbo().Duckets--;
                Session.SendMessage(new HabboActivityPointNotificationComposer(Session.GetHabbo().Duckets, -1));
                
                RavenEnvironment.GetGame().GetAchievementManager().ProgressAchievement(Session, "ACH_CameraPhotoCount", 1);
            }

            Session.SendMessage(new BuyPhoto());

            Session.GetHabbo().GetInventoryComponent().UpdateItems(false);

            using (IQueryAdapter dbClient = RavenEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("INSERT INTO items_camera VALUES (@id, '" + Session.GetHabbo().Id + "',@creator_name, '" + roomId + "','" + timestamp + "')");
                dbClient.AddParameter("id", md5image);
                dbClient.AddParameter("creator_name", Session.GetHabbo().Username);
                dbClient.RunQuery();
            }
        }
    }
}
