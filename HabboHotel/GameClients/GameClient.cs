﻿using System;

using Raven.Net;
using System.Linq;
using Raven.Core;
using Raven.Communication.Packets.Incoming;
using Raven.HabboHotel.Rooms;
using Raven.HabboHotel.Users;
using Raven.Communication.Interfaces;
using Raven.HabboHotel.Users.UserDataManagement;
using ConnectionManager;

using Raven.Communication.Packets.Outgoing.Sound;
using Raven.Communication.Packets.Outgoing.Rooms.Chat;
using Raven.Communication.Packets.Outgoing.Handshake;
using Raven.Communication.Packets.Outgoing.Navigator;
using Raven.Communication.Packets.Outgoing.Moderation;
using Raven.Communication.Packets.Outgoing.Inventory.AvatarEffects;
using Raven.Communication.Packets.Outgoing.Inventory.Achievements;


using Raven.Communication.Encryption.Crypto.Prng;
using Raven.HabboHotel.Users.Messenger.FriendBar;
using Raven.Communication.Packets.Outgoing.BuildersClub;
using Raven.HabboHotel.Moderation;

using Raven.Database.Interfaces;
using Raven.HabboHotel.Subscriptions;
using Raven.HabboHotel.Permissions;
using Raven.Communication.Packets.Outgoing;
using Raven.Communication.Packets.Outgoing.Nux;
using Raven.Communication.Packets.Outgoing.Rooms.Notifications;
using System.Collections.Generic;
using Raven.HabboHotel.Catalog;
using Raven.Communication.Packets.Outgoing.Talents;
using Raven.HabboHotel.Users.Messenger;
using System.Xml;
using System.Net;
using System.IO;
using Raven.Communication.Packets.Outgoing.Campaigns;
using Raven.Communication.Packets.Outgoing.Notifications;
using System.Threading;
using Raven.Communication.Packets.Outgoing.Messenger;
using Raven.Communication.Packets.Outgoing.Rooms.Furni;
using Raven.HabboHotel.Helpers;
using Raven.Communication.Packets.Outgoing.Help.Helpers;
using Raven.Utilities;

namespace Raven.HabboHotel.GameClients
{
    public class GameClient
    {
        private readonly int _id;
        private Habbo _habbo;
        public string MachineId;
        private bool _disconnected;
        public string ssoTicket;
        public ARC4 RC4Client = null;
        private GamePacketParser _packetParser;
        private ConnectionInformation _connection;
        public int PingCount { get; set; }

        public Session wsSession;

        public GameClient(int ClientId, ConnectionInformation pConnection)
        {
            this._id = ClientId;
            this._connection = pConnection;
            this._packetParser = new GamePacketParser(this);
            this.PingCount = 0;
        }

        private void SwitchParserRequest()
        {
            _packetParser.SetConnection(_connection);
            _packetParser.onNewPacket += parser_onNewPacket;
            byte[] data = (_connection.parser as InitialPacketParser).currentData;
            _connection.parser.Dispose();
            _connection.parser = _packetParser;
            _connection.parser.handlePacketData(data);
        }

        private void parser_onNewPacket(ClientPacket Message)
        {
            try
            {
                RavenEnvironment.GetGame().GetPacketManager().TryExecutePacket(this, Message);
            }
            catch (Exception e)
            {
                Logging.LogPacketException(Message.ToString(), e.ToString());
            }
        }

        internal string SendNotification(string v1, string v2)
        {
            throw new NotImplementedException();
        }

        private void PolicyRequest()
        {
            _connection.SendData(RavenEnvironment.GetDefaultEncoding().GetBytes("<?xml version=\"1.0\"?>\r\n" +
                   "<!DOCTYPE cross-domain-policy SYSTEM \"/xml/dtds/cross-domain-policy.dtd\">\r\n" +
                   "<cross-domain-policy>\r\n" +
                   "<allow-access-from domain=\"*\" to-ports=\"1-31111\" />\r\n" +
                   "</cross-domain-policy>\x0"));
        }


        public void StartConnection()
        {
            if (_connection == null)
                return;

            this.PingCount = 0;

            (_connection.parser as InitialPacketParser).PolicyRequest += PolicyRequest;
            (_connection.parser as InitialPacketParser).SwitchParserRequest += SwitchParserRequest;
            _connection.startPacketProcessing();
        }

