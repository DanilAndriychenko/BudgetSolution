using System;
using System.Collections.Generic;
using System.IO;
using DataStorage;

namespace Models
{
    public class DbTransaction: IStorable
    {
        public decimal Value { get; set; }
        public Currency Currency { get; }
        // public Category Category { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; private set; }
        public List<FileInfo> Files { get; set; }
        public Guid Guid { get; set; }
        
        
        public DbTransaction(decimal value, Currency currency, /*Category category,*/ DateTime date, Guid guid,
            string description = "", List<FileInfo> files = null)
        {
            Guid = guid;
            Value = value;
            Currency = currency;
            // Category = category ?? throw new ArgumentNullException(nameof(category));
            Description = description ?? throw new ArgumentNullException(nameof(description));
            Date = date;
            Files = files ?? new List<FileInfo>();
        }
        /*public DbTransaction(Transaction transaction)
        {
            Guid = transaction.Guid;
            Value = transaction.Value;
            Currency = transaction.Currency;
            // Category = category ?? throw new ArgumentNullException(nameof(category));
            Description = transaction.Description;
            Date = transaction.Date;
            Files = transaction.Files;
        }*/
    }
}