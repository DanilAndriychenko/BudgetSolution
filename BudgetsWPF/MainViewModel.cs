using BudgetsWPF.Authentication;
using BudgetsWPF.Navigation;
using BudgetsWPF.Transactions;
using BudgetsWPF.Wallets;

namespace BudgetsWPF
{
    public class MainViewModel : NavigationBase<MainNavigatableTypes>
    {
        public MainViewModel()
        {
            Navigate(MainNavigatableTypes.Auth);
        }
        
        protected override INavigatable<MainNavigatableTypes> CreateViewModel(MainNavigatableTypes type)
        {
            if (type == MainNavigatableTypes.Auth)
            {
                return new AuthViewModel(() => Navigate(MainNavigatableTypes.Wallets));
            }
            else if (type == MainNavigatableTypes.Wallets)
            {
                return new WalletsViewModel(() => Navigate(MainNavigatableTypes.Transactions));
            }
            else if (type == MainNavigatableTypes.Transactions)
            {
                return new TransactionsViewModel(() => Navigate(MainNavigatableTypes.Wallets));
            }
            else
                return null;
        }
    }
}
