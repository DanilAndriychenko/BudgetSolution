using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using AV.ProgrammingWithCSharp.Budgets.GUI.WPF.Navigation;
using BudgetsWPF.Navigation;
using Models;
using Prism.Commands;
using Prism.Mvvm;
using Services;

namespace BudgetsWPF.Wallets
{
    public class WalletsViewModel: BindableBase , INavigatable<MainNavigatableTypes>
    {
        private WalletService _service;
        private WalletDetailsViewModel _currentWallet;
        public ObservableCollection<WalletDetailsViewModel> Wallets { get; set; }

        public WalletDetailsViewModel CurrentWallet
        {
            get
            {
                return _currentWallet;
            }
            set
            {
                _currentWallet = value;
                RaisePropertyChanged();
            }
        }
        public DelegateCommand AddWalletCommand { get; }
        public DelegateCommand DeleteWalletCommand { get; }

        public WalletsViewModel()
        {
            WalletService.Wallets = User.CurrUser.Wallets;
            _service = new WalletService();
            Wallets = new ObservableCollection<WalletDetailsViewModel>();
            AddWalletCommand = new DelegateCommand(AddWallet);
            DeleteWalletCommand = new DelegateCommand(DeleteWallet);
            foreach (var wallet in WalletService.GetWallets())
            {
                Wallets.Add(new WalletDetailsViewModel(wallet));
            }
        }


        public MainNavigatableTypes Type 
        {
            get
            {
                return MainNavigatableTypes.Wallets;
            }
        }
        public void ClearSensitiveData()
        {
            
        }

        private async void AddWallet()
        {
            Wallet wallet = new Wallet("New Wallet", User.CurrUser, new List<Category>(), Guid.NewGuid());
            await WalletService.AddWallet(wallet);
            Wallets.Add(new WalletDetailsViewModel(wallet));
            RaisePropertyChanged();
        }

        private void DeleteWallet()
        {
            if (_currentWallet == null)
                return;
            WalletService.DeleteWallet(_currentWallet.Wallet);
            Wallets.Remove(_currentWallet);
        }
        
    }
}
