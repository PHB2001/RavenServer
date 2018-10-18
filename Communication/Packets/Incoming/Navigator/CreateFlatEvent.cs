using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Raven.HabboHotel.Rooms;
using Raven.Communication.Packets.Outgoing.Navigator;
using Raven.Communication.Packets.Outgoing.Moderation;
using Raven.HabboHotel.Navigator;

namespace Raven.Communication.Packets.Incoming.Navigator
{
    class CreateFlatEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null)
                return;
            
            if (Session.GetHabbo().UsersRooms.Count >= 500)
            {
                Session.SendMessage(new CanCreateRoomComposer(true, 500));
                return;
            }

            string word;
            string Name = Packet.PopString();
            Name = RavenEnvironment.GetGame().GetChatManager().GetFilter().IsUnnaceptableWord(Name, out word) ? "Spam" : Name;
            string Description = Packet.PopString();
            Description = RavenEnvironment.GetGame().GetChatManager().GetFilter().IsUnnaceptableWord(Description, out word) ? "Spam" : Description;
            string ModelName = Packet.PopString();

            int Category = Packet.PopInt();
            int MaxVisitors = Packet.PopInt();//10 = min, 25 = max.
            int TradeSettings = Packet.PopInt();//2 = All can trade, 1 = owner only, 0 = no trading.

            if (Name.Length < 3)
                return;

            if (Name.Length > 25)
                return;

            RoomModel RoomModel = null;
            if (!RavenEnvironment.GetGame().GetRoomManager().TryGetModel(ModelName, out RoomModel))
                return;

            SearchResultList SearchResultList = null;
            if (!RavenEnvironment.GetGame().GetNavigator().TryGetSearchResultList(Category, out SearchResultList))
                Category = 36;
            
            if (SearchResultList.CategoryType != NavigatorCategoryType.CATEGORY || SearchResultList.RequiredRank > Session.GetHabbo().Rank)
                Category = 36;

            if (MaxVisitors < 10 || MaxVisitors > 25)
                MaxVisitors = 10;

            if (TradeSettings < 0 || TradeSettings > 2)
                TradeSettings = 0;

            RoomData NewRoom = RavenEnvironment.GetGame().GetRoomManager().CreateRoom(Session, Name, Description, ModelName, Category, MaxVisitors, TradeSettings);
            if (NewRoom != null)
            {
                Session.SendMessage(new FlatCreatedComposer(NewRoom.Id, Name));
            }
        }
    }
}
