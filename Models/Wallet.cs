using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DataStorage;

namespace Models
{
    public enum Currency
    {
        Uah = 1,
        Usd = 28,
        Eur = 33
    }

    public class Wallet
    {
        public static Wallet CurrWallet { get; set; }
        private const int NumOfTransactionAtOnce = 10;
        private string _name;

        public string Name
        {
            get => _name;
            set
            {
                if (!string.IsNullOrEmpty(value))
                    _name = value;
                else
                    throw new Exception("Wallet name should not be empty.");
            }
        }
        public decimal Balance { 
            get
            {
                decimal balance = 0;
                foreach (var transaction in Transactions)
                    balance += Transaction.GetConvertedPrice(transaction.Value, transaction.Currency, Currency);
                return balance;
            }
        }
        public string Description { get; set; }
        private Currency _currancy;
        public Currency Currency
        {
            get
            {
                return _currancy;
            }
            set
            {
                _currancy = value;
            }
        }

        public SortedSet<Transaction> Transactions { get; }

        // public List<Category> Categories { get; }
        public User Owner { get; }
        public List<User> Contributors { get; }
        public Guid Guid { get; }

        public Wallet(string name, User owner /*, List<Category> categories*/, Guid guid,
            SortedSet<Transaction> transactions, decimal balance = 0,
            Currency currency = Currency.Uah, string description = "")
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            // Categories = categories ?? throw new ArgumentNullException(nameof(categories));
            Guid = guid;
            _currancy = currency;
            Description = description ?? throw new ArgumentNullException(nameof(description));
            Transactions = transactions;
            Owner = owner;
            Contributors = new List<User>();
        }

        public void AddTransaction(Transaction transaction)
        {
            Transactions.Add(transaction);
            
        }

        public bool RemoveTransaction(Transaction transactionToBeRemoved)
        {
            if (!Transactions.Contains(transactionToBeRemoved))
                return false;
            Transactions.Remove(transactionToBeRemoved);
            return true;
        }

        // Returns NumOfTransactionAtOnce from start if there is no transaction at start returns null
        public List<Transaction> GetTransactionsList(int start = 0)
        {
            if (Transactions.Count < start)
            {
                return new List<Transaction>();
            }

            var counter = 0;
            var result = new List<Transaction>();
            foreach (var transaction in Transactions)
            {
                counter++;
                if (counter == NumOfTransactionAtOnce + start)
                {
                    break;
                }
                if(counter >= start)
                    result.Add(transaction);
            }

            return result;
        }

        public decimal GetSpending()
        {
            return Transactions.TakeWhile(transaction => transaction.Date.Month == DateTime.Now.Month)
                .Where(transaction => transaction.Value < 0)
                .Aggregate<Transaction, decimal>(0, (current, transaction) => current - transaction.GetValue(Currency));
        }

        public decimal GetIncome()
        {
            return Transactions.TakeWhile(transaction => transaction.Date.Month == DateTime.Now.Month)
                .Where(transaction => transaction.Value > 0)
                .Aggregate<Transaction, decimal>(0, (current, transaction) => current + transaction.GetValue(Currency));
        }

        public void AddContributor(User user)
        {
            if (user == Owner)
                return;
            if (!Contributors.Contains(user))
                Contributors.Add(user);
        }
    }
}