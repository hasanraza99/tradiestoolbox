using SQLite;
using System.Collections.Generic;
using System.Linq;
using TradiesToolbox.Models;

namespace TradiesToolbox.Data
{
    public class QuoteDatabase
    {
        private readonly SQLiteConnection _database;

        public QuoteDatabase()
        {
            _database = DBConnection.GetConnection();
            _database.CreateTable<Quote>();
        }

        public List<Quote> GetQuotes() => _database.Table<Quote>().ToList();
        public Quote GetQuote(int id) => _database.Find<Quote>(id);
        public int AddQuote(Quote quote) => _database.Insert(quote);
        public int UpdateQuote(Quote quote) => _database.Update(quote);
        public int DeleteQuote(int id) => _database.Delete<Quote>(id);
    }
}