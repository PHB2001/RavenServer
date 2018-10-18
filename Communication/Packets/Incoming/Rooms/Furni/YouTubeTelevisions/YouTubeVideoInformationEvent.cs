using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Raven.HabboHotel.Items.Televisions;
using Raven.Communication.Packets.Outgoing.Rooms.Furni.YouTubeTelevisions;

namespace Raven.Communication.Packets.Incoming.Rooms.Furni.YouTubeTelevisions
{
    class YouTubeVideoInformationEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int ItemId = Packet.PopInt();
            string VideoId = Packet.PopString();

            foreach (TelevisionItem Tele in RavenEnvironment.GetGame().GetTelevisionManager().TelevisionList.ToList())
            {
                if (Tele.YouTubeId != VideoId)
                    continue;

                Session.SendMessage(new GetYouTubeVideoComposer(ItemId, Tele.YouTubeId));
            }
        }
    }
}