using System;

namespace Raven.HabboHotel.Users.UserDataManagement
{
    public class UserDataNotFoundException : Exception
    {
        public UserDataNotFoundException(string reason)
            : base(reason)
        {
        }
    }
}