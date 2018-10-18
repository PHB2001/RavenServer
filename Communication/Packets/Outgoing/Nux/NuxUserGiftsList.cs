using System;
using System.Collections.Generic;
using System.Data;
using Raven.Database.Interfaces;
using Raven;
using Raven.HabboHotel.Items;
using Raven.Communication.Packets.Outgoing;

namespace Raven.Communication.Packets.Outgoing.Nux
{
    internal class NuxUserGiftsListManager
    {
        internal NuxUserGiftsList NuxUserGiftsList;

        internal void Initialize(IQueryAdapter dbClient)
        {
            NuxUserGiftsList = null;

            dbClient.SetQuery("SELECT * FROM nuxgifts_presents");
            var row = dbClient.getRow();

            if (row == null)
                return;

            NuxUserGiftsList = new NuxUserGiftsList((string)row["types"], (string)row["rewards"]);
        }
    }


    internal class NuxUserGiftsList
    {
        internal string[] Type, Reward;
        internal NuxUserGiftsList(string type, string reward)
        {
            Type = type.Split(';');
            Reward = reward.Split(';');
        }
    }
}
