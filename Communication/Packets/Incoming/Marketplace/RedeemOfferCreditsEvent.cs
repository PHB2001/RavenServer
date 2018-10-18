﻿using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;

using Raven.Communication.Packets.Outgoing.Inventory.Purse;
using Raven.Database.Interfaces;


namespace Raven.Communication.Packets.Incoming.Marketplace
{
    class RedeemOfferCreditsEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int CreditsOwed = 0;

            DataTable Table = null;
            using (IQueryAdapter dbClient = RavenEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT `asking_price` FROM `catalog_marketplace_offers` WHERE `user_id` = '" + Session.GetHabbo().Id + "' AND state = '2'");
               Table = dbClient.getTable();
            }

            if (Table != null)
            {
                foreach (DataRow row in Table.Rows)
                {
                    CreditsOwed += Convert.ToInt32(row["asking_price"]);
                }

                if (CreditsOwed >= 1)
                {
                    Session.GetHabbo().Duckets += CreditsOwed;
                    Session.SendMessage(new HabboActivityPointNotificationComposer(Session.GetHabbo().Duckets, CreditsOwed));
                }

                using (IQueryAdapter dbClient = RavenEnvironment.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.RunQuery("DELETE FROM `catalog_marketplace_offers` WHERE `user_id` = '" + Session.GetHabbo().Id + "' AND `state` = '2'");
                }
            }
        }
    }
}