using System;
using System.Collections.ObjectModel;
using Models;
using Prism.Commands;
using Prism.Mvvm;

namespace BudgetsWPF.Wallets
{
    public class TransactionsViewModel: BindableBase
    {
        public Transaction Transaction { get; }
        public ObservableCollection<string> Transactions { get; set; }

        public decimal Value
        {
            get
            {
                return Transaction.Value;
            }
            set
            {
                Transaction.Value = value;
                //todo add transaction service
                /*WalletService.GetStorage().AddOrUpdateAsync(new DbWallet(value, Wallet.Description, Wallet.Balance,
                    Wallet.Currency, Wallet.Transactions, Wallet.Categories, Wallet.Owner.Guid, Wallet.Guid));*/
                RaisePropertyChanged(nameof(DisplayName));
            }
        }
        
        public string DisplayName => $"{Transaction.Value} (${Transaction.Currency})";
        public DelegateCommand AddTransactionCommand { get; }

        public TransactionsViewModel(Transaction transaction)
        {
            Transaction = transaction;
            AddTransactionCommand = new DelegateCommand(AddTransaction);
            // AddTransactionCommand
        }
        private void AddTransaction()
        {
            var transaction = new Transaction(0, Currency.Eur, DateTime.Now, Guid.NewGuid());
            // await WalletService.AddWallet(wallet);
            Transactions.Add("New Transaction");
            RaisePropertyChanged();
        }
        
    }
}