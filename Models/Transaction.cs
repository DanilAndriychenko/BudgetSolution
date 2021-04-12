﻿using System;
using System.Collections.Generic;
using System.IO;

namespace Models
{
    public class Transaction
    {
        public decimal Value { get; set; }
        public Currency Currency { get; }
        public Category Category { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; private set; }
        public List<FileInfo> Files { get; set; }

        public Transaction(decimal value, Currency currency, Category category, DateTime date,
            string description = "", List<FileInfo> files = null)
        {
            Value = value;
            Currency = currency;
            Category = category ?? throw new ArgumentNullException(nameof(category));
            Description = description ?? throw new ArgumentNullException(nameof(description));
            Date = date;
            Files = files ?? new List<FileInfo>();
        }

        public static decimal GetConvertedPrice(decimal price, Currency currencyOfPrice, Currency currencyToConvert)
        {
            return currencyOfPrice == currencyToConvert
                ? price
                : Math.Round((price * (int) currencyOfPrice) / (decimal) currencyToConvert, 2);
        }

        public decimal GetValue(Currency currency)
        {
            return GetConvertedPrice(Value, Currency, currency);
        }

        public override string ToString()
        {
            return Value + " " + Enum.GetName(typeof(Currency), Currency);
        }
    }
}