using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Raven.Database.Interfaces;
using Raven.HabboHotel.Users;


namespace Raven.Communication.Packets.Incoming.Moderation
{
    class ModerationTradeLockEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null || !Session.GetHabbo().GetPermissions().HasRight("mod_trade_lock"))
                return;

            int UserId = Packet.PopInt();
            string Message = Packet.PopString();
            double Days = (Packet.PopInt() / 1440);
            string Unknown1 = Packet.PopString();
            string Unknown2 = Packet.PopString();

            double Length = (RavenEnvironment.GetUnixTimestamp() + (Days * 86400));

            Habbo Habbo = RavenEnvironment.GetHabboById(UserId);
            if (Habbo == null)
            {
                Session.SendWhisper("Ocurrio un error mientras se realizaba la busqueda de este usuario enla DB");
                return;
            }

            if (Habbo.GetPermissions().HasRight("mod_trade_lock") && !Session.GetHabbo().GetPermissions().HasRight("mod_trade_lock_any"))
            {
                Session.SendWhisper("Oops,No puede bloquear el trade a este usuario");
                return;
            }

            if (Days < 1)
                Days = 1;

            if (Days > 365)
                Days = 365;

            using (IQueryAdapter dbClient = RavenEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunQuery("UPDATE `user_info` SET `trading_locked` = '" + Length + "', `trading_locks_count` = `trading_locks_count` + '1' WHERE `user_id` = '" + Habbo.Id + "' LIMIT 1");
            }

            if (Habbo.GetClient() != null)
            {
                Habbo.TradingLockExpiry = Length;
                Habbo.GetClient().SendNotification("Usted no puede tradear por  " + Days + " dia(s)!\r\rRazon:\r\r" + Message);
            }
        }
    }
}
