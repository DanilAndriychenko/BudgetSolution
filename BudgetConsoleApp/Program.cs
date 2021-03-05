using System;
using System.Collections.Generic;

namespace BudgetConsoleApp
{
    internal static class Program
    {
        // TODO add id and freeId to every class
        private static void Main(string[] args)
        {
            User sasha = new User("Sasha", "Shliakhova", "oleksandra.shliakhova@ukma.edu.ua");
            sasha.AddCategory("Food", Color.Orange, Icon.Food, "Except for cafe and restaurants");
            sasha.AddCategory("Transport", Color.Blue, Icon.Transport);
            sasha.AddCategory("Clothes", Color.Yellow, Icon.Clothes);
            sasha.AddWallet("Monobank", sasha.Categories);
            //sasha.CommitTransaction(sasha.GetWalletByName("Monobank"));

            /*User sasha = new User("Sasha", "Shlyakhova", "oleksandra.shliakhova@ukma.edu.ua");*/
            sasha.AddCategory("Food", Color.Blue, Icon.Food);
            sasha.AddCategory("Clothes", Color.Pink, Icon.Clothes);
            sasha.AddWallet("Monobank", sasha.Categories, "Main balance");


            /*User sasha = new User("Sasha", "Shlyakhova", "email@gmail.com");*/
            var categories = new List<Category> {new Category("Food")};
            var wallet = new Wallet("Monobank", sasha, categories);

            wallet.AddTransaction(10.99m, wallet.Categories[0], DateTime.Now);
            wallet.AddTransaction(11.99m, wallet.Categories[0], DateTime.Now);
            wallet.AddTransaction(12.99m, wallet.Categories[0], DateTime.Now);
            wallet.AddTransaction(13.99m, wallet.Categories[0], DateTime.Now);
            wallet.AddTransaction(14.99m, wallet.Categories[0], DateTime.Now);
            wallet.AddTransaction(15.99m, wallet.Categories[0], DateTime.Now);
            wallet.AddTransaction(16.99m, wallet.Categories[0], DateTime.Now);
            wallet.AddTransaction(17.99m, wallet.Categories[0], DateTime.Now);
            wallet.AddTransaction(18.99m, wallet.Categories[0], DateTime.Now);
            wallet.AddTransaction(19.99m, wallet.Categories[0], DateTime.Now);
            wallet.AddTransaction(20.99m, wallet.Categories[0], DateTime.Now);

            var lastTransactions = wallet.GetTransactions();
            /*foreach (var transaction in lastTransactions)
                Console.WriteLine(transaction);

            Console.WriteLine('\n');

            lastTransactions = wallet.GetTransactions(5);
            foreach (var transaction in lastTransactions)
                Console.WriteLine(transaction);
            Console.WriteLine('\n');
            lastTransactions = wallet.GetTransactions(10);
            foreach (var transaction in lastTransactions)
                Console.WriteLine(transaction);*/


            //sasha.Wallets[0].AddTransaction(15.99, new Category("Flowers"), DateTime.Now, "Flowers for holiday");
        }
    }
}