using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Models;

namespace Services
{
    public class WalletService
    {
        public static List<Wallet> Wallets { get; set; }

        public WalletService()
        {
            Wallets = User.CurrUser.Wallets;
        }

        public static List<Wallet> GetWallets()
        {
            return Wallets.ToList();
        }

        public static async Task<bool> AddWallet(Wallet wallet)
        {
            Wallets.Add(wallet);
            await AddWalletAsync(wallet);
            return true;
        }

        public static async Task<bool> DeleteWallet(Wallet wallet)
        {
            Wallets.Remove(wallet);
            await RemoveWalletAsync(wallet);
            return true;
        }

        public static async Task<bool> AddWalletAsync(Wallet wallet)
        {
            var users = await AuthenticationService.GetStorage().GetAllAsync();
            var dbUser = users.FirstOrDefault(user =>
                user.Guid == User.CurrUser.Guid
            );
            if (dbUser == null)
                throw new Exception("User cannot be found");
            dbUser.Wallets.Add(wallet.Guid);
            await AuthenticationService.GetStorage().AddOrUpdateAsync(dbUser);
            return true;
        }

        public static async Task<bool> RemoveWalletAsync(Wallet wallet)
        {
            var users = await AuthenticationService.GetStorage().GetAllAsync();
            var dbUser = users.FirstOrDefault(user => user.Guid == User.CurrUser.Guid);
            if (dbUser == null)
                throw new Exception("User cannot be found");
            dbUser.Wallets.Remove(wallet.Guid);
            await AuthenticationService.GetStorage().AddOrUpdateAsync(dbUser);
            return true;
        }
    }
}