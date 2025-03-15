using SQLite;
using System;
using System.IO;
using TradiesToolbox.Models;

namespace TradiesToolbox.Data
{
    public static class DBConnection
    {
        private const string DatabaseFilename = "TradiesToolbox.db3";

        private const SQLiteOpenFlags Flags =
            SQLiteOpenFlags.ReadWrite |
            SQLiteOpenFlags.Create |
            SQLiteOpenFlags.SharedCache;

        public static string DatabasePath =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DatabaseFilename);

        private static SQLiteConnection _database;

        public static SQLiteConnection GetConnection()
        {
            if (_database == null)
            {
                Console.WriteLine("Creating database connection");
                _database = new SQLiteConnection(DatabasePath, Flags);

                // Create all necessary tables
                _database.CreateTable<Client>();
                _database.CreateTable<User>();
                _database.CreateTable<Job>();
                _database.CreateTable<Quote>();

                Console.WriteLine("Database tables created successfully");
            }
            return _database;
        }
    }
}