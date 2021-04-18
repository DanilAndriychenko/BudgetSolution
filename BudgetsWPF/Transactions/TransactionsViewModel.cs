using System;
using System.Collections.ObjectModel;
using BudgetsWPF.Navigation;
using Prism.Commands;
using Prism.Mvvm;
using Services;
using Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BudgetsWPF.Transactions
{
    public class TransactionsViewModel : BindableBase, INavigatable<MainNavigatableTypes>
    {
        private Action _goToWallets;
        private Wallet _wallet;

        private TransactionService _service = new();
        private TransactionDetailsViewModel _currentTransaction;
        public TransactionDetailsViewModel CurrentTransaction
        {
            get => _currentTransaction;
            set
            {
                _currentTransaction = value;
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<TransactionDetailsViewModel> Transactions { get; set; }

        public DelegateCommand AddTransactionCommand { get; }
        public DelegateCommand DeleteTransactionCommand { get; }
        public DelegateCommand GoBackCommand { get; }
        public TransactionsViewModel(Action goToWallets)
        {
            _wallet = Wallet.CurrWallet;
            _goToWallets = goToWallets;

            AddTransactionCommand = new DelegateCommand(AddTransaction);
            DeleteTransactionCommand = new DelegateCommand(DeleteTransaction);
            GoBackCommand = new DelegateCommand(async () => { goToWallets(); await saveTransactions(); });

            Transactions = new ObservableCollection<TransactionDetailsViewModel>();
            InitializeTransactions();
        }

        private async Task<bool> saveTransactions()
        {
            foreach (TransactionDetailsViewModel transactionVM in Transactions)
            {
                var transaction = transactionVM.Transaction;

                DbTransaction dbTransaction = new DbTransaction(
                   transaction.Value, transaction.Currency, transaction.Date,
                   transaction.Guid, transaction.Description, transaction.Files);
                await TransactionService.GetStorage().AddOrUpdateAsync(dbTransaction);
            }
            var transactionsGuids = new List<Guid>();
            foreach (var t in Wallet.CurrWallet.Transactions)
                transactionsGuids.Add(t.Guid);
            var dbWallet = new DbWallet(Wallet.CurrWallet.Name, Wallet.CurrWallet.Description, Wallet.CurrWallet.Balance, Wallet.CurrWallet.Currency, transactionsGuids, Wallet.CurrWallet.Owner.Guid, Wallet.CurrWallet.Guid);
            await WalletService.GetStorage().AddOrUpdateAsync(dbWallet);
            return true;
        }

        private async void AddTransaction()
        {
            var transaction = new Transaction(0, Currency.Eur, DateTime.Now, Guid.NewGuid());
            Transactions.Add(new TransactionDetailsViewModel(transaction));
            Wallet.CurrWallet.AddTransaction(transaction);
        }

        private async void DeleteTransaction()
        {
            if (_currentTransaction != null)
            {
                Wallet.CurrWallet.RemoveTransaction(_currentTransaction.Transaction);
                Transactions.Remove(_currentTransaction);
            }
        }

        private void InitializeTransactions()
        {
            Transactions.Clear();
            foreach (var t in Wallet.CurrWallet.Transactions)
            {
                Transactions.Add(new TransactionDetailsViewModel(t));
            }
        }


        public MainNavigatableTypes Type => MainNavigatableTypes.Transactions;

        public void ClearSensitiveData()
        {
            InitializeTransactions();
            _currentTransaction = null;
        }
    }
}
