using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Raven.HabboHotel.Rooms;
using Raven.HabboHotel.Rooms.Music;
using Raven.HabboHotel.Items;
using Raven.Communication.Packets.Outgoing.Rooms.Music;
using Raven.Database.Interfaces;

namespace Raven.Communication.Packets.Incoming.Rooms.Music
{
    class AddPlaylistItemEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            Room Instance = Session.GetHabbo().CurrentRoom;

            if (Instance == null || Instance.GetRoomMusicManager().PlaylistSize >= MusicManager.PlaylistCapacity)
                return;

            int Itemid = Packet.PopInt();

            Item DiskItem = Session.GetHabbo().GetInventoryComponent().GetItem(Itemid);

            if (DiskItem == null || DiskItem.GetBaseItem().InteractionType != InteractionType.MUSIC_DISC)
                return;

            SongItem SongItem = new SongItem(DiskItem);

            if (Instance.GetRoomMusicManager().AddDisk(SongItem) >= 0)
            {
                SongItem.SaveToDatabase(Instance.RoomId);
                Session.GetHabbo().GetInventoryComponent().RemoveItem(DiskItem.Id);

                List<SongInstance> list = Instance.GetRoomMusicManager().Playlist.Values.ToList();

                Session.SendMessage(new GetJukeboxPlaylistsComposer(MusicManager.PlaylistCapacity, list));
                Session.SendMessage(new GetJukeboxDisksComposer(Session.GetHabbo().GetInventoryComponent().songDisks));

                list.Clear();
                list = null;
            }
        }
    }
}
