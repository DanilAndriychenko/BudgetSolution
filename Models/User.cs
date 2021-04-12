using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Models
{
    public class User
    {
        public static User CurrUser { get; set; }
        public Guid Guid { get; }
        private string _firstName;
        private string _lastName;
        public string EmailAddress { get; }
        public string Login { get;}
        public string FirstName
        {
            get => _firstName;
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _firstName = value;
                }
                else
                {
                    throw new Exception("User name should not be empty.");
                }
            }
        }
        public string LastName
        {
            get => _lastName;
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _lastName = value;
                }
                else
                {
                    throw new Exception("User surname should not be empty.");
                }
            }
        }

        public List<Wallet> Wallets { get; }
        public List<Category> Categories { get; }

        public User(Guid guid, string name, string surname, string emailAddress, string login)
        {
            Guid = guid;
            FirstName = name;
            LastName = surname;
            if (IsValidEmail(emailAddress))
                EmailAddress = emailAddress;
            else
                throw new ArgumentException("Invalid email" + emailAddress);

            Wallets = new List<Wallet>();
            Categories = new List<Category>();
            Login = login;
        }
        public User(Guid guid, string name, string surname, string emailAddress, string login, List<Wallet> wallets)
        {
            Guid = guid;
            FirstName = name;
            LastName = surname;
            if (IsValidEmail(emailAddress))
                EmailAddress = emailAddress;
            else
                throw new ArgumentException("Invalid email" + emailAddress);

            Wallets = new List<Wallet>();
            Categories = new List<Category>();
            Login = login;
            Wallets = wallets;
        }
        public bool IsValidEmail(string source)
        {
            return new EmailAddressAttribute().IsValid(source);
        }

        /*public void AddWallet(string name, List<Category> categories, decimal balance = 0,
            Currency currency = Currency.Uah, string description = "")
        {
            _wallets.Add(new Wallet(name, this, categories, balance, currency, description));
        }*/
        
        /*public void AddWallet(string name, List<Category> categories, string description, decimal balance = 0,
            Currency currency = Currency.Uah)
        {
            _wallets.Add(new Wallet(name, this, categories, balance, currency, description));
        }*/

        public void AddCategory(string name, Color color = Color.Gray,
            Icon icon = Icon.Uncategorized, string description = "")
        {
            Categories.Add(new Category(name, color, icon, description));
        }

        public Wallet GetWalletByName(string walletName)
        {
            return Wallets.FirstOrDefault(wallet => wallet.Name == walletName);
        }
    }
}