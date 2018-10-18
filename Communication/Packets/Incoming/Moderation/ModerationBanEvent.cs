using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Raven.Database.Interfaces;
using Raven.HabboHotel.Users;
using Raven.HabboHotel.Support;


using Raven.HabboHotel.GameClients;
using Raven.HabboHotel.Moderation;

namespace Raven.Communication.Packets.Incoming.Moderation
{
    class ModerationBanEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null || !Session.GetHabbo().GetPermissions().HasRight("mod_soft_ban"))
                return;

            int UserId = Packet.PopInt();
            string Message = Packet.PopString();
            double Length = (Packet.PopInt() * 3600) + RavenEnvironment.GetUnixTimestamp();
            string Unknown1 = Packet.PopString();
            string Unknown2 = Packet.PopString();
            bool IPBan = Packet.PopBoolean();
            bool MachineBan = Packet.PopBoolean();

            if (MachineBan)
                IPBan = false;

            Habbo Habbo = RavenEnvironment.GetHabboById(UserId);

            if (Habbo == null)
            {
                Session.SendWhisper("Ocurrio un error mientras se buscaba este usuario en la base de datos");
                return;
            }

            if (Habbo.GetPermissions().HasRight("mod_tool") && !Session.GetHabbo().GetPermissions().HasRight("mod_ban_any"))
            {
                Session.SendWhisper("Oops, no se puede banear a este usuario");
                return;
            }

            Message = (Message != null ? Message : "sin razon.");

            string Username = Habbo.Username;
            using (IQueryAdapter dbClient = RavenEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunQuery("UPDATE `user_info` SET `bans` = `bans` + '1' WHERE `user_id` = '" + Habbo.Id + "' LIMIT 1");
            }

            if (IPBan == false && MachineBan == false)
                RavenEnvironment.GetGame().GetModerationManager().BanUser(Session.GetHabbo().Username, ModerationBanType.USERNAME, Habbo.Username, Message, Length);
            else if (IPBan == true)
                RavenEnvironment.GetGame().GetModerationManager().BanUser(Session.GetHabbo().Username, ModerationBanType.IP, Habbo.Username, Message, Length);
            else if (MachineBan == true)
            {
                RavenEnvironment.GetGame().GetModerationManager().BanUser(Session.GetHabbo().Username, ModerationBanType.IP, Habbo.Username, Message, Length);
                RavenEnvironment.GetGame().GetModerationManager().BanUser(Session.GetHabbo().Username, ModerationBanType.USERNAME, Habbo.Username, Message, Length);
                RavenEnvironment.GetGame().GetModerationManager().BanUser(Session.GetHabbo().Username, ModerationBanType.MACHINE, Habbo.Username, Message, Length);
            }

            GameClient TargetClient = RavenEnvironment.GetGame().GetClientManager().GetClientByUsername(Habbo.Username);
            if (TargetClient != null)
                TargetClient.Disconnect();
        }
    }
}