﻿using Raven.Communication.Packets.Outgoing.Campaigns;
using Raven.Communication.Packets.Outgoing.Inventory.Furni;
using Raven.Communication.Packets.Outgoing.Inventory.Purse;
using Raven.Communication.Packets.Outgoing.Rooms.Notifications;
using Raven.Communication.Packets.Outgoing.Users;
using Raven.Database.Interfaces;
using Raven.HabboHotel.GameClients;
using Raven.HabboHotel.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Communication.Packets.Incoming.Calendar
{
    class OpenCalendarBoxEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            string CampaignName = Packet.PopString();
            int CampaignDay = Packet.PopInt(); // INDEX VALUE.

            // Si no es el nombre de campaña actual.
            if (CampaignName != RavenEnvironment.GetGame().GetCalendarManager().GetCampaignName())
                return;

            // Si es un día inválido.
            if (CampaignDay < 0 || CampaignDay > RavenEnvironment.GetGame().GetCalendarManager().GetTotalDays() - 1 || CampaignDay < RavenEnvironment.GetGame().GetCalendarManager().GetUnlockDays())
                // Mini fix
                return;



            // Días próximos
            if (CampaignDay > RavenEnvironment.GetGame().GetCalendarManager().GetUnlockDays())

                return;


            // Esta recompensa ya ha sido recogida.
            if (Session.GetHabbo().calendarGift[CampaignDay])

                return;


            Session.GetHabbo().calendarGift[CampaignDay] = true;

            // PACKET PARA ACTUALIZAR?
            Session.SendMessage(new CalendarPrizesComposer(RavenEnvironment.GetGame().GetCalendarManager().GetCampaignDay(CampaignDay + 1)));
            Session.SendMessage(new CampaignCalendarDataComposer(Session.GetHabbo().calendarGift));


            string Gift = RavenEnvironment.GetGame().GetCalendarManager().GetGiftByDay(CampaignDay + 1);
            string GiftType = Gift.Split(':')[0];
            string GiftValue = Gift.Split(':')[1];

            switch (GiftType.ToLower())
            {
                case "itemid":
                    {
                        ItemData Item = null;
                        if (!RavenEnvironment.GetGame().GetItemManager().GetItem(int.Parse(GiftValue), out Item))
                        {
                            // No existe este ItemId.
                            return;
                        }

                        Item GiveItem = ItemFactory.CreateSingleItemNullable(Item, Session.GetHabbo(), "", "");
                        if (GiveItem != null)
                        {
                            Session.GetHabbo().GetInventoryComponent().TryAddItem(GiveItem);

                            Session.SendMessage(new FurniListNotificationComposer(GiveItem.Id, 1));
                            Session.SendMessage(new FurniListUpdateComposer());
                        }

                        Session.GetHabbo().GetInventoryComponent().UpdateItems(false);
                    }
                    break;

                case "badge":
                    {
                        Session.GetHabbo().GetBadgeComponent().GiveBadge(GiftValue, true, Session);
                    }
                    break;

                case "diamonds":
                    {
                        Session.GetHabbo().Diamonds += int.Parse(GiftValue);
                        Session.SendMessage(new HabboActivityPointNotificationComposer(Session.GetHabbo().Diamonds, 0, 5));
                    }
                    break;

                case "gotwpoints":
                    {
                        Session.GetHabbo().GOTWPoints += int.Parse(GiftValue);
                        Session.SendMessage(new HabboActivityPointNotificationComposer(Session.GetHabbo().GOTWPoints, 0, 103));
                    }
                    break;

                case "vip":
                    {
                        var IsVIP = Session.GetHabbo().GetClubManager().HasSubscription("club_vip");
                        if (IsVIP)
                        {
                            Session.SendMessage(new AlertNotificationHCMessageComposer(4));
                        }
                        else
                        {
                            Session.SendMessage(new AlertNotificationHCMessageComposer(5));
                        }
                        if (Session.GetHabbo().Rank > 2)
                        {
                            using (IQueryAdapter dbClient = RavenEnvironment.GetDatabaseManager().GetQueryReactor())
                            {
                                dbClient.RunQuery("UPDATE `users` SET `rank_vip` = '1' WHERE `user_id` = '" + Session.GetHabbo().Id + "' LIMIT 1");
                            }
                        }
                        else
                        {
                            using (IQueryAdapter dbClient = RavenEnvironment.GetDatabaseManager().GetQueryReactor())
                            {
                                dbClient.RunQuery("UPDATE `users` SET `rank` = '2' WHERE `id` = '" + Session.GetHabbo().Id + "' LIMIT 1");
                                dbClient.RunQuery("UPDATE `users` SET `rank_vip` = '1' WHERE `id` = '" + Session.GetHabbo().Id + "' LIMIT 1");
                            }
                        }

                        Session.GetHabbo().GetClubManager().AddOrExtendSubscription("club_vip", int.Parse(GiftValue) * 24 * 3600, Session);
                        Session.GetHabbo().GetBadgeComponent().GiveBadge("VIP", true, Session);

                        RavenEnvironment.GetGame().GetAchievementManager().ProgressAchievement(Session, "ACH_VipClub", 1);
                        Session.SendMessage(new ScrSendUserInfoComposer(Session.GetHabbo()));
                    }
                    break;
            }

            using (IQueryAdapter dbClient = RavenEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.runFastQuery("INSERT INTO user_campaign_gifts VALUES (NULL, '" + Session.GetHabbo().Id + "','" + CampaignName + "','" + (CampaignDay + 1) + "')");
            }
        }
    }
}
