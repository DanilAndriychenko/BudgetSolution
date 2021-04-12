using AV.ProgrammingWithCSharp.Budgets.GUI.WPF.Navigation;
using BudgetsWPF.Authentication;
using BudgetsWPF.Navigation;
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
            else
            {
                return new WalletsViewModel();
            }
        }
    }
}
