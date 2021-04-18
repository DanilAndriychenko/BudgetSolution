using System;
using System.Collections.Generic;
using System.IO;

namespace Models
{
    public class Transaction : IComparable<Transaction>
    {
        public decimal Value { get; set; }
        public Currency Currency { get; set; }
        // public Category Category { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public List<FileInfo> Files { get; set; }
        public Guid Guid { get; }

        public Transaction(decimal value, Currency currency, /*Category category,*/ DateTime date, Guid guid,
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

        public static decimal GetConvertedPrice(decimal price, Currency currencyOfPrice, Currency currencyToConvert)
        {
            return currencyOfPrice == currencyToConvert
                ? price
                : Math.Round((price * (int)currencyOfPrice) / (decimal)currencyToConvert, 2);
        }

        public decimal GetValue(Currency currency)
        {
            return GetConvertedPrice(Value, Currency, currency);
        }

        public int CompareTo(Transaction? other)
        {
            if (other.Date > Date)
                return 1;
            if (other.Date < Date)
                return -1;
            if (Transaction.GetConvertedPrice(other.Value, other.Currency, other.Currency) >
                Transaction.GetConvertedPrice(Value, Currency, other.Currency))
                return -1;
            if (Transaction.GetConvertedPrice(other.Value, other.Currency, other.Currency) <
                Transaction.GetConvertedPrice(Value, Currency, other.Currency))
                return 1;
            return 0;
        }

        public override string ToString()
        {
            return Value + " " + Enum.GetName(typeof(Currency), Currency) + "(Date: " + Date + ")";
        }
    }
}