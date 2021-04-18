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
        public DelegateCommand GoToTransactions { get; }

        public WalletDetailsViewModel(Wallet wallet, Action goToTransactions)
        {
            Wallet = wallet;
            GoToTransactions = new DelegateCommand(()=> {  Wallet.CurrWallet = Wallet; goToTransactions(); });
        }

        public string Name
        {
            get => Wallet.Name;
            set
            {
                Wallet.Name = value;
                List<Guid> transactionGuids = new List<Guid>();
                foreach (var t in Wallet.Transactions)
                {
                    transactionGuids.Add(t.Guid);
                }

                _ = WalletService.GetStorage().AddOrUpdateAsync(new DbWallet(value, Wallet.Description, Wallet.Balance,
                    Wallet.Currency, transactionGuids, Wallet.Owner.Guid, Wallet.Guid));
                RaisePropertyChanged(nameof(DisplayName));
            }
        }

        public decimal Balance
        {
            get => Wallet.Balance;
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
                List<Guid> transactionGuids = new List<Guid>();
                foreach (var t in Wallet.Transactions)
                {
                    transactionGuids.Add(t.Guid);
                }

                _ = WalletService.GetStorage().AddOrUpdateAsync(new DbWallet(Wallet.Name, Wallet.Description,
                    Wallet.Balance,
                    Wallet.Currency, transactionGuids, Wallet.Owner.Guid, Wallet.Guid));
                RaisePropertyChanged(nameof(DisplayName));
                RaisePropertyChanged(nameof(Balance));
                RaisePropertyChanged(nameof(Income));
                RaisePropertyChanged(nameof(Outcome));
            }
        }

        public string Description
        {
            get => Wallet.Description;
            set
            {
                Wallet.Description = value;
                List<Guid> transactionGuids = new List<Guid>();
                foreach (var t in Wallet.Transactions)
                {
                    transactionGuids.Add(t.Guid);
                }

                _ = WalletService.GetStorage().AddOrUpdateAsync(new DbWallet(Wallet.Name, Wallet.Description,
                    Wallet.Balance,
                    Wallet.Currency, transactionGuids, Wallet.Owner.Guid, Wallet.Guid));
                RaisePropertyChanged(nameof(DisplayName));
            }
        }

        private int _transactionsStartingIndex { get; set; }

        public int TransactionsStartingIndex
        {
            get
            {
                return _transactionsStartingIndex;
            }
            set
            {
                _transactionsStartingIndex = value;
                RaisePropertyChanged(nameof(LatestTransactionsList));
            }
        }

        public List<Transaction> LatestTransactionsList
        {
            get
            {
                return Wallet.GetTransactionsList(_transactionsStartingIndex);
            }
        }

        public decimal Income 
        { 
            get
            {
                return Wallet.GetIncome();
            }
        }
        public decimal Outcome
        {
            get
            {
                return Wallet.GetSpending();
            }
        }

        public string DisplayName => $"{Wallet.Name} (${Wallet.Balance})";

    }
}