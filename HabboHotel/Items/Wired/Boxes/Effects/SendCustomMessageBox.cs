using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Collections.Concurrent;

using Raven.Communication.Packets.Incoming;
using Raven.HabboHotel.Rooms;
using Raven.HabboHotel.Users;
using Raven.Communication.Packets.Outgoing.Rooms.Chat;
using Raven.Communication.Packets.Outgoing;
using Raven.Communication.Packets.Outgoing.Rooms.Notifications;

namespace Raven.HabboHotel.Items.Wired.Boxes.Effects
{
    class SendCustomMessageBox : IWiredItem
    {
        public Room Instance { get; set; }

        public Item Item { get; set; }

        public WiredBoxType Type { get { return WiredBoxType.SendCustomMessageBox; } }

        public ConcurrentDictionary<int, Item> SetItems { get; set; }

        public string StringData { get; set; }

        public bool BoolData { get; set; }

        public string ItemsData { get; set; }

        public SendCustomMessageBox(Room Instance, Item Item)
        {
            this.Instance = Instance;
            this.Item = Item;
            this.SetItems = new ConcurrentDictionary<int, Item>();
        }

        public void HandleSave(ClientPacket Packet)
        {
            int Unknown = Packet.PopInt();
            string Message = Packet.PopString();

            this.StringData = Message;
        }

        public bool Execute(params object[] Params)
        {
            if (Params == null || Params.Length == 0)
                return false;

            Habbo Player = (Habbo)Params[0];
            if (Player == null || Player.GetClient() == null || string.IsNullOrWhiteSpace(StringData))
                return false;

            RoomUser User = Player.CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(Player.Username);
            if (User == null)
                return false;

            string Message = StringData;

            Player.GetClient().SendMessage(new RoomCustomizedAlertComposer(Message));
            return true;
        }
    }
}