using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using Raven.HabboHotel.GameClients;
using Raven.HabboHotel.Rooms;
using Raven.HabboHotel.Users;
using Raven.Communication.Packets.Incoming;
using System.Collections.Concurrent;

using Raven.Database.Interfaces;
using log4net;
using Raven.HabboHotel.Items;

namespace Raven.HabboHotel.Rooms.Music
{
    public class SongInstance
    {
        private readonly SongItem mDiskItem;
        private readonly SongData mSongData;

        public SongInstance(SongItem Item, SongData SongData)
        {
            mDiskItem = Item;
            mSongData = SongData;
        }

        public SongData SongData
        {
            get { return mSongData; }
        }

        public SongItem DiskItem
        {
            get { return mDiskItem; }
        }
    }
}