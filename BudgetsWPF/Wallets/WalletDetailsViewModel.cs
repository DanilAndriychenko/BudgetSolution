using Models;
using Prism.Commands;
using Prism.Mvvm;
using Services;

namespace BudgetsWPF.Wallets
{
    public class WalletDetailsViewModel : BindableBase
    {
        public Wallet Wallet { get; }

        public string Name
        {
            get
            {
                return Wallet.Name;
            }
            set
            {
                Wallet.Name = value;
                RaisePropertyChanged(nameof(DisplayName));
            }
        }

        public decimal Balance
        {
            get
            {
                return Wallet.Balance;
            }
            set
            {
                Wallet.Balance = value;
                RaisePropertyChanged(nameof(DisplayName));
            }
        }

        public string DisplayName
        {
            get
            {
                return $"{Wallet.Name} (${Wallet.Balance})";
            }
        }

        public WalletDetailsViewModel(Wallet wallet)
        {
            Wallet = wallet;
        }

    }
}
