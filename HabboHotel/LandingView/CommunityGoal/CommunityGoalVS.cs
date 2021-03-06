﻿using log4net;
using Raven.Database.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.HabboHotel.LandingView.CommunityGoal
{
    public class CommunityGoalVS
    {
        private static readonly ILog log = LogManager.GetLogger("Raven.HabboHotel.LandingView.CommunityGoalVS");

        private int Id;
        private string Name;
        private int LeftVotes;
        private int RightVotes;

        public void LoadCommunityGoalVS()
        {
            using (IQueryAdapter dbClient = RavenEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT * FROM `landing_communitygoalvs` ORDER BY `id` DESC LIMIT 1");
                DataRow dRow = dbClient.getRow();

                if (dRow != null)
                {
                    Id = (int)dRow["id"];
                    Name = (string)dRow["name"];
                    LeftVotes = (int)dRow["left_votes"];
                    RightVotes = (int)dRow["right_votes"];
                }
            }
        }

        public int GetId()
        {
            return Id;
        }

        public string GetName()
        {
            return Name;
        }

        public int GetLeftVotes()
        {
            return LeftVotes;
        }

        public int GetRightVotes()
        {
            return RightVotes;
        }

        public void IncreaseLeftVotes()
        {
            LeftVotes++;
        }

        public void IncreaseRightVotes()
        {
            RightVotes++;
        }
    }
}
