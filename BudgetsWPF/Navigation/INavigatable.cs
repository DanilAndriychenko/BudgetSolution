using System;

namespace AV.ProgrammingWithCSharp.Budgets.GUI.WPF.Navigation
{
    public interface INavigatable<TObject> where TObject: Enum
    {
        public TObject Type { get; }

        public void ClearSensitiveData();
    }
}
