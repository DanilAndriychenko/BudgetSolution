using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DataStorage;
using Models;

namespace Services
{
    public class AuthenticationService
    {
        private static readonly FileDataStorage<DBUser> _storage = new FileDataStorage<DBUser>();

        public async Task<User> AuthenticateAsync(AuthenticationUser authUser)
        {
            if (String.IsNullOrWhiteSpace(authUser.Login) || String.IsNullOrWhiteSpace(authUser.Password))
                throw new ArgumentException("Login or Password is Empty");
            var users = await _storage.GetAllAsync();
            string password = EncryptPassword(authUser.Password);
            var dbUser = users.FirstOrDefault(user => user.Login == authUser.Login && user.Password == password);
            if (dbUser == null)
                throw new Exception("Wrong Login or Password");
            List<Wallet> wallets = new List<Wallet>(dbUser.Wallets.Count);
            User user = new User(dbUser.Guid, dbUser.FirstName, dbUser.LastName, dbUser.Email, dbUser.Login, wallets);
            //ToDo refactor
            for(int i = 0; i < dbUser.Wallets.Count; i++)
                wallets.Add(new Wallet("New Wallet", user, new List<Category>(), dbUser.Wallets[i]));
            User.CurrUser = user;
            return user;
        }

        public static FileDataStorage<DBUser> GetStorage()
        {
            return _storage;
        }

        private static string EncryptPassword(string password)
        {
            byte[] data = System.Text.Encoding.ASCII.GetBytes(password);
            data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
            return System.Text.Encoding.ASCII.GetString(data);
        }

        public async Task<bool> RegisterUserAsync(RegistrationUser regUser)
        {
            Thread.Sleep(2000);
            var users = await _storage.GetAllAsync();
            var dbUser = users.FirstOrDefault(user => user.Login == regUser.Login);
            if (dbUser != null)
                throw new Exception("User already exists");
            if (String.IsNullOrWhiteSpace(regUser.Login) || String.IsNullOrWhiteSpace(regUser.Password) || String.IsNullOrWhiteSpace(regUser.LastName))
                throw new ArgumentException("Login, Password or Last Name is Empty");
            dbUser = new DBUser(Guid.NewGuid(), regUser.FirstName, regUser.LastName, regUser.Email,
                regUser.Login, EncryptPassword(regUser.Password), new List<Guid>());
            await _storage.AddOrUpdateAsync(dbUser);
            return true;
        }
    }
}
