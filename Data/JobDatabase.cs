using SQLite;
using System.Collections.Generic;
using System.Linq;
using TradiesToolbox.Models;

namespace TradiesToolbox.Data
{
    public class JobDatabase
    {
        private readonly SQLiteConnection _database;

        public JobDatabase()
        {
            _database = DBConnection.GetConnection();
            _database.CreateTable<Job>();
        }

        public List<Job> GetJobs() => _database.Table<Job>().ToList();
        public Job GetJob(int id) => _database.Find<Job>(id);
        public int AddJob(Job job) => _database.Insert(job);
        public int UpdateJob(Job job) => _database.Update(job);
        public int DeleteJob(int id) => _database.Delete<Job>(id);
    }
}