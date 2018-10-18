using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Raven.HabboHotel.Groups;

namespace Raven.Communication.Packets.Outgoing.Catalog
{
    class GroupFurniConfigComposer : ServerPacket
    {
        public GroupFurniConfigComposer(ICollection<Group> Groups)
            : base(ServerPacketHeader.GroupFurniConfigMessageComposer)
        {
            base.WriteInteger(Groups.Count);
            foreach (Group Group in Groups)
            {
                base.WriteInteger(Group.Id);
                base.WriteString(Group.Name);
                base.WriteString(Group.Badge);
                base.WriteString((RavenEnvironment.GetGame().GetGroupManager().SymbolColours.ContainsKey(Group.Colour1)) ? RavenEnvironment.GetGame().GetGroupManager().SymbolColours[Group.Colour1].Colour : "4f8a00"); // Group Colour 1
                base.WriteString((RavenEnvironment.GetGame().GetGroupManager().BackGroundColours.ContainsKey(Group.Colour2)) ? RavenEnvironment.GetGame().GetGroupManager().BackGroundColours[Group.Colour2].Colour : "4f8a00"); // Group Colour 2            
                base.WriteBoolean(false);
                base.WriteInteger(Group.CreatorId);
                base.WriteBoolean(Group.ForumEnabled);
            }
        }
    }
}