        public bool TryAuthenticate(string AuthTicket)
        {
            try
            {
                byte errorCode = 0;
                UserData userData = UserDataFactory.GetUserData(AuthTicket, out errorCode);
                if (errorCode == 1 || errorCode == 2)
                {
                    Disconnect();
                    return false;
                }

                #region Ban Checking
                //Let's have a quick search for a ban before we successfully authenticate..
                ModerationBan BanRecord = null;
                if (!string.IsNullOrEmpty(MachineId))
                {
                    if (RavenEnvironment.GetGame().GetModerationManager().IsBanned(MachineId, out BanRecord))
                    {
                        if (RavenEnvironment.GetGame().GetModerationManager().MachineBanCheck(MachineId))
                        {
                            Disconnect();
                            return false;
                        }
                    }
                }

                if (userData.user != null)
                {
                    //Now let us check for a username ban record..
                    BanRecord = null;
                    if (RavenEnvironment.GetGame().GetModerationManager().IsBanned(userData.user.Username, out BanRecord))
                    {
                        if (RavenEnvironment.GetGame().GetModerationManager().UsernameBanCheck(userData.user.Username))
                        {
                            Disconnect();
                            return false;
                        }
                    }
                }
                #endregion

                RavenEnvironment.GetGame().GetClientManager().RegisterClient(this, userData.userID, userData.user.Username);
                _habbo = userData.user;
                

                if (_habbo != null)
                {
                    this.ssoTicket = AuthTicket;
                    userData.user.Init(this, userData);

                    
                    SendMessage(new AuthenticationOKComposer());
                    SendMessage(new AvatarEffectsComposer(_habbo.Effects().GetAllEffects));
                    SendMessage(new NavigatorSettingsComposer(_habbo.HomeRoom));
                    SendMessage(new FavouritesComposer(userData.user.FavoriteRooms));
                    SendMessage(new FigureSetIdsComposer(_habbo.GetClothing().GetClothingAllParts));
                    SendMessage(new UserRightsComposer(_habbo));
                    SendMessage(new AvailabilityStatusComposer());
                    SendMessage(new AchievementScoreComposer(_habbo.GetStats().AchievementPoints));
                    

                    //var habboClubSubscription = new ServerPacket(ServerPacketHeader.HabboClubSubscriptionComposer);
                    //habboClubSubscription.WriteString("club_habbo");
                    //habboClubSubscription.WriteInteger(0);
                    //habboClubSubscription.WriteInteger(0);
                    //habboClubSubscription.WriteInteger(0);
                    //habboClubSubscription.WriteInteger(2);
                    //habboClubSubscription.WriteBoolean(false);
                    //habboClubSubscription.WriteBoolean(false);
                    //habboClubSubscription.WriteInteger(0);
                    //habboClubSubscription.WriteInteger(0);
                    //habboClubSubscription.WriteInteger(0);
                    //SendMessage(habboClubSubscription);

                    SendMessage(new BuildersClubMembershipComposer());
                    SendMessage(new CfhTopicsInitComposer());

                    SendMessage(new BadgeDefinitionsComposer(RavenEnvironment.GetGame().GetAchievementManager()._achievements));
                    SendMessage(new SoundSettingsComposer(_habbo.ClientVolume, _habbo.ChatPreference, _habbo.AllowMessengerInvites, _habbo.FocusPreference, FriendBarStateUtility.GetInt(_habbo.FriendbarState)));
                    SendMessage(new TalentTrackLevelComposer());

                    if (GetHabbo().GetMessenger() != null)
                        GetHabbo().GetMessenger().OnStatusChanged(true);

                    if (_habbo.Rank < 2 && !RavenStaticGameSettings.HotelOpenForUsers)
                    {
                        SendMessage(new SendHotelAlertLinkEventComposer("Actualmente solo el Equipo Adminsitrativo puede entrar al hotel para comprobar que todo está bien antes de que los usuarios puedan entrar. Vuelve a intentarlo en unos minutos, podrás encontrar más información en nuestro Facebook.", RavenEnvironment.GetDBConfig().DBData["facebook_url"]));
                        Thread.Sleep(10000);
                        Disconnect();
                        return false;
                    }

                    if (!string.IsNullOrEmpty(MachineId))
                    {
                        if (this._habbo.MachineId != MachineId)
                        {
                            using (IQueryAdapter dbClient = RavenEnvironment.GetDatabaseManager().GetQueryReactor())
                            {
                                dbClient.SetQuery("UPDATE `users` SET `machine_id` = @MachineId WHERE `id` = @id LIMIT 1");
                                dbClient.AddParameter("MachineId", MachineId);
                                dbClient.AddParameter("id", _habbo.Id);
                                dbClient.RunQuery();
                            }
                        }

                        _habbo.MachineId = MachineId;
                    }
                    PermissionGroup PermissionGroup = null;
                    if (RavenEnvironment.GetGame().GetPermissionManager().TryGetGroup(_habbo.Rank, out PermissionGroup))
                    {
                        if (!String.IsNullOrEmpty(PermissionGroup.Badge))
                            if (!_habbo.GetBadgeComponent().HasBadge(PermissionGroup.Badge))
                                _habbo.GetBadgeComponent().GiveBadge(PermissionGroup.Badge, true, this);
                    }

                    SubscriptionData SubData = null;
                    if (RavenEnvironment.GetGame().GetSubscriptionManager().TryGetSubscriptionData(this._habbo.VIPRank, out SubData))
                    {
                        if (!String.IsNullOrEmpty(SubData.Badge))
                        {
                            if (!_habbo.GetBadgeComponent().HasBadge(SubData.Badge))
                                _habbo.GetBadgeComponent().GiveBadge(SubData.Badge, true, this);
                        }
                    }

                    if (!RavenEnvironment.GetGame().GetCacheManager().ContainsUser(_habbo.Id))
                        RavenEnvironment.GetGame().GetCacheManager().GenerateUser(_habbo.Id);
                   
                    _habbo.InitProcess();

                    //ICollection<MessengerBuddy> Friends = new List<MessengerBuddy>();
                    //foreach (MessengerBuddy Buddy in this.GetHabbo().GetMessenger().GetFriends().ToList())
                    //{
                    //    if (Buddy == null)
                    //        continue;

                    //    GameClient Friend = RavenEnvironment.GetGame().GetClientManager().GetClientByUserID(Buddy.Id);
                    //    if (Friend == null)
                    //        continue;
                    //    string figure = this.GetHabbo().Look;


                    //    Friend.SendMessage(RoomNotificationComposer.SendBubble("usr/look/" + this.GetHabbo().Username + "", this.GetHabbo().Username + " se ha conectado a " + RavenEnvironment.GetDBConfig().DBData["hotel.name"] + ".", ""));

                    //}

                    if (GetHabbo()._NUX) { SendMessage(new MassEventComposer("habbopages/bienvenida.txt")); }
                    else { SendMessage(new MassEventComposer("habbopages/welk.txt?249")); }
                

                    if (RavenEnvironment.GetDBConfig().DBData["pin.system.enable"] == "0")
                        GetHabbo().StaffOk = true;

                    if(GetHabbo().StaffOk)
                    {
                        if (GetHabbo().GetPermissions().HasRight("mod_tickets"))
                        {
                              SendMessage(new ModeratorInitComposer(
                              RavenEnvironment.GetGame().GetModerationManager().UserMessagePresets,
                              RavenEnvironment.GetGame().GetModerationManager().RoomMessagePresets,
                              RavenEnvironment.GetGame().GetModerationManager().GetTickets));
                        }
                    }

                    if (GetHabbo().Rank > 2 || GetHabbo().TeamRank == 1 || GetHabbo().TeamRank == 2 || GetHabbo().TeamRank == 3)
                    {
                        HelperToolsManager.AddHelper(_habbo.GetClient(), false, true, true);
                        SendMessage(new HandleHelperToolComposer(true));
                    }

                    //SendMessage(new CampaignCalendarDataComposer(_habbo.calendarGift));
                    //if (int.Parse(RavenEnvironment.GetDBConfig().DBData["advent.calendar.enable"]) == 1) // Tk Custom By Whats
                    //    SendMessage(new MassEventComposer("openView/calendar"));

                    if (RavenEnvironment.GetGame().GetTargetedOffersManager().TargetedOffer != null)
                    {
                        RavenEnvironment.GetGame().GetTargetedOffersManager().Initialize(RavenEnvironment.GetDatabaseManager().GetQueryReactor());
                        TargetedOffers TargetedOffer = RavenEnvironment.GetGame().GetTargetedOffersManager().TargetedOffer;

                        if (TargetedOffer.Expire > RavenEnvironment.GetIUnixTimestamp())
                        {

                            if (TargetedOffer.Limit != GetHabbo()._TargetedBuy)
                            {

                                SendMessage(RavenEnvironment.GetGame().GetTargetedOffersManager().TargetedOffer.Serialize());
                            }
                        }
                        else if (TargetedOffer.Expire == -1)
                        {

                            if (TargetedOffer.Limit != GetHabbo()._TargetedBuy)
                            {

                                SendMessage(RavenEnvironment.GetGame().GetTargetedOffersManager().TargetedOffer.Serialize());
                            }
                        }
                        else
                        {
                            using (var dbClient = RavenEnvironment.GetDatabaseManager().GetQueryReactor())
                                dbClient.runFastQuery("UPDATE targeted_offers SET active = 'false'");
                            using (var dbClient2 = RavenEnvironment.GetDatabaseManager().GetQueryReactor())
                                dbClient2.runFastQuery("UPDATE users SET targeted_buy = '0' WHERE targeted_buy > 0");
                        }
                    }

                    if (_habbo.MysticBoxes.Count == 0 && _habbo.MysticKeys.Count == 0)
                    {
                        var box = RandomNumber.GenerateRandom(1, 8);
                        string boxcolor = "";
                        switch (box)
                        {
                            case 1:
                                boxcolor = "purple";
                                break;
                            case 2:
                                boxcolor = "blue";
                                break;
                            case 3:
                                boxcolor = "green";
                                break;
                            case 4:
                                boxcolor = "yellow";
                                break;
                            case 5:
                                boxcolor = "lilac";
                                break;
                            case 6:
                                boxcolor = "orange";
                                break;
                            case 7:
                                boxcolor = "turquoise";
                                break;
                            case 8:
                                boxcolor = "red";
                                break;
                        }

                        var key = RandomNumber.GenerateRandom(1, 8);
                        string keycolor = "";
                        switch (key)
                        {
                            case 1:
                                keycolor = "purple";
                                break;
                            case 2:
                                keycolor = "blue";
                                break;
                            case 3:
                                keycolor = "green";
                                break;
                            case 4:
                                keycolor = "yellow";
                                break;
                            case 5:
                                keycolor = "lilac";
                                break;
                            case 6:
                                keycolor = "orange";
                                break;
                            case 7:
                                keycolor = "turquoise";
                                break;
                            case 8:
                                keycolor = "red";
                                break;
                        }

                        _habbo.MysticKeys.Add(keycolor);
                        _habbo.MysticBoxes.Add(boxcolor);

                        using (var dbClient = RavenEnvironment.GetDatabaseManager().GetQueryReactor())
                            dbClient.runFastQuery("INSERT INTO user_mystic_data(user_id, mystic_keys, mystic_boxes) VALUES(" + GetHabbo().Id + ", '" + keycolor + "', '" + boxcolor +"');");
                    }

                    SendMessage(new MysteryBoxDataComposer(_habbo.GetClient()));

                    SendMessage(new HCGiftsAlertComposer());

                    //if(!GetHabbo()._NUX)
                    //{
                    //    DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                    //    dtDateTime = dtDateTime.AddSeconds(GetHabbo().LastOnline);

                    //    if ((DateTime.Now - dtDateTime).TotalDays > 2)
                    //    {
                    //        //RavenEnvironment.GetGame().GetAchievementManager().ProgressAchievement(_habbo.GetClient(), "ACH_Login", 1, true);
                    //        RavenEnvironment.GetGame().GetAchievementManager().ProgressAchievement(_habbo.GetClient(), "ACH_RegistrationDuration", 1);
                    //    }

                    //    else if ((DateTime.Now - dtDateTime).TotalDays > 1 && (DateTime.Now - dtDateTime).TotalDays < 2)
                    //    {
                    //        RavenEnvironment.GetGame().GetAchievementManager().ProgressAchievement(_habbo.GetClient(), "ACH_Login", 1);
                    //        RavenEnvironment.GetGame().GetAchievementManager().ProgressAchievement(_habbo.GetClient(), "ACH_RegistrationDuration", 1);

                    //        if(GetHabbo().Rank > 2 || GetHabbo()._guidelevel > 0)
                    //        {
                    //            RavenEnvironment.GetGame().GetAchievementManager().ProgressAchievement(_habbo.GetClient(), "ACH_GuideEnrollmentLifetime", 1);
                    //        }
                    //    }
                    //}
                    

                    RavenEnvironment.GetGame().GetRewardManager().CheckRewards(this);

                    if (GetHabbo()._NUX)
                    {
                        RavenEnvironment.GetGame().GetClientManager().StaffAlert(new RoomInviteComposer(int.MinValue, GetHabbo().Username + " acaba de registrarse en Mabbi."));

                        GetHabbo()._NUX = false;
                        using (var dbClient = RavenEnvironment.GetDatabaseManager().GetQueryReactor())
                            dbClient.runFastQuery("UPDATE users SET nux_user = 'false' WHERE id = " + GetHabbo().Id + ";");
                    }

                    return true;
                }
            }
            catch (Exception e)
            {
                Logging.LogCriticalException("Bug during user login: " + e);
            }
            return false;
        }

