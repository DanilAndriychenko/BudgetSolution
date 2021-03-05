using System;
using System.Collections.Generic;
using System.Linq;

namespace BudgetConsoleApp
{
    public class User
    {
        private string _name;
        private string _surname;
        private string EmailAddress { get; }
        public string Name
        {
            get => _name;
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _name = value;
                }
                else
                {
                    throw new Exception("User name should not be empty.");
                }
            }
        }
        public string Surname
        {
            get => _surname;
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _surname = value;
                }
                else
                {
                    throw new Exception("User surname should not be empty.");
                }
            }
        }

        private readonly List<Wallet> _wallets;
        public List<Category> Categories { get; }

        public User(string name, string surname, string emailAddress)
        {
            Name = name;
            Surname = surname;
            // TODO validate emailAddress
            EmailAddress = emailAddress;

            _wallets = new List<Wallet>();
            Categories = new List<Category>();
        }
        public void AddWallet(string name, List<Category> categories, decimal balance = 0,
            Currency currency = Currency.Uah, string description = "")
        {
            _wallets.Add(new Wallet(name, this, categories, balance, currency, description));
        }
        public void AddWallet(string name, List<Category> categories, string description, decimal balance = 0,
            Currency currency = Currency.Uah)
        {
            _wallets.Add(new Wallet(name, this, categories, balance, currency, description));
        }
        public void AddWallet(Wallet wallet)
        {
            if (wallet.Categories.Any(walletCategory => !Categories.Contains(walletCategory))) return;
            _wallets.Add(wallet);
        }

        public void AddCategory(string name, Color color = Color.Gray,
            Icon icon = Icon.Uncategorized, string description = "")
        {
            Categories.Add(new Category(name, color, icon, description));
        }

        public Wallet GetWalletByName(string walletName)
        {
            return _wallets.FirstOrDefault(wallet => wallet.Name == walletName);
        }
    }
}