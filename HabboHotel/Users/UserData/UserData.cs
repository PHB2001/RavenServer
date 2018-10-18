using System.Collections;
using System.Collections.Generic;
using Raven.HabboHotel.Achievements;
using Raven.HabboHotel.Items;
using Raven.HabboHotel.Rooms.AI;
using Raven.HabboHotel.Rooms;
using Raven.HabboHotel.Users.Badges;
using Raven.HabboHotel.Users.Inventory;
using Raven.HabboHotel.Users.Messenger;
using Raven.HabboHotel.Users.Relationships;
using System.Collections.Concurrent;
using Plus.HabboHotel.Club;

namespace Raven.HabboHotel.Users.UserDataManagement
{
    public class UserData
    {
        public int userID;
        public Habbo user;

        public Dictionary<int, Relationship> Relations;
        public ConcurrentDictionary<string, UserAchievement> achievements;
        public List<Badge> badges;
        public List<int> favouritedRooms;
        public List<string> tags;
        public List<string> MysticKeys;
        public List<string> MysticBoxes;
        public Dictionary<int, MessengerRequest> requests;
        public Dictionary<int, MessengerBuddy> friends;
        public List<int> ignores;
        public Dictionary<int, int> quests;
        public Dictionary<int, UserTalent> Talents;
        public List<RoomData> rooms;
        public Dictionary<string, Subscription> subscriptions;

        public UserData(int userID, ConcurrentDictionary<string, UserAchievement> achievements, List<int> favouritedRooms, List<string> tags, List<string> MysticKeys, List<string> MysticBoxes, List<int> ignores,
            List<Badge> badges, Dictionary<int, MessengerBuddy> friends, Dictionary<int, MessengerRequest> requests, List<RoomData> rooms, Dictionary<int, int> quests, Habbo user,
            Dictionary<int, Relationship> Relations, Dictionary<int, UserTalent> talents, Dictionary<string, Subscription> subscriptions)
        {
            this.userID = userID;
            this.achievements = achievements;
            this.favouritedRooms = favouritedRooms;
            this.tags = tags;
            this.MysticBoxes = MysticBoxes;
            this.MysticKeys = MysticKeys;
            this.ignores = ignores;
            this.badges = badges;
            this.friends = friends;
            this.requests = requests;
            this.rooms = rooms;
            this.quests = quests;
            this.user = user;
            this.Talents = talents;
            this.Relations = Relations;
            this.subscriptions = subscriptions;
        }
    }
}