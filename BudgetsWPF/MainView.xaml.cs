using System.Windows.Controls;
using BudgetsWPF;

namespace AV.ProgrammingWithCSharp.Budgets.GUI.WPF
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }
}
