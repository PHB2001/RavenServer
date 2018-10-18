using Raven.Communication.Packets.Incoming;
using Raven.HabboHotel.Rooms;
using Raven.HabboHotel.Users;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Raven.HabboHotel.Items.Wired.Boxes.Triggers
{
    class SpaceUserIdle : IWiredItem
    {
        public Room Instance { get; set; }
        public Item Item { get; set; }
        public WiredBoxType Type { get { return WiredBoxType.TriggerSpaceUserIdle; } }
        public ConcurrentDictionary<int, Item> SetItems { get; set; }
        public string StringData { get; set; }
        public bool BoolData { get; set; }
        public string ItemsData { get; set; }

        public SpaceUserIdle(Room Instance, Item Item)
        {
            this.Item = Item;
            this.Instance = Instance;
            this.SetItems = new ConcurrentDictionary<int, Item>();
        }

        public void HandleSave(ClientPacket Packet)
        {
            int Unknown = Packet.PopInt();
            string Unknown2 = Packet.PopString();

            this.StringData = Unknown2;
        }

        public bool Execute(params object[] Params)
        {
            var Player = (Habbo)Params[0];

            //bool Success = false;
            ICollection<RoomUser> Avatars = Instance.GetRoomUserManager().GetRoomUsers().ToList();
            ICollection<IWiredItem> Effects = Instance.GetWired().GetEffects(this);
            ICollection<IWiredItem> Conditions = Instance.GetWired().GetConditions(this);

            if (Conditions.Count > 0)
            {
                foreach (IWiredItem Condition in Conditions.ToList())
                {
                    foreach (var Effect in Effects)
                    {
                        if (!Effect.Execute(Player))
                            return false;

                        Instance.GetWired().OnEvent(Effect.Item);
                    }

                    if (!Condition.Execute(Player.GetClient().GetHabbo()))
                        continue;

                    Instance.GetWired().OnEvent(Condition.Item);
                }
            }
            else
            {
                foreach (var Effect in Effects)
                {
                    if (!Effect.Execute(Player))
                        return false;

                    Instance.GetWired().OnEvent(Effect.Item);
                }
            }

            //Check the ICollection to find the random addon effect.
            var HasRandomEffectAddon = Effects.Any(x => x.Type == WiredBoxType.AddonRandomEffect);
            if (HasRandomEffectAddon)
            {
                //Okay, so we have a random addon effect, now lets get the IWiredItem and attempt to execute it.
                var RandomBox = Effects.FirstOrDefault(x => x.Type == WiredBoxType.AddonRandomEffect);
                if (!RandomBox.Execute())
                    return false;

                //Success! Let's get our selected box and continue.
                var SelectedBox = Instance.GetWired().GetRandomEffect(Effects);
                if (!SelectedBox.Execute())
                    return false;

                //Woo! Almost there captain, now lets broadcast the update to the room instance.
                Instance?.GetWired().OnEvent(RandomBox.Item);
                Instance?.GetWired().OnEvent(SelectedBox.Item);
            }
            return true;
        }
    }
}