using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Models;
using Prism.Commands;
using Prism.Mvvm;
using Services;

namespace BudgetsWPF.Wallets
{
    public class WalletDetailsViewModel : BindableBase
    {
        public Wallet Wallet { get; }
        public ObservableCollection<Transaction> Transactions { get; set; }
        private TransactionService _service = new();
        private Transaction _currTransaction;

        public Transaction Transaction
        {
            get { return _currTransaction; }
            set
            {
                _currTransaction = value;
                RaisePropertyChanged();
            }
        }

        public DelegateCommand AddTransactionCommand { get; }
        public DelegateCommand DeleteTransactionCommand { get; }

        public WalletDetailsViewModel(Wallet wallet)
        {
            Wallet = wallet;
            Transactions = new ObservableCollection<Transaction>();
            AddTransactionCommand = new DelegateCommand(AddTransaction);
            DeleteTransactionCommand = new DelegateCommand(DeleteTransaction);
            InitializeTransactions();
        }
        
        private async void InitializeTransactions()
        {
            var dbTransactions = await _service.GetTransactions(Wallet);
            foreach (var t in dbTransactions)
            {
                Transactions.Add(t);
            }
        }

        public string Name
        {
            get => Wallet.Name;
            set
            {
                Wallet.Name = value;
                SortedSet<Guid> transactionGuids = new SortedSet<Guid>();
                foreach (var t in Wallet.Transactions)
                {
                    transactionGuids.Add(t.Guid);
                }

                WalletService.GetStorage().AddOrUpdateAsync(new DbWallet(value, Wallet.Description, Wallet.Balance,
                    Wallet.Currency, transactionGuids,  Wallet.Owner.Guid, Wallet.Guid));
                RaisePropertyChanged(nameof(DisplayName));
            }
        }

        public decimal Balance
        {
            get => Wallet.Balance;
            set
            {
                Wallet.Balance = value;
                SortedSet<Guid> transactionGuids = new SortedSet<Guid>();
                foreach (var t in Wallet.Transactions)
                {
                    transactionGuids.Add(t.Guid);
                }

                WalletService.GetStorage().AddOrUpdateAsync(new DbWallet(Wallet.Name, Wallet.Description, value,
                    Wallet.Currency, transactionGuids,  Wallet.Owner.Guid, Wallet.Guid));
                RaisePropertyChanged(nameof(DisplayName));
            }
        }

        public string CurrencyStr
        {
            get => Wallet.Currency.ToString().ToUpper();
            set
            {
                Wallet.Currency = value switch
                {
                    "USD" => Currency.Usd,
                    "EUR" => Currency.Eur,
                    "UAH" => Currency.Uah,
                    _ => Wallet.Currency
                };
                SortedSet<Guid> transactionGuids = new SortedSet<Guid>();
                foreach (var t in Wallet.Transactions)
                {
                    transactionGuids.Add(t.Guid);
                }

                WalletService.GetStorage().AddOrUpdateAsync(new DbWallet(Wallet.Name, Wallet.Description,
                    Wallet.Balance,
                    Wallet.Currency, transactionGuids, Wallet.Owner.Guid, Wallet.Guid));
                RaisePropertyChanged(nameof(DisplayName));
            }
        }

        public string CurrencyStrTransaction
        {
            get => Transaction.Currency.ToString().ToUpper();
            set
            {
                if (Transaction == null)
                    return;
                Transaction.Currency = value switch
                {
                    "USD" => Currency.Usd,
                    "EUR" => Currency.Eur,
                    "UAH" => Currency.Uah,
                    _ => Wallet.Currency
                };
                TransactionService.GetStorage().AddOrUpdateAsync(new DbTransaction(Transaction.Value,
                    Transaction.Currency,
                    Transaction.Date, Transaction.Guid, Transaction.Description, Transaction.Files));
                RaisePropertyChanged(TransactionDisplayName);
            }
        }
        public string Description
        {
            get => Wallet.Description;
            set
            {
                Wallet.Description = value;
                SortedSet<Guid> transactionGuids = new SortedSet<Guid>();
                foreach (var t in Wallet.Transactions)
                {
                    transactionGuids.Add(t.Guid);
                }

                WalletService.GetStorage().AddOrUpdateAsync(new DbWallet(Wallet.Name, Wallet.Description,
                    Wallet.Balance,
                    Wallet.Currency, transactionGuids, Wallet.Owner.Guid, Wallet.Guid));
                RaisePropertyChanged(nameof(DisplayName));
            }
        }

        public string TransactionValue
        {
            get => Transaction.Value.ToString();
            set
            {
                try
                {
                    decimal result = decimal.Parse(value);
                    Transaction.Value = result;
                }
                catch (Exception e)
                {
                    // ignored
                }
                TransactionService.GetStorage().AddOrUpdateAsync(new DbTransaction(Transaction.Value,
                    Transaction.Currency,
                    Transaction.Date, Transaction.Guid, Transaction.Description, Transaction.Files));
                RaisePropertyChanged(TransactionDisplayName);
            }
        }

        public string DisplayName => $"{Wallet.Name} (${Wallet.Balance})";
        public string TransactionDisplayName => $"{Transaction.Value} (${Transaction.Currency})";


        private async void AddTransaction()
        {
            var transaction = new Transaction(0, Currency.Eur, DateTime.Now, Guid.NewGuid());
            await TransactionService.AddTransaction(transaction, Wallet);
            Transactions.Add(transaction);
        }

        private async void DeleteTransaction()
        {
            if(_currTransaction != null)
            {
                await TransactionService.RemoveTransaction(_currTransaction, Wallet);
                Transactions.Remove(_currTransaction);
            }
        }
    }
}