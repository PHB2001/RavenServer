#region

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Raven.Communication.Packets.Incoming;
using Raven.HabboHotel.Rooms;

#endregion

namespace Raven.HabboHotel.Items.Wired.Boxes.Triggers
{
    internal class RepeaterBox : IWiredItem, IWiredCycle
    {
        private int _delay;

        public RepeaterBox(Room Instance, Item Item)
        {
            this.Instance = Instance;
            this.Item = Item;
            SetItems = new ConcurrentDictionary<int, Item>();
        }

        public int Delay
        {
            get { return _delay; }
            set
            {
                _delay = value;
                TickCount = value;
            }
        }

        public int TickCount { get; set; }

        public bool OnCycle()
        {
            var Success = false;
            ICollection<RoomUser> Avatars = Instance.GetRoomUserManager().GetRoomUsers();
            var Effects = Instance.GetWired().GetEffects(this);
            var Conditions = Instance.GetWired().GetConditions(this);

            foreach (var Condition in Conditions)
            {
                foreach (var Avatar in Avatars)
                {
                    if (Avatar?.GetClient() == null || Avatar.GetClient().GetHabbo() == null ||
                        !Condition.Execute(Avatar.GetClient().GetHabbo()))
                        continue;

                    Success = true;
                }

                if (!Success)
                    return false;

                Success = false;
                Instance.GetWired().OnEvent(Condition.Item);
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
            else
            {
                foreach (var Effect in Effects.Where(Effect => Effect.Execute()))
                    Instance?.GetWired().OnEvent(Effect.Item);
            }

            if (Delay == 1)
                TickCount = 0;
            else
                TickCount = Delay;

            return true;
        }

        public Room Instance { get; set; }
        public Item Item { get; set; }

        public WiredBoxType Type => WiredBoxType.TriggerRepeat;

        public ConcurrentDictionary<int, Item> SetItems { get; set; }
        public string StringData { get; set; }
        public bool BoolData { get; set; }
        public string ItemsData { get; set; }

        public void HandleSave(ClientPacket Packet)
        {
            var Unknown = Packet.PopInt();
            var Delay = Packet.PopInt();

            this.Delay = Delay;
            TickCount = Delay;
        }

        public bool Execute(params object[] Params) => true;
    }
}