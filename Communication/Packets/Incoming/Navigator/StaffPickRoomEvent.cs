using Raven.HabboHotel.Navigator;
using Raven.HabboHotel.GameClients;
using Raven.Database.Interfaces;
using Raven.HabboHotel.Rooms;
using Raven.Communication.Packets.Outgoing.Navigator;
using Raven.Communication.Packets.Outgoing.Rooms.Settings;

namespace Raven.Communication.Packets.Incoming.Navigator
{
    class StaffPickRoomEvent : IPacketEvent
    {
        public void Parse(GameClient session, ClientPacket packet)
        {
            GameClient TargetClient = RavenEnvironment.GetGame().GetClientManager().GetClientByUsername(session.GetHabbo().CurrentRoom.OwnerName);
        
            if (!session.GetHabbo().GetPermissions().HasRight("room.staff_picks.management"))
                return;

            Room room = null;
            if (!RavenEnvironment.GetGame().GetRoomManager().TryGetRoom(packet.PopInt(), out room))
                return;

            StaffPick staffPick = null;
            if (!RavenEnvironment.GetGame().GetNavigator().TryGetStaffPickedRoom(room.Id, out staffPick))
            {
                if (RavenEnvironment.GetGame().GetNavigator().TryAddStaffPickedRoom(room.Id))
                {
                    using (IQueryAdapter dbClient = RavenEnvironment.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.SetQuery("INSERT INTO `navigator_staff_picks` (`room_id`,`image`) VALUES (@roomId, null)");
                        dbClient.AddParameter("roomId", room.Id);
                        dbClient.RunQuery();
                    }
                    if (TargetClient != null)
                    {
                        RavenEnvironment.GetGame().GetAchievementManager().ProgressAchievement(TargetClient, "ACH_Spr", 1, false);
                    }
                }
            }

            else
            {
                if (RavenEnvironment.GetGame().GetNavigator().TryRemoveStaffPickedRoom(room.Id))
                {
                    using (IQueryAdapter dbClient = RavenEnvironment.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.SetQuery("DELETE FROM `navigator_staff_picks` WHERE `room_id` = @roomId LIMIT 1");
                        dbClient.AddParameter("roomId", room.Id);
                        dbClient.RunQuery();
                    }
                }
            }

            room.SendMessage(new RoomSettingsSavedComposer(room.RoomId));
            room.SendMessage(new RoomInfoUpdatedComposer(room.RoomId));
        }
    }
}