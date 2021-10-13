using System;
using System.Collections.Generic;
using System.Linq;

namespace Vector_unitech_api.Security
{
    public static class UserRepository
    {
        public static User Get( string username, string password )
        {
            var users = new List<User>
            {
                new User { Id = 1, Username = "John", Password = "123", Role = "Admin" },
                new User { Id = 2, Username = "Jane", Password = "132", Role = "" }
            };


            return users.FirstOrDefault( x =>
                 string.Equals( x.Username.ToLower(), username.ToLower(), StringComparison.Ordinal ) && x.Password == x.Password );
        }
    }
}
