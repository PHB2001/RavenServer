using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections.Generic;

using log4net;
using Raven.Database.Interfaces;


namespace Raven.HabboHotel.Global
{
    public class ServerStatusUpdater : IDisposable
    {
        private static ILog log = LogManager.GetLogger("Mango.Global.ServerUpdater");

        private const int UPDATE_IN_SECS = 30;
        string HotelName = RavenEnvironment.GetConfig().data["hotel.name"];

        private Timer _timer;
        
        public ServerStatusUpdater()
        {
        }

        public void Init()
        {
            this._timer = new Timer(new TimerCallback(this.OnTick), null, TimeSpan.FromSeconds(UPDATE_IN_SECS), TimeSpan.FromSeconds(UPDATE_IN_SECS));

            Console.Title = "Raven - [0] ON - [0] ROOMS - [0] UPTIME";

            log.Info(">> Server Status -> READY!");
        }

        public void OnTick(object Obj)
        {
            this.UpdateOnlineUsers();
        }

        private void UpdateOnlineUsers()
        {
            TimeSpan Uptime = DateTime.Now - RavenEnvironment.ServerStarted;

            int UsersOnline = Convert.ToInt32(RavenEnvironment.GetGame().GetClientManager().Count);
            int RoomCount = RavenEnvironment.GetGame().GetRoomManager().Count;

            Console.Title = "Raven - [" + UsersOnline + "] ON - [" + RoomCount + "] ROOMS - [" + Uptime.Days + "] DAYS " + Uptime.Hours + "] HOURS";

            using (IQueryAdapter dbClient = RavenEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("UPDATE `server_status` SET `users_online` = @users, `loaded_rooms` = @loadedRooms LIMIT 1;");
                dbClient.AddParameter("users", UsersOnline);
                dbClient.AddParameter("loadedRooms", RoomCount);
                dbClient.RunQuery();
            }
        }


        public void Dispose()
        {
            using (IQueryAdapter dbClient = RavenEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunQuery("UPDATE `server_status` SET `users_online` = '0', `loaded_rooms` = '0'");
            }

            this._timer.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
