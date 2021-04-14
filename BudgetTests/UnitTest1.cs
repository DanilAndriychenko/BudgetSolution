using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BudgetTests
{
    [TestClass]
    public class UnitTest1
    {
        /*[TestMethod]
        public void TransactionDisplay()
        {
            var sasha = new User(Guid.NewGuid(), "Sasha", "Shlyakhova", "email@gmail.com", "sun-ok");
            var categories = new List<Category> {new("Food")};
            var wallet = new Wallet("Monobank", sasha, categories, Guid.NewGuid());
            wallet.AddTransaction(-1, wallet.Categories[0], DateTime.Now);
            wallet.AddTransaction(-1, Currency.Eur, wallet.Categories[0], DateTime.Now);
            wallet.AddTransaction(-1, Currency.Usd, wallet.Categories[0], DateTime.Now);
            wallet.AddTransaction(-1, Currency.Eur, wallet.Categories[0], new DateTime(2001, 11, 30));
            wallet.AddTransaction(-1, Currency.Usd, wallet.Categories[0], new DateTime(2000, 11, 30));
            wallet.AddTransaction(-1, Currency.Eur, wallet.Categories[0], new DateTime(2011, 11, 30));
            wallet.AddTransaction(-1, Currency.Usd, wallet.Categories[0], new DateTime(2011, 11, 30));
            wallet.AddTransaction(-1, Currency.Eur, wallet.Categories[0], new DateTime(2012, 11, 30));
            wallet.AddTransaction(-1, Currency.Usd, wallet.Categories[0], new DateTime(2001, 11, 30));
            wallet.AddTransaction(-1, Currency.Usd, wallet.Categories[0], new DateTime(2011, 12, 30));
            wallet.AddTransaction(-1, Currency.Uah, wallet.Categories[0], new DateTime(2001, 11, 30));
            Assert.AreEqual(wallet.GetTransactions()[9].Date, new DateTime(2001, 11, 30));
        }
        [TestMethod]
        public void TransactionEditing()
        {
            var sasha = new User(Guid.NewGuid(), "Sasha", "Shlyakhova", "email@gmail.com", "sun-ok");
            var categories = new List<Category> { new Category("Food") };
            var wallet = new Wallet("Monobank", sasha, categories, Guid.NewGuid());
            wallet.AddTransaction(15.99m, wallet.Categories[0], DateTime.Now);
            wallet.GetTransactions()[0].Value = 16;
            Assert.AreEqual(wallet.GetTransactions()[0].Value, 16);
        }
        [TestMethod]
        public void WalletOwner()
        {
            var sasha = new User(Guid.NewGuid(), "Sasha", "Shlyakhova", "email@gmail.com", "sun-ok");
            var categories = new List<Category> { new Category("Food") };
            var wallet = new Wallet("Monobank", sasha, categories, Guid.NewGuid());
            wallet.AddTransaction(15.99m, wallet.Categories[0], DateTime.Now);
            Assert.AreEqual(wallet.Owner, sasha);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException),
        "A userId of null was inappropriately allowed.")]
        public void InvalidEmail()
        {
            var unused = new User(Guid.NewGuid(), "Sasha", "Shlyakhova", "email.com", "sun-ok");
        }
        
        [TestMethod]
        public void WalletSharing()
        {
            var sasha = new User(Guid.NewGuid(), "Sasha", "Shlyakhova", "email@gmail.com", "sun-ok");
            var danil = new User(Guid.NewGuid(), "Danil", "Andriychenko", "email2@gmail.com", "sun-ok");
            var categories = new List<Category> { new Category("Food") };
            var wallet = new Wallet("Monobank", sasha, categories, Guid.NewGuid());
            wallet.AddTransaction(15.99m, wallet.Categories[0], DateTime.Now);
            wallet.AddContributor(danil);
            Assert.AreEqual(wallet.Contributors.Count, 1);
        }
        [TestMethod]
        public void WalletSharingToOwner()
        {
            var sasha = new User(Guid.NewGuid(), "Sasha", "Shlyakhova", "email@gmail.com", "sun-ok");
            var categories = new List<Category> { new Category("Food") };
            var wallet = new Wallet("Monobank", sasha, categories, Guid.NewGuid());
            wallet.AddTransaction(15.99m, wallet.Categories[0], DateTime.Now);
            wallet.AddContributor(sasha);
            Assert.AreEqual(wallet.Contributors.Count, 0);
        }
        [TestMethod]
        public void WalletSharingToNone()
        {
            var sasha = new User(Guid.NewGuid(), "Sasha", "Shlyakhova", "email@gmail.com", "sun-ok");
            var categories = new List<Category> { new Category("Food") };
            var wallet = new Wallet("Monobank", sasha, categories, Guid.NewGuid());
            wallet.AddTransaction(15.99m, wallet.Categories[0], DateTime.Now);
            Assert.AreEqual(wallet.Contributors.Count, 0);
        }

        [TestMethod]
        public void CurrencyConverterTestUah()
        {
            var sasha = new User(Guid.NewGuid(), "Sasha", "Shlyakhova", "email@gmail.com", "sun-ok");
            var categories = new List<Category> { new Category("Food") };
            var wallet = new Wallet("Monobank", sasha, categories, Guid.NewGuid());
            wallet.AddTransaction(15.99m, wallet.Categories[0], DateTime.Now);
            Assert.AreEqual(wallet.GetTransactions().Count, 1);
        }

        [TestMethod]
        public void BalanceTest()
        {
            var sasha = new User(Guid.NewGuid(), "Sasha", "Shlyakhova", "email@gmail.com", "sun-ok");
            var categories = new List<Category> { new Category("Food") };
            var wallet = new Wallet("Monobank", sasha, categories, Guid.NewGuid());
            for (int i = 0; i < 15; i++)
            {
                wallet.AddTransaction(1, wallet.Categories[0], DateTime.Now);
            }
            Assert.AreEqual(wallet.Balance, 15);
        }

        [TestMethod]
        public void BalanceDifferentCurrenciesTest()
        {
            var sasha = new User(Guid.NewGuid(), "Sasha", "Shlyakhova", "email@gmail.com", "sun-ok");
            var categories = new List<Category> { new Category("Food") };
            var wallet = new Wallet("Monobank", sasha, categories, Guid.NewGuid());
            wallet.AddTransaction(1, wallet.Categories[0], DateTime.Now);
            wallet.AddTransaction(1, Currency.Eur, wallet.Categories[0], DateTime.Now);
            wallet.AddTransaction(1, Currency.Usd, wallet.Categories[0], DateTime.Now);
            Assert.AreEqual(wallet.Balance, 62);
        }

        [TestMethod]
        public void IncomeTest()
        {
            var sasha = new User(Guid.NewGuid(), "Sasha", "Shlyakhova", "email@gmail.com", "sun-ok");
            var categories = new List<Category> { new Category("Food") };
            var wallet = new Wallet("Monobank", sasha, categories, Guid.NewGuid());
            wallet.AddTransaction(1, wallet.Categories[0], DateTime.Now);
            wallet.AddTransaction(1, Currency.Eur, wallet.Categories[0], DateTime.Now);
            wallet.AddTransaction(1, Currency.Usd, wallet.Categories[0], DateTime.Now);
            wallet.AddTransaction(1, Currency.Usd, wallet.Categories[0], new DateTime(2001, 11, 30));
            Assert.AreEqual(wallet.GetIncome(), 62);
        }

        [TestMethod]
        public void IncomeZeroTest()
        {
            var sasha = new User(Guid.NewGuid(), "Sasha", "Shlyakhova", "email@gmail.com", "sun-ok");
            var categories = new List<Category> { new Category("Food") };
            var wallet = new Wallet("Monobank", sasha, categories, Guid.NewGuid());
            wallet.AddTransaction(1, wallet.Categories[0], new DateTime(2001, 11, 30));
            wallet.AddTransaction(1, Currency.Eur, wallet.Categories[0], new DateTime(2001, 11, 30));
            wallet.AddTransaction(1, Currency.Usd, wallet.Categories[0], new DateTime(2001, 11, 30));
            wallet.AddTransaction(1, Currency.Usd, wallet.Categories[0], new DateTime(2001, 11, 30));
            Assert.AreEqual(wallet.GetIncome(), 0);
        }

        [TestMethod]
        public void SpendingTest()
        {
            var sasha = new User(Guid.NewGuid(), "Sasha", "Shlyakhova", "email@gmail.com", "sun-ok");
            var categories = new List<Category>();
            categories.Add(new Category("Food"));
            var wallet = new Wallet("Monobank", sasha, categories, Guid.NewGuid());
            wallet.AddTransaction(-1, wallet.Categories[0], DateTime.Now);
            wallet.AddTransaction(-1, Currency.Eur, wallet.Categories[0], DateTime.Now);
            wallet.AddTransaction(-1, Currency.Usd, wallet.Categories[0], DateTime.Now);
            wallet.AddTransaction(-1, Currency.Usd, wallet.Categories[0], new DateTime(2001, 11, 30));
            Assert.AreEqual(wallet.GetSpending(), 62);
        }

        [TestMethod]
        public void SpendingZeroTest()
        {
            var sasha = new User(Guid.NewGuid(), "Sasha", "Shlyakhova", "email@gmail.com", "sun-ok");
            var categories = new List<Category> { new Category("Food") };
            var wallet = new Wallet("Monobank", sasha, categories, Guid.NewGuid());
            wallet.AddTransaction(-1, wallet.Categories[0], new DateTime(2001, 11, 30));
            wallet.AddTransaction(-1, Currency.Eur, wallet.Categories[0], new DateTime(2001, 11, 30));
            wallet.AddTransaction(-1, Currency.Usd, wallet.Categories[0], new DateTime(2001, 11, 30));
            wallet.AddTransaction(-1, Currency.Usd, wallet.Categories[0], new DateTime(2001, 11, 30));
            Assert.AreEqual(wallet.GetSpending(), 0);
        }*/
    }
}