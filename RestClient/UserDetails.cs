using System;
using System.Collections.Generic;
using System.Text;

namespace UserConnector
{
    public class UserDetails
    {
        public int Page { get; set; }

        public List<User> Data { get; set; }
    }

    public class User
    {
        public int Id { get; set; }

        public string Email { get; set; }
    }
}
