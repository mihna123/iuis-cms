using System;
using System.Text;
using System.Collections.Generic;
using System.IO;

namespace CMSystem
{
    public class UserService
    {
        private static readonly string filename = "data/users.json";

        public List<User> LoadUsers()
        {
            var users = new List<User>();
            var json = new Chilkat.JsonArray();
            var jsonFile = File.ReadAllText(filename);

            bool success = json.Load(jsonFile);
            if (success != true)
            {
                Console.WriteLine("Couldn't open {0}", filename);
            }

            for (int i = 0; i < json.Size; i++)
            {
                var jsonUser = json.ObjectAt(i);
                var newUser = new User
                {
                    UserName = jsonUser.StringOf("username"),
                    Password = jsonUser.StringOf("password"),
                    Type = jsonUser.StringOf("usertype") == "admin"
                      ? UserType.Admin
                      : UserType.Visitor
                };
                users.Add(newUser);
            }

            return users;
        }
    }
}
