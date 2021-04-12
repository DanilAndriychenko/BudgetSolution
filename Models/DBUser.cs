using System;
using System.Collections.Generic;
using DataStorage;

namespace Models
{
    public class DbUser : IStorable
    {
        public Guid Guid { get; }
        public string FirstName { get; }

        public string LastName { get; }

        public string Email { get;}
        public string Login { get;}
        public string Password { get;}
        public List<Guid> Wallets { get; }

        public DbUser(Guid guid, string firstName, string lastName, string email, string login, string password, List<Guid> wallets)
        {
            Guid = guid;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Login = login;
            Password = password;
            Wallets = wallets;
        }
    }
}
