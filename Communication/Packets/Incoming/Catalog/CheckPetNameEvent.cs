﻿using Raven.Communication.Packets.Outgoing.Catalog;
using Raven.HabboHotel.GameClients;
using Raven.Communication.Packets.Incoming;

namespace Raven.Communication.Packets.Incoming.Catalog
{
    public class CheckPetNameEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            string PetName = Packet.PopString();
            string word;
            if (PetName.Length < 2)
            {
                Session.SendMessage(new CheckPetNameComposer(2, "2"));
                return;
            }
            else if (PetName.Length > 15)
            {
                Session.SendMessage(new CheckPetNameComposer(1, "15"));
                return;
            }
            else if (!RavenEnvironment.IsValidAlphaNumeric(PetName))
            {
                Session.SendMessage(new CheckPetNameComposer(3, ""));
                return;
            }
            else if (RavenEnvironment.GetGame().GetChatManager().GetFilter().IsUnnaceptableWord(PetName, out word))
            {
                Session.SendMessage(new CheckPetNameComposer(4, "" + word));
                return;
            }

            Session.SendMessage(new CheckPetNameComposer(0, ""));
        }
    }
}