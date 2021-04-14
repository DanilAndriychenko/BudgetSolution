using System;

namespace BudgetsWPF.Navigation
{
    public interface INavigatable<TObject> where TObject: Enum
    {
        public TObject Type { get; }

        public void ClearSensitiveData();
    }
}
