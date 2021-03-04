using System;
using System.Collections.Generic;
using System.IO;

namespace Budget
{

    public enum Currency
    {
        Uah = 1,
        Usd = 28,
        Eur = 33
    }

    public class Wallet
    {
        private const int NUM_OF_TRANSACTION_AT_ONCE = 10;
        public string Name { get; set; }
        public double Balance { get; private set; }
        public string Description { get; }
        public Currency Currency { get; }

        private SortedSet<Transaction> Transactions;
        public List<Category> Categories { get; }
        public List<User> Owners { get; }

        public Wallet(string name, User owner, List<Category> categories, double balance = 0,
            Currency currency = Currency.Uah, string description = "")
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Categories = categories ?? throw new ArgumentNullException(nameof(categories));
            Balance = balance;
            Currency = currency;
            Description = description ?? throw new ArgumentNullException(nameof(description));
            Transactions = new SortedSet<Transaction>(Comparer<Transaction>.Create((a, b) => b.DateTime.CompareTo(a.DateTime)));
            Owners = new List<User> { owner ?? throw new ArgumentNullException(nameof(owner)) };
        }

        public void AddTransaction(double value, Currency currency,
            Category category, DateTime dateTime, string description = "", List<FileInfo> files = null)
        {
            if (Categories.Contains(category))
            {
                Transaction.AddTo(this, value, currency, category, dateTime, description, files);
                Balance += Transaction.GetConvertedPrice(value, currency, Currency);
            }
            else throw new ArgumentException("Category of transaction(" + category + ") doesn't match wallet's category list."/*"Transaction category doesn't match wallet's category list."*/);
        }

        public void AddTransaction(double value,
           Category category, DateTime dateTime, string description = "", List<FileInfo> files = null)
        {
            if (Categories.Contains(category))
            {
                Transaction.AddTo(this, value, Currency, category, dateTime, description, files);
                Balance += Transaction.GetConvertedPrice(value, Currency, Currency);
            }
            else throw new ArgumentException("Category of transaction(" + category + ") doesn't match wallet's category list."/*"Transaction category doesn't match wallet's category list."*/);
        }

        //returns first NUM_OF_TRANSACTION_AT_ONCE transactions
        public SortedSet<Transaction> GetTransactions()
        {
            if (Transactions.Count < NUM_OF_TRANSACTION_AT_ONCE)
                return Transactions;
            int counter = 0;
            SortedSet<Transaction> result = new SortedSet<Transaction>(Transactions.Comparer);
            foreach (var transaction in Transactions)
            {
                counter++;
                result.Add(transaction);
                if (counter == NUM_OF_TRANSACTION_AT_ONCE)
                    break;
            }
            return result;
        }

        //returns NUM_OF_TRANSACTION_AT_ONCE from start
        //if there is no transacion at start returns null
        public SortedSet<Transaction> GetTransactions(int start)
        {
            if (Transactions.Count < start)
                return null;
            int counter = 0;
            SortedSet<Transaction> result = new SortedSet<Transaction>();
            foreach (var transaction in Transactions)
            {
                counter++;
                result.Add(transaction);
                if (counter == NUM_OF_TRANSACTION_AT_ONCE + start)
                    break;
            }
            return result;
        }

        public double Spendings()
        {
            double spendings = 0;
            foreach (var transaction in Transactions)
            {
                if (transaction.DateTime.Month != DateTime.Now.Month)
                    break;
                spendings += (transaction.GetValue(Currency) < 0) ? -transaction.GetValue(Currency) : 0;
            }
            return spendings;

        }
        public double Income()
        {
            double spendings = 0;
            foreach (var transaction in Transactions)
            {
                if (transaction.DateTime.Month != DateTime.Now.Month)
                    break;
                spendings += (transaction.GetValue(Currency) > 0) ? transaction.GetValue(Currency) : 0;
            }
            return spendings;

        }

        public class Transaction
        {
            public double Value { get; set; }
            public Currency Currency { get; }
            public Category Category { get; set; }
            public string Description { get; set; }
            public DateTime DateTime { get; private set; }
            public List<FileInfo> Files { get; set; }

            private Transaction(double value, Currency currency, Category category, DateTime dateTime,
                string description = "", List<FileInfo> files = null)
            {
                Value = value;
                Currency = currency;
                Category = category ?? throw new ArgumentNullException(nameof(category));
                Description = description ?? throw new ArgumentNullException(nameof(description));
                DateTime = dateTime;
                Files = files ?? new List<FileInfo>();
            }

            public static void AddTo(Wallet wallet, double value, Currency currency,
            Category category, DateTime dateTime, string description = "", List<FileInfo> files = null)
            {
                wallet.Transactions.Add(new Transaction(value, currency, category, dateTime, description, files));
            }
            public static double GetConvertedPrice(double price, Currency currencyOfPrice, Currency currencyToConvert)
            {
                if (currencyOfPrice == currencyToConvert) return price;
                return Math.Round((price * (int)currencyOfPrice) / (double)currencyToConvert, 2);
            }

            public double GetValue(Currency currency)
            {
                return GetConvertedPrice(Value, Currency, currency);
            }

            override public string ToString()
            {
                return string.Format(Value + " " + Enum.GetName(typeof(Currency), Currency));
            }

            // TODO modifier for currency that makes convention from one currency to another and change Value field
        }

    }
}