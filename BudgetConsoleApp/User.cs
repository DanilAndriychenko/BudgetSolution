using System;
using System.Collections.Generic;
using System.Linq;

namespace Budget
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

        private List<Wallet> wallets;
        public List<Category> Categories { get; }

        public User(string name, string surname, string emailAddress)
        {
            Name = name;
            Surname = surname;
            // TODO validate emailAddress
            EmailAddress = emailAddress;

            wallets = new List<Wallet>();
            Categories = new List<Category>();
        }

        public void ChangeSurname(string newSurname)
        {
            Surname = newSurname ?? throw new ArgumentNullException(nameof(newSurname));
        }

        public void AddWallet(string name, List<Category> categories, double balance = 0,
            Currency currency = Currency.Uah, string description = "")
        {
            wallets.Add(new Wallet(name, this, categories, balance, currency, description));
        }
        public void AddWallet(string name, List<Category> categories, string description, double balance = 0,
            Currency currency = Currency.Uah)
        {
            wallets.Add(new Wallet(name, this, categories, balance, currency, description));
        }
        public void AddWallet(Wallet wallet)
        {
            foreach (var walletCategory in wallet.Categories)
                if (!Categories.Contains(walletCategory)) return;
            wallets.Add(wallet);
        }

        public void AddCategory(string name, Color color = Color.Gray,
            Icon icon = Icon.Uncategorized, string description = "")
        {
            Categories.Add(new Category(name, color, icon, description));
        }

        public Wallet GetWalletByName(string walletName)
        {
            return wallets.FirstOrDefault(wallet => wallet.Name == walletName);
        }
    }
}