using System;

namespace CMSystem
{

    public class User
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public UserType Type { get; set; }
    }


    public enum UserType
    {
        Admin, Visitor
    }
}
