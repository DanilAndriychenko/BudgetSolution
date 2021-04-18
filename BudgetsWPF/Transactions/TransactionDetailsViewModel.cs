using System;
using Models;
using Prism.Commands;
using Prism.Mvvm;
using Services;

namespace BudgetsWPF.Transactions
{
    public class TransactionDetailsViewModel: BindableBase
    {
        public Transaction Transaction { get; private set; }
        public TransactionDetailsViewModel(Transaction transaction)
        {
            Transaction = transaction;
        }

        public string Value
        {
            get => Transaction?.Value.ToString();
            set
            {
                try
                {
                    decimal result = decimal.Parse(value);
                    Transaction.Value = result;
                }
                catch (Exception e)
                {
                    // ignored
                }
                _ = TransactionService.GetStorage().AddOrUpdateAsync(new DbTransaction(Transaction.Value,
                    Transaction.Currency,
                    Transaction.Date, Transaction.Guid, Transaction.Description, Transaction.Files));
                RaisePropertyChanged(nameof(DisplayName));
            }
        }
        public string CurrencyStr
        {
            get => Transaction?.Currency.ToString().ToUpper();
            set
            {
                if (Transaction == null)
                    return;
                Transaction.Currency = value switch
                {
                    "USD" => Currency.Usd,
                    "EUR" => Currency.Eur,
                    "UAH" => Currency.Uah,
                    _ => Transaction.Currency
                };
                _ = TransactionService.GetStorage().AddOrUpdateAsync(new DbTransaction(Transaction.Value,
                    Transaction.Currency,
                    Transaction.Date, Transaction.Guid, Transaction.Description, Transaction.Files));
                RaisePropertyChanged(nameof(DisplayName));
            }
        }

        public string Description
        {
            get => Transaction.Description;
            set
            {
                Transaction.Description = value;
                _ = TransactionService.GetStorage().AddOrUpdateAsync(new DbTransaction(Transaction.Value,
                   Transaction.Currency,
                   Transaction.Date, Transaction.Guid, Transaction.Description, Transaction.Files));
            }
        }

        public DateTime Date
        {
            get => Transaction.Date;
            set
            {
                Transaction.Date = value;
                _ = TransactionService.GetStorage().AddOrUpdateAsync(new DbTransaction(Transaction.Value,
                   Transaction.Currency,
                   Transaction.Date, Transaction.Guid, Transaction.Description, Transaction.Files));
            }
        }
        public string DisplayName => $"{Transaction.Value} {Transaction.Currency}";
        
    }
}
