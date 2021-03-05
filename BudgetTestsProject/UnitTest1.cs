using System;
using System.Collections.Generic;
using BudgetConsoleApp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace BudgetTestsProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void WalletSharing()
        {
            var sasha = new User("Sasha", "Shlyakhova", "email@gmail.com");
            var danil = new User("Danil", "Andriychenko", "email2@gmail.com");
            var categories = new List<Category> {new Category("Food")};
            var wallet = new Wallet("Monobank", sasha, categories);
            wallet.AddTransaction(15.99m, wallet.Categories[0], DateTime.Now);
            Assert.AreEqual(wallet.GetTransactions().Count, 1);
        }

        [TestMethod]
        public void CurrencyConverterTestUah()
        {
            var sasha = new User("Sasha", "Shlyakhova", "email@gmail.com");
            var categories = new List<Category> {new Category("Food")};
            var wallet = new Wallet("Monobank", sasha, categories);
            wallet.AddTransaction(15.99m, wallet.Categories[0], DateTime.Now);
            Assert.AreEqual(wallet.GetTransactions().Count, 1);
        }

        [TestMethod]
        public void BalanceTest()
        {
            var sasha = new User("Sasha", "Shlyakhova", "email@gmail.com");
            var categories = new List<Category> {new Category("Food")};
            var wallet = new Wallet("Monobank", sasha, categories);
            for (int i = 0; i < 15; i++)
            {
                wallet.AddTransaction(1, wallet.Categories[0], DateTime.Now);
            }
            Assert.AreEqual(wallet.Balance, 15.00);
        }

        [TestMethod]
        public void BalanceDifferentCurrenciesTest()
        {
            var sasha = new User("Sasha", "Shlyakhova", "email@gmail.com");
            var categories = new List<Category> {new Category("Food")};
            var wallet = new Wallet("Monobank", sasha, categories);
            wallet.AddTransaction(1, wallet.Categories[0], DateTime.Now);
            wallet.AddTransaction(1, Currency.Eur, wallet.Categories[0], DateTime.Now);
            wallet.AddTransaction(1, Currency.Usd, wallet.Categories[0], DateTime.Now);
            Assert.AreEqual(wallet.Balance, 62);
        }

        [TestMethod]
        public void IncomeTest()
        {
            var sasha = new User("Sasha", "Shlyakhova", "email@gmail.com");
            var categories = new List<Category> {new Category("Food")};
            var wallet = new Wallet("Monobank", sasha, categories);
            wallet.AddTransaction(1, wallet.Categories[0], DateTime.Now);
            wallet.AddTransaction(1, Currency.Eur, wallet.Categories[0], DateTime.Now);
            wallet.AddTransaction(1, Currency.Usd, wallet.Categories[0], DateTime.Now);
            wallet.AddTransaction(1, Currency.Usd, wallet.Categories[0], new DateTime(2001, 11, 30));
            Assert.AreEqual(wallet.GetIncome(), 62);
        }

        [TestMethod]
        public void IncomeZeroTest()
        {
            var sasha = new User("Sasha", "Shlyakhova", "email@gmail.com");
            var categories = new List<Category> {new Category("Food")};
            var wallet = new Wallet("Monobank", sasha, categories);
            wallet.AddTransaction(1, wallet.Categories[0], new DateTime(2001, 11, 30));
            wallet.AddTransaction(1, Currency.Eur, wallet.Categories[0], new DateTime(2001, 11, 30));
            wallet.AddTransaction(1, Currency.Usd, wallet.Categories[0], new DateTime(2001, 11, 30));
            wallet.AddTransaction(1, Currency.Usd, wallet.Categories[0], new DateTime(2001, 11, 30));
            Assert.AreEqual(wallet.GetIncome(), 0);
        }

        [TestMethod]
        public void SpendingTest()
        {
            var sasha = new User("Sasha", "Shlyakhova", "email@gmail.com");
            var categories = new List<Category>();
            categories.Add(new Category("Food"));
            var wallet = new Wallet("Monobank", sasha, categories);
            wallet.AddTransaction(-1, wallet.Categories[0], DateTime.Now);
            wallet.AddTransaction(-1, Currency.Eur, wallet.Categories[0], DateTime.Now);
            wallet.AddTransaction(-1, Currency.Usd, wallet.Categories[0], DateTime.Now);
            wallet.AddTransaction(-1, Currency.Usd, wallet.Categories[0], new DateTime(2001, 11, 30));
            Assert.AreEqual(wallet.GetSpending(), 62);
        }

        [TestMethod]
        public void SpendingZeroTest()
        {
            var sasha = new User("Sasha", "Shlyakhova", "email@gmail.com");
            var categories = new List<Category> {new Category("Food")};
            var wallet = new Wallet("Monobank", sasha, categories);
            wallet.AddTransaction(-1, wallet.Categories[0], new DateTime(2001, 11, 30));
            wallet.AddTransaction(-1, Currency.Eur, wallet.Categories[0], new DateTime(2001, 11, 30));
            wallet.AddTransaction(-1, Currency.Usd, wallet.Categories[0], new DateTime(2001, 11, 30));
            wallet.AddTransaction(-1, Currency.Usd, wallet.Categories[0], new DateTime(2001, 11, 30));
            Assert.AreEqual(wallet.GetSpending(), 0);
        }
    }
}