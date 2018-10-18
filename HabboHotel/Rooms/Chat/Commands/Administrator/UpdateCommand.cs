using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Raven.Communication.Packets.Outgoing.Catalog;
using Raven.Core;
using Raven.HabboHotel.GameClients;
using Raven.Communication.Packets.Outgoing.Notifications;
using Raven.Communication.Packets.Outgoing.Rooms.Notifications;
using Raven.Communication.Packets.Outgoing.Inventory.Achievements;

namespace Raven.HabboHotel.Rooms.Chat.Commands.Administrator
{
    class UpdateCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_update"; }
        }

        public string Parameters
        {
            get { return "%variable%"; }
        }

        public string Description
        {
            get { return "Actualiza una característica de Raven."; }
        }

        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Debe introducir algo para actualizar, e.g. :update catalog");
                return;
            }


            string UpdateVariable = Params[1];
            switch (UpdateVariable.ToLower())
            {
                case "calendar":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_rewards"))
                        {
                            Session.SendWhisper("Oops, ha surgido un error.");
                            break;
                        }

                        RavenEnvironment.GetGame().GetCalendarManager().Init();
                        Session.SendWhisper("Calendario actualizado.");
                        break;
                    }
                case "ecotron":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_rewards"))
                        {
                            Session.SendWhisper("Oops, ha surgido un error.");
                            break;
                        }

                        RavenEnvironment.GetGame().GetFurniMaticRewardsMnager().Initialize(RavenEnvironment.GetDatabaseManager().GetQueryReactor());
                        Session.SendWhisper("Premios ecotrón actualizados.");
                        break;
                    }
                case "grupos":
                case "groups":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_catalog"))
                        {
                            Session.SendWhisper("Oops, usted no tiene permiso para actualizar los grupos.");
                            break;
                        }

                        string Message = CommandManager.MergeParams(Params, 2);

                        RavenEnvironment.GetGame().GetGroupManager().Init();

                        break;
                    }

                case "cata":
                case "catalog":
                case "catalogue":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_catalog"))
                        {
                            Session.SendWhisper("Oops, usted no tiene permiso para actualizar el catálogo.");
                            break;
                        }

                        string Message = CommandManager.MergeParams(Params, 2);

                        RavenEnvironment.GetGame().GetCatalogFrontPageManager().LoadFrontPage();
                        RavenEnvironment.GetGame().GetCatalog().Init(RavenEnvironment.GetGame().GetItemManager());
                        RavenEnvironment.GetGame().GetClientManager().SendMessage(new CatalogUpdatedComposer());
                        RavenEnvironment.GetGame().GetClientManager().SendMessage(RoomNotificationComposer.SendBubble("catalogue", "¡El catálogo ha sido actualizado, échale un vistazo!", "catalog/open/" + Message + ""));

                        break;
                    }              

                case "goals":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_catalog"))
                        {
                            Session.SendWhisper("Oops, usted no tiene permiso para actualizar el LandingCommunityGoalVS.");
                            break;
                        }

                        string Message = CommandManager.MergeParams(Params, 2);

                        RavenEnvironment.GetGame().GetCommunityGoalVS().LoadCommunityGoalVS();

                        Session.SendWhisper("Has actualizado satisfactoriamente los LandingCommunityGoalVS.", 34);

                        break;
                    }
                case "pinatas":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_catalog"))
                        {
                            Session.SendWhisper("Oops, usted no tiene permiso para actualizar los premios de las piñatas.");
                            break;
                        }

                        RavenEnvironment.GetGame().GetPinataManager().Initialize(RavenEnvironment.GetDatabaseManager().GetQueryReactor());
                        RavenEnvironment.GetGame().GetClientManager().SendMessage(RoomNotificationComposer.SendBubble("catalogue", "Se han actualizado los premios de las piñatas.", ""));
                        break;
                    }

                case "polls":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_catalog"))
                        {
                            Session.SendWhisper("Oops, usted no tiene permiso para actualizar los premios de las piñatas.");
                            break;
                        }
                        RavenEnvironment.GetGame().GetPollManager().Init();
                        break;
                    }

                case "list":
                    {
                        StringBuilder List = new StringBuilder("");
                        List.AppendLine("Lista de comandos para actualizar");
                        List.AppendLine("---------------------------------");
                        List.AppendLine(":update catalog = Actualizar el cátalogo.");
                        List.AppendLine(":update items = Actualiza los ítems, si cambias algo en 'furniture'");
                        List.AppendLine(":update models = Por si añades algun modelo de sala manualmente");
                        List.AppendLine(":update promotions = Actualiza las noticias que estan en vista hotel 'Server Landinds'");
                        List.AppendLine(":update filter = Actualiza el filtro, 'siempre ejecutar si se añade una palabra'");
                        List.AppendLine(":update navigator = Actualiza el Navegador");
                        List.AppendLine(":update rights = Actualiza los Permisos");
                        List.AppendLine(":update configs = Actualiza la configuracion del hotel");
                        List.AppendLine(":update bans = Actualiza los baneados");
                        List.AppendLine(":update tickets = Actualiza los tickets de mod");
                        List.AppendLine(":update badge_definitions = Actualiza las placas agregadas");
                        List.AppendLine(":update vouchers = Actualiza los vouchers agregados");
                        List.AppendLine(":update characters = Actualiza los carácteres del filtro.");
                        List.AppendLine(":update offers = Actualiza las ofertas relámpago del hotel.");
                        List.AppendLine(":update nux = Actualiza los premios nux del hotel.");
                        List.AppendLine(":update polls = Actualiza los polls del hotel.");
                        Session.SendMessage(new MOTDNotificationComposer(List.ToString()));
                        break;
                    }

                case "nux":
                    if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_furni"))
                    {
                        Session.SendWhisper("Oops, usted no tiene permiso para actualizar esto.");
                        break;
                    }

                    RavenEnvironment.GetGame().GetNuxUserGiftsListManager().Initialize(RavenEnvironment.GetDatabaseManager().GetQueryReactor());
                    RavenEnvironment.GetGame().GetNuxUserGiftsManager().Initialize(RavenEnvironment.GetDatabaseManager().GetQueryReactor());
                    Session.SendWhisper("Has recargado los nux gifts.");
                    break;

                case "characters":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_filter"))
                        {
                            Session.SendWhisper("Oops, Usted no tiene permiso para actualizar los carácteres del filtro");
                            break;
                        }

                        RavenEnvironment.GetGame().GetChatManager().GetFilter().InitCharacters();
                        Session.SendWhisper("Carácteres del filtro actualiza2 correctamente.");
                        break;
                    }

                case "items":
                case "furni":
                case "furniture":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_furni"))
                        {
                            Session.SendWhisper("Oops, usted no tiene permiso para actualizar los furnis");
                            break;
                        }

                        RavenEnvironment.GetGame().GetItemManager().Init();
                        Session.SendWhisper("Items actualizados correctamente.");
                        break;
                    }

                case "crafting":
                    if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_furni"))
                    {
                        Session.SendWhisper("Oops, usted no tiene permiso para actualizar el crafting.");
                        break;
                    }

                    RavenEnvironment.GetGame().GetCraftingManager().Init();
                    Session.SendWhisper("Crafting actualizado correctamente.");
                    break;

                case "offers":
                    if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_furni"))
                    {
                        Session.SendWhisper("Oops, usted no tiene permiso para actualizar los furnis");
                        break;
                    }

                    RavenEnvironment.GetGame().GetTargetedOffersManager().Initialize(RavenEnvironment.GetDatabaseManager().GetQueryReactor());
                    Session.SendWhisper("Ofertas relámpago actualizadas correctamente.");
                    break;

                case "songs":
                    if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_furni"))
                    {
                        Session.SendWhisper("Oops, usted no tiene permiso para actualizar las canciones.");
                        break;
                    }

                    RavenEnvironment.GetGame().GetMusicManager().Init();
                    Session.SendWhisper("Has recargado todas las canciones.");
                    break;

                case "models":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_models"))
                        {
                            Session.SendWhisper("Oops, usted no tiene permiso para actualizar los Models");
                            break;
                        }

                        RavenEnvironment.GetGame().GetRoomManager().LoadModels();
                        Session.SendWhisper("Modelos de sala actualizados correctamente.");
                        break;
                    }

                case "promotions":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_promotions"))
                        {
                            Session.SendWhisper("Oops, usted no tiene permisos para actualizar las promociones.");
                            break;
                        }

                        RavenEnvironment.GetGame().GetLandingManager().LoadPromotions();
                        Session.SendWhisper("Noticias de vista al Hotel actualizadas correctamente.");
                        break;
                    }

                case "youtube":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_youtube"))
                        {
                            Session.SendWhisper("Oops, usted no tiene permiso para actualizar los videos de Youtube TV.");
                            break;
                        }

                        RavenEnvironment.GetGame().GetTelevisionManager().Init();
                        Session.SendWhisper("Youtube television actualizado correctamente");
                        break;
                    }

                case "filter":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_filter"))
                        {
                            Session.SendWhisper("Oops, Usted no tiene permiso para actualizar el filtro");
                            break;
                        }

                        RavenEnvironment.GetGame().GetChatManager().GetFilter().InitWords();
                        RavenEnvironment.GetGame().GetClientManager().StaffAlert(RoomNotificationComposer.SendBubble("filters", Session.GetHabbo().Username + " ha actualizado el filtro del hotel.", ""));
                        break;
                    }

                case "navigator":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_navigator"))
                        {
                            Session.SendWhisper("Oops, usted no tiene permiso para actualizar el navegador.");
                            break;
                        }

                        RavenEnvironment.GetGame().GetNavigator().Init();
                        RavenEnvironment.GetGame().GetClientManager().SendMessage(RoomNotificationComposer.SendBubble("newuser", Session.GetHabbo().Username + " ha modificado las salas públicas del hotel.", ""));
                        break;
                    }

                case "ranks":
                case "rights":
                case "permissions":
                case "commands":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_rights"))
                        {
                            Session.SendWhisper("Oops, usted no tiene derecho para actualizar los derechos y permisos.");
                            break;
                        }

                        RavenEnvironment.GetGame().GetPermissionManager().Init();

                        foreach (GameClient Client in RavenEnvironment.GetGame().GetClientManager().GetClients.ToList())
                        {
                            if (Client == null || Client.GetHabbo() == null || Client.GetHabbo().GetPermissions() == null)
                                continue;

                            Client.GetHabbo().GetPermissions().Init(Client.GetHabbo());
                        }

                        RavenEnvironment.GetGame().GetClientManager().StaffAlert(RoomNotificationComposer.SendBubble("commandsupdated", Session.GetHabbo().Username + " ha actualizado todos los permisos, comandos y rangos del hotel.", ""));
                        break;
                    }

                case "config":
                case "settings":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_configuration"))
                        {
                            Session.SendWhisper("Oops, usted no tiene permiso para actualizar la configuracion del Hotel");
                            break;
                        }

                        RavenEnvironment.ConfigData = new ConfigData();
                        RavenEnvironment.GetGame().GetClientManager().StaffAlert(RoomNotificationComposer.SendBubble("commandsupdated", Session.GetHabbo().Username + " ha recargado la configuración del hotel.", ""));
                        break;
                    }

                case "bans":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_bans"))
                        {
                            Session.SendWhisper("Oops, usted no tiene permiso para actualizar la lista de baneados");
                            break;
                        }

                        RavenEnvironment.GetGame().GetModerationManager().ReCacheBans();
                        RavenEnvironment.GetGame().GetClientManager().StaffAlert(RoomNotificationComposer.SendBubble("commandsupdated", Session.GetHabbo().Username + " ha actualizado la lista de baneos de " + RavenEnvironment.GetDBConfig().DBData["hotel.name"] + ".", ""));
                        break;
                    }

                case "quests":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_quests"))
                        {
                            Session.SendWhisper("Oops, usted no tiene permiso para actualizar las misiones.");
                            break;
                        }

                        RavenEnvironment.GetGame().GetQuestManager().Init();
                        RavenEnvironment.GetGame().GetClientManager().StaffAlert(RoomNotificationComposer.SendBubble("commandsupdated", Session.GetHabbo().Username + " ha actualizado todas las misiones y retos de " + RavenEnvironment.GetDBConfig().DBData["hotel.name"] + ".", ""));
                        break;
                    }

                case "achievements":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_achievements"))
                        {
                            Session.SendWhisper("Oops, usted no tiene permiso para actualizar los logros.");
                            break;
                        }

                        RavenEnvironment.GetGame().GetAchievementManager().LoadAchievements();
                        RavenEnvironment.GetGame().GetClientManager().SendMessage(new BadgeDefinitionsComposer(RavenEnvironment.GetGame().GetAchievementManager()._achievements));
                        RavenEnvironment.GetGame().GetClientManager().StaffAlert(RoomNotificationComposer.SendBubble("commandsupdated", Session.GetHabbo().Username + " ha recargado todos los desafíos y achievements del hotel satisfactoriamente.", ""));
                        break;
                    }



                case "moderation":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_moderation"))
                        {
                            Session.SendWhisper("Oops, you do not have the 'command_update_moderation' permission.");
                            break;
                        }

                        RavenEnvironment.GetGame().GetModerationManager().Init();
                        RavenEnvironment.GetGame().GetClientManager().ModAlert("Moderation presets have been updated. Please reload the client to view the new presets.");
                        RavenEnvironment.GetGame().GetClientManager().StaffAlert(RoomNotificationComposer.SendBubble("commandsupdated", Session.GetHabbo().Username + " ha actualizado la configuración de los permisos de moderación.", ""));
                        break;
                    }

                //case "tickets":
                //    {
                //        if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_tickets"))
                //        {
                //            Session.SendWhisper("Oops, No tiene permisos para actualizar los tickets");
                //            break;
                //        }

                //        if (RavenEnvironment.GetGame().GetModerationTool().Tickets.Count > 0)
                //            RavenEnvironment.GetGame().GetModerationTool().Tickets.Clear();

                //        RavenEnvironment.GetGame().GetClientManager().StaffAlert(RoomNotificationComposer.SendBubble("tickets", Session.GetHabbo().Username + " ha actualizado y vaciado todos los tickets de Habbi.", ""));
                //        break;
                //    }

                case "vouchers":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_vouchers"))
                        {
                            Session.SendWhisper("Oops, no tienes los permisos suficientes para actualizar los vouchers.");
                            break;
                        }

                        RavenEnvironment.GetGame().GetCatalog().GetVoucherManager().Init();
                        RavenEnvironment.GetGame().GetClientManager().StaffAlert(RoomNotificationComposer.SendBubble("commandsupdated", Session.GetHabbo().Username + " ha actualizado los códigos voucher del hotel.", ""));
                        break;
                    }

                case "gc":
                case "games":
                case "gamecenter":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_game_center"))
                        {
                            Session.SendWhisper("Oops, you do not have the 'command_update_game_center' permission.");
                            break;
                        }

                        RavenEnvironment.GetGame().GetGameDataManager().Init();
                        RavenEnvironment.GetGame().GetLeaderBoardDataManager().Init();
                        Session.SendWhisper("Game Center cache successfully updated.");
                        break;
                    }

                case "pet_locale":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_pet_locale"))
                        {
                            Session.SendWhisper("Oops, you do not have the 'command_update_pet_locale' permission.");
                            break;
                        }

                        RavenEnvironment.GetGame().GetChatManager().GetPetLocale().Init();
                        Session.SendWhisper("Pet locale cache successfully updated.");
                        break;
                    }

                case "locale":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_locale"))
                        {
                            Session.SendWhisper("Oops, you do not have the 'command_update_locale' permission.");
                            break;
                        }

                        RavenEnvironment.GetGame().GetLanguageLocale().Init();
                        Session.SendWhisper("Locale cache successfully updated.");
                        break;
                    }

                case "mutant":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_anti_mutant"))
                        {
                            Session.SendWhisper("Oops, you do not have the 'command_update_anti_mutant' permission.");
                            break;
                        }

                        RavenEnvironment.GetGame().GetAntiMutant().Init();
                        Session.SendWhisper("Anti mutant successfully reloaded.");
                        break;
                    }

                case "bots":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_bots"))
                        {
                            Session.SendWhisper("Oops, you do not have the 'command_update_bots' permission.");
                            break;
                        }

                        RavenEnvironment.GetGame().GetBotManager().Init();
                        Session.SendWhisper("Bot recargados correctamente");
                        break;
                    }

                case "rewards":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_rewards"))
                        {
                            Session.SendWhisper("Oops, you do not have the 'command_update_rewards' permission.");
                            break;
                        }

                        RavenEnvironment.GetGame().GetRewardManager().Reload();
                        Session.SendWhisper("Rewards managaer successfully reloaded.");
                        break;
                    }

                case "chat_styles":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_chat_styles"))
                        {
                            Session.SendWhisper("Oops, you do not have the 'command_update_chat_styles' permission.");
                            break;
                        }

                        RavenEnvironment.GetGame().GetChatManager().GetChatStyles().Init();
                        Session.SendWhisper("Chat Styles successfully reloaded.");
                        break;
                    }

                case "badges":
                case "badge_definitions":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_badge_definitions"))
                        {
                            Session.SendWhisper("Oops, you do not have the 'command_update_badge_definitions' permission.");
                            break;
                        }

                        RavenEnvironment.GetGame().GetBadgeManager().Init();
                        RavenEnvironment.GetGame().GetClientManager().StaffAlert(RoomNotificationComposer.SendBubble("definitions", Session.GetHabbo().Username + " ha actualizado las definiciones de placas.", ""));
                        break;
                    }
                default:
                    Session.SendWhisper("'" + UpdateVariable + "' es un comando invalido, escribelo correctamente");
                    break;
            }
        }
    }
}
