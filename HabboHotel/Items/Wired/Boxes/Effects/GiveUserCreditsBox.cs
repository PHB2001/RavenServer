﻿using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Collections.Concurrent;

using Raven.Communication.Packets.Incoming;
using Raven.HabboHotel.Rooms;
using Raven.HabboHotel.Users;
using Raven.Communication.Packets.Outgoing.Rooms.Chat;
using Raven.Communication.Packets.Outgoing.Rooms.Notifications;
using Raven.Communication.Packets.Outgoing.Inventory.Purse;

namespace Raven.HabboHotel.Items.Wired.Boxes.Effects
{
    class GiveUserCreditsBox : IWiredItem
    {
        public Room Instance { get; set; }

        public Item Item { get; set; }

        public WiredBoxType Type { get { return WiredBoxType.EffectGiveUserCredits; } }

        public ConcurrentDictionary<int, Item> SetItems { get; set; }

        public string StringData { get; set; }

        public bool BoolData { get; set; }

        public string ItemsData { get; set; }

        public GiveUserCreditsBox(Room Instance, Item Item)
        {
            this.Instance = Instance;
            this.Item = Item;
            this.SetItems = new ConcurrentDictionary<int, Item>();
        }

        public void HandleSave(ClientPacket Packet)
        {
            int Unknown = Packet.PopInt();
            string Credits = Packet.PopString();

            this.StringData = Credits;
        }

        public bool Execute(params object[] Params)
        {
            if (Params == null || Params.Length == 0)
                return false;

            Habbo Owner = RavenEnvironment.GetHabboById(Item.UserID);
            if (Owner == null || !Owner.GetPermissions().HasRight("room_item_wired_rewards"))
                return false;

            Habbo Player = (Habbo)Params[0];
            if (Player == null || Player.GetClient() == null)
                return false;

            RoomUser User = Player.CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(Player.Username);
            if (User == null)
                return false;

            if (String.IsNullOrEmpty(StringData))
                return false;

            Player.Credits += int.Parse(StringData);
            Player.GetClient().SendMessage(RoomNotificationComposer.SendBubble("eventoxx", "Felicidades, " + Player.Username + ", acabas de recibir " + StringData + " créditos por un Wired de recompensa.", "catalog/open"));
            Player.GetClient().SendMessage(new CreditBalanceComposer(Player.Credits));
            return true;
        }
    }
}