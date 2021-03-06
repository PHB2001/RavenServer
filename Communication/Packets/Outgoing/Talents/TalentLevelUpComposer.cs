﻿using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Raven.HabboHotel.Achievements;

namespace Raven.Communication.Packets.Outgoing.Talents
{
    class TalentLevelUpComposer : ServerPacket
    {
        public TalentLevelUpComposer(Talent talent)
           : base(ServerPacketHeader.TalentLevelUpMessageComposer)
        {
            base.WriteString(talent.Type);
            base.WriteInteger(talent.Level);
            base.WriteInteger(0);

            if (talent.Type == "citizenship" && talent.Level == 4)
            {
                base.WriteInteger(2);
                base.WriteString("HABBO_CLUB_VIP_7_DAYS");
                base.WriteInteger(7);
                base.WriteString(talent.Prize);
                base.WriteInteger(0);
            }
            else
            {
                base.WriteInteger(1);
                base.WriteString(talent.Prize);
                base.WriteInteger(0);
            }
        }
    }
}
