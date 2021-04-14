using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataStorage;
using Models;

namespace Services
{
    public class TransactionService
    {
        private static FileDataStorage<DbTransaction> Storage = new();
        public static List<Transaction> Transactions { get; set; }

        public TransactionService()
        {
            Transactions = new List<Transaction>();
        }

        public async Task<IEnumerable<Transaction>> GetTransactions(Wallet walletFrom)
        {
            var transactions = await Storage.GetAllAsync();
            List<Transaction> res = new List<Transaction>();
            foreach (var walletTransaction in walletFrom.Transactions)
            {
                var transaction = transactions.FirstOrDefault(t =>
                    t.Guid == walletTransaction.Guid
                );
                if (transaction != null)
                    res.Add(new Transaction(transaction.Value, transaction.Currency,
                        transaction.Date, transaction.Guid, transaction.Description));
            }

            return res;
        }

        public static async Task AddTransaction(Transaction transaction, Wallet wallet)
        {
            await AddTransactionAsync(transaction, wallet);
        }

        public static async Task AddTransactionAsync(Transaction transaction, Wallet wallet)
        {
            var dbWallet = await WalletService.GetStorage().GetAsync(wallet.Guid);
            if (dbWallet == null)
                throw new Exception("Wallet cannot be found");
            dbWallet.Transactions.Add(transaction.Guid);
            await Storage.AddOrUpdateAsync(new DbTransaction(transaction.Value, transaction.Currency,
                transaction.Date, transaction.Guid, transaction.Description));
            await WalletService.GetStorage().AddOrUpdateAsync(dbWallet);
        }

        public static async Task RemoveTransaction(Transaction transaction, Wallet wallet)
        {
            await RemoveTransactionAsync(transaction, wallet);
        }

        public static async Task RemoveTransactionAsync(Transaction transaction, Wallet wallet)
        {
            var dbWallet = await WalletService.GetStorage().GetAsync(wallet.Guid);
            if (dbWallet == null)
                throw new Exception("Wallet cannot be found");
            dbWallet.Transactions.Remove(transaction.Guid);
            await WalletService.GetStorage().AddOrUpdateAsync(dbWallet);
        }

        public static FileDataStorage<DbTransaction> GetStorage()
        {
            return Storage;
        }
    }
}