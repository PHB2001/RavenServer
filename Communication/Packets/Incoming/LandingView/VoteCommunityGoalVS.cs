﻿using Raven.Database.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Communication.Packets.Incoming.LandingView
{
    class VoteCommunityGoalVS : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int VoteType = Packet.PopInt(); // 1 izq, 2 der

            if (VoteType == 1)
            {
                using (IQueryAdapter dbClient = RavenEnvironment.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.runFastQuery("UPDATE landing_communitygoalvs SET left_votes = left_votes + 1 WHERE id = " + RavenEnvironment.GetGame().GetCommunityGoalVS().GetId());
                }

                RavenEnvironment.GetGame().GetCommunityGoalVS().IncreaseLeftVotes();
            }
            else if (VoteType == 2)
            {
                using (IQueryAdapter dbClient = RavenEnvironment.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.runFastQuery("UPDATE landing_communitygoalvs SET right_votes = right_votes + 1 WHERE id = " + RavenEnvironment.GetGame().GetCommunityGoalVS().GetId());
                }

                RavenEnvironment.GetGame().GetCommunityGoalVS().IncreaseRightVotes();
            }
            RavenEnvironment.GetGame().GetCommunityGoalVS().LoadCommunityGoalVS();
        }
    }
}
