using SQLite;
using System.Collections.Generic;
using System.Linq;
using TradiesToolbox.Models;

namespace TradiesToolbox.Data
{
    public class ClientDatabase
    {
        private readonly SQLiteConnection _database;

        public ClientDatabase()
        {
            _database = DBConnection.GetConnection();
            _database.CreateTable<Client>();
        }

        public List<Client> GetClients() => _database.Table<Client>().ToList();

        public Client GetClient(int id) => _database.Find<Client>(id);

        public int AddClient(Client client) => _database.Insert(client);

        public int UpdateClient(Client client) => _database.Update(client);

        public int DeleteClient(int id) => _database.Delete<Client>(id);

    }
}