        public void SendWhisper(string Message, int Colour = 0)
        {
            if (this == null || GetHabbo() == null || GetHabbo().CurrentRoom == null)
                return;

            RoomUser User = GetHabbo().CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(GetHabbo().Username);
            if (User == null)
                return;

            SendMessage(new WhisperComposer(User.VirtualId, Message, 0, (Colour == 0 ? User.LastBubble : Colour)));
        }

        public void SendChat(string Message, int Colour = 0)
        {
            if (this == null || GetHabbo() == null || GetHabbo().CurrentRoom == null)
                return;

            RoomUser User = GetHabbo().CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(GetHabbo().Username);
            if (User == null)
                return;

            SendMessage(new ChatComposer(User.VirtualId, Message, 0, (Colour == 0 ? User.LastBubble : Colour)));
        }

        public void SendNotification(string Message)
        {
            SendMessage(new BroadcastMessageAlertComposer(Message));
        }

        public void SendMessage(IServerPacket Message)
        {
            byte[] bytes = Message.GetBytes();

            if (Message == null)
                return;

            if (GetConnection() == null)
                return;

            GetConnection().SendData(bytes);
        }

        public void SendShout(string Message, int Colour = 0)
        {
            if (this == null || GetHabbo() == null || GetHabbo().CurrentRoom == null)
                return;

            RoomUser User = GetHabbo().CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(GetHabbo().Username);
            if (User == null)
                return;

            SendMessage(new ShoutComposer(User.VirtualId, Message, 0, (Colour == 0 ? User.LastBubble : Colour)));
        }

