using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BudgetConsoleApp
{
    public enum Currency
    {
        Uah = 1,
        Usd = 28,
        Eur = 33
    }

    public class Wallet
    {
        private const int NumOfTransactionAtOnce = 10;
        public string Name { get; set; }
        public decimal Balance { get; private set; }
        public string Description { get; }
        public Currency Currency { get; }
        private readonly SortedSet<Transaction> _transactions;
        public List<Category> Categories { get; }
        public User Owner { get; }
        public List<User> Contributors { get; }

        public Wallet(string name, User owner, List<Category> categories, decimal balance = 0,
            Currency currency = Currency.Uah, string description = "")
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Categories = categories ?? throw new ArgumentNullException(nameof(categories));
            Balance = balance;
            Currency = currency;
            Description = description ?? throw new ArgumentNullException(nameof(description));
            _transactions = new SortedSet<Transaction>(Comparer<Transaction>.Create((a, b)
                => b.DateTime.CompareTo(a.DateTime)));
            Owner = owner;
            Contributors = new List<User> {owner ?? throw new ArgumentNullException(nameof(owner))};
        }

        public void AddTransaction(decimal value, Currency currency,
            Category category, DateTime dateTime, string description = "", List<FileInfo> files = null)
        {
            if (Categories.Contains(category))
            {
                _transactions.Add(new Transaction(value, currency, category, dateTime, description, files));
                Balance += Transaction.GetConvertedPrice(value, currency, Currency);
            }
            else
            {
                throw new ArgumentException("Category of transaction(" + category +
                                            ") doesn't match wallet's category list.");
            }
        }

        public void AddTransaction(decimal value,
            Category category, DateTime dateTime, string description = "", List<FileInfo> files = null)
        {
            AddTransaction(value, Currency, category, dateTime, description, files);
        }

        // Returns NumOfTransactionAtOnce from start if there is no transaction at start returns null
        public List<Transaction> GetTransactions(int start = 0)
        {
            if (_transactions.Count < start)
            {
                return new List<Transaction>();
            }
            var counter = 0;
            var result = new List<Transaction>();
            foreach (var transaction in _transactions)
            {
                counter++;
                result.Add(transaction);
                if (counter == NumOfTransactionAtOnce + start)
                {
                    break;
                }
            }
            return result;
        }

        // TODO distinct common method for this two functions
        public decimal GetSpending()
        {
            return _transactions.TakeWhile(transaction => transaction.DateTime.Month == DateTime.Now.Month)
                .Where(transaction => transaction.Value < 0)
                .Aggregate<Transaction, decimal>(0, (current, transaction) => current - transaction.GetValue(Currency));
        }

        public decimal GetIncome()
        {
            return _transactions.TakeWhile(transaction => transaction.DateTime.Month == DateTime.Now.Month)
                .Where(transaction => transaction.Value > 0)
                .Aggregate<Transaction, decimal>(0, (current, transaction) => current + transaction.GetValue(Currency));
        }
    }
}