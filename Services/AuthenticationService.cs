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
        private static readonly FileDataStorage<DbUser> Storage = new();

        public static async Task<User> AuthenticateAsync(AuthenticationUser authUser)
        {
            if (string.IsNullOrWhiteSpace(authUser.Login) || string.IsNullOrWhiteSpace(authUser.Password))
                throw new ArgumentException("Login or Password is Empty");
            var users = await Storage.GetAllAsync();
            var password = EncryptPassword(authUser.Password);
            var dbUser = users.FirstOrDefault(user => user.Login == authUser.Login && user.Password == password);
            if (dbUser == null)
                throw new Exception("Wrong Login or Password");
            var wallets = new List<Wallet>(dbUser.Wallets.Count);
            var user = new User(dbUser.Guid, dbUser.FirstName, dbUser.LastName, dbUser.Email, dbUser.Login, wallets);
            for (int i = 0; i < dbUser.Wallets.Count; i++)
            {
                DbWallet wallet = await WalletService.GetStorage().GetAsync(dbUser.Wallets[i]);
                SortedSet<Transaction> transactions = new SortedSet<Transaction>();
                foreach (var tGuid in wallet.Transactions)
                {
                    var t = await TransactionService.GetStorage().GetAsync(tGuid);
                    transactions.Add(new Transaction(t.Value, t.Currency, t.Date, t.Guid, t.Description, t.Files));
                }
                wallets.Add(new Wallet(wallet.Name, user, wallet.Guid, transactions, wallet.Balance,
                                                wallet.Currency, wallet.Description));
            }
            User.CurrUser = user;
            return user;
        }

        public static FileDataStorage<DbUser> GetStorage()
        {
            return Storage;
        }

        private static string EncryptPassword(string password)
        {
            var data = System.Text.Encoding.ASCII.GetBytes(password);
            data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
            return System.Text.Encoding.ASCII.GetString(data);
        }

        public static async Task RegisterUserAsync(RegistrationUser regUser)
        {
            Thread.Sleep(2000);
            var users = await Storage.GetAllAsync();
            var dbUser = users.FirstOrDefault(user => user.Login == regUser.Login);
            if (dbUser != null)
                throw new Exception("User already exists");
            if (string.IsNullOrWhiteSpace(regUser.Login) || string.IsNullOrWhiteSpace(regUser.Password) ||
                string.IsNullOrWhiteSpace(regUser.LastName))
                throw new ArgumentException("Login, Password or Last Name is Empty");
            dbUser = new DbUser(Guid.NewGuid(), regUser.FirstName, regUser.LastName, regUser.Email,
                regUser.Login, EncryptPassword(regUser.Password), new List<Guid>());
            await Storage.AddOrUpdateAsync(dbUser);
        }
    }
}