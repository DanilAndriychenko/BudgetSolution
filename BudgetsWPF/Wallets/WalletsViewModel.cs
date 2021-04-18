using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using BudgetsWPF.Navigation;
using Models;
using Prism.Commands;
using Prism.Mvvm;
using Services;

namespace BudgetsWPF.Wallets
{
    public class WalletsViewModel: BindableBase , INavigatable<MainNavigatableTypes>
    {
        private WalletDetailsViewModel _currentWallet;
        public ObservableCollection<WalletDetailsViewModel> Wallets { get; set; }

        public WalletDetailsViewModel CurrentWallet
        {
            get => _currentWallet;
            set
            {
                _currentWallet = value;
                Wallet.CurrWallet = _currentWallet.Wallet;
                RaisePropertyChanged();
            }
        }
        public DelegateCommand AddWalletCommand { get; }
        public DelegateCommand DeleteWalletCommand { get; }
        private Action _goToTransactions;

        public WalletsViewModel(Action goToTransactions)
        {
            _goToTransactions = goToTransactions;
            WalletService.Wallets = User.CurrUser.Wallets;
            Wallets = new ObservableCollection<WalletDetailsViewModel>();
            AddWalletCommand = new DelegateCommand(AddWallet);
            DeleteWalletCommand = new DelegateCommand(DeleteWallet);
            foreach (var wallet in WalletService.GetWallets())
            {
                Wallets.Add(new WalletDetailsViewModel(wallet, _goToTransactions));
            }
        }

        private async void AddWallet()
        {
            var wallet = new Wallet("New Wallet", User.CurrUser, Guid.NewGuid(), new SortedSet<Transaction>());
            await WalletService.AddWallet(wallet);
            Wallets.Add(new WalletDetailsViewModel(wallet, _goToTransactions));
            RaisePropertyChanged();
        }

        private async void DeleteWallet()
        {
            if (_currentWallet == null) return;
            await WalletService.DeleteWallet(_currentWallet.Wallet);
            Wallets.Remove(_currentWallet);
        }
        public MainNavigatableTypes Type => MainNavigatableTypes.Wallets;

        public void ClearSensitiveData()
        {
            
        }
        
    }
}
