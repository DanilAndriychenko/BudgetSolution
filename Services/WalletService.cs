using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataStorage;
using Models;

namespace Services
{
    public class WalletService
    {
        private static readonly FileDataStorage<DbWallet> Storage = new();
        public static List<Wallet> Wallets { get; set; }

        public WalletService()
        {
            Wallets = User.CurrUser.Wallets;
        }

        public static IEnumerable<Wallet> GetWallets()
        {
            return Wallets.ToList();
        }

        public static async Task AddWallet(Wallet wallet)
        {
            Wallets.Add(wallet);
            await AddWalletAsync(wallet);
        }

        public static async Task DeleteWallet(Wallet wallet)
        {
            Wallets.Remove(wallet);
            await RemoveWalletAsync(wallet);
        }

        public static FileDataStorage<DbWallet> GetStorage()
        {
            return Storage;
        }

        public static async Task AddWalletAsync(Wallet wallet)
        {
            var users = await AuthenticationService.GetStorage().GetAllAsync();
            var dbUser = users.FirstOrDefault(user =>
                user.Guid == User.CurrUser.Guid
            );
            if (dbUser == null)
                throw new Exception("User cannot be found");
            dbUser.Wallets.Add(wallet.Guid);
            List<Guid> transactionGuids = new List<Guid>();
            foreach (var t in wallet.Transactions)
            {
                transactionGuids.Add(t.Guid);
            }

            await Storage.AddOrUpdateAsync(new DbWallet(wallet.Name, wallet.Description, wallet.Balance,
                wallet.Currency, transactionGuids, dbUser.Guid, wallet.Guid));
            await AuthenticationService.GetStorage().AddOrUpdateAsync(dbUser);
        }

        public static async Task RemoveWalletAsync(Wallet wallet)
        {
            var users = await AuthenticationService.GetStorage().GetAllAsync();
            var dbUser = users.FirstOrDefault(user => user.Guid == User.CurrUser.Guid);
            if (dbUser == null)
                throw new Exception("User cannot be found");
            dbUser.Wallets.Remove(wallet.Guid);
            await AuthenticationService.GetStorage().AddOrUpdateAsync(dbUser);
        }
    }
}