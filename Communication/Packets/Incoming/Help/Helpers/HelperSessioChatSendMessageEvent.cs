using Raven.Communication.Packets.Outgoing.Help.Helpers;
using Raven.Database.Interfaces;
using Raven.HabboHotel.GameClients;
using Raven.HabboHotel.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Communication.Packets.Incoming.Help.Helpers
{
    class HelperSessioChatSendMessageEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            var Element = HelperToolsManager.GetElement(Session);
            var message = Packet.PopString();
            if (Element.OtherElement != null)
            {
                Session.SendMessage(new HelperSessionSendChatComposer(Session.GetHabbo().Id, message));
                Element.OtherElement.Session.SendMessage(new HelperSessionSendChatComposer(Session.GetHabbo().Id, message));
                LogHelper(Session.GetHabbo().Id, Element.OtherElement.Session.GetHabbo().Id, message);
            }
            else
            {
                Session.SendMessage(new CallForHelperErrorComposer(0));
            }
        }

        public void LogHelper(int From_Id, int ToId, string Message)
        {
            DateTime Now = DateTime.Now;
            using (IQueryAdapter dbClient = RavenEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("INSERT INTO chatlogs_helper VALUES (NULL, " + From_Id + ", " + ToId + ", @message, UNIX_TIMESTAMP())");
                dbClient.AddParameter("message", Message);
                dbClient.RunQuery();
            }
        }
    }
}
