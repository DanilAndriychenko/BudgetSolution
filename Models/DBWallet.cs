using System;
using System.Collections.Generic;
using DataStorage;

namespace Models
{
    public class DbWallet: IStorable
    {
        public string Name { get; }
        public string Description { get; }
        public decimal Balance { get; }
        public Currency Currency { get; }
        public List<Guid> Transactions { get; }
        // public List<Category> Categories { get; }
        public Guid Owner { get; }
        public Guid Guid { get; }

        public DbWallet(string name, string description, decimal balance, Currency currency, List<Guid> transactions/*, List<Category> categories*/, Guid owner, Guid guid)
        {
            Name = name;
            Description = description;
            Balance = balance;
            Currency = currency;
            Transactions = transactions;
            // Categories = categories;
            Owner = owner;
            Guid = guid;
        }
    }
}