        public int ConnectionID
        {
            get { return _id; }
        }

        public ConnectionInformation GetConnection()
        {
            return _connection;
        }

        public Habbo GetHabbo()
        {
            return _habbo;
        }

        public void Disconnect()
        {
            try
            {
                if (GetHabbo() != null)
                {
                    using (IQueryAdapter dbClient = RavenEnvironment.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.RunQuery(GetHabbo().GetQueryString);
                    }

                    GetHabbo().OnDisconnect();
                   

                }
            }
            catch (Exception e)
            {
                Logging.LogException(e.ToString());
            }


            if (!_disconnected)
            {
                if (_connection != null)
                _connection.Dispose();
                _disconnected = true;
            }
        }

        public void Dispose()
        {
            if (GetHabbo() != null)
                GetHabbo().OnDisconnect();

            this.MachineId = string.Empty;
            this._disconnected = true;
            this._habbo = null;
            this._connection = null;
            this.RC4Client = null;
            this._packetParser = null;
        }

        public class Meteorologia
        {
            public string ApiVersion { get; set; }
            public Data Data { get; set; }
        }

        public class Data
        {
            public string Location { get; set; }
            public string Temperature { get; set; }
            public string Skytext { get; set; }
            public string Humidity { get; set; }
            public string Wind { get; set; }
            public string Date { get; set; }
            public string Day { get; set; }
        }
    }
}