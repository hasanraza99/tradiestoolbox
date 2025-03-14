using System.Linq;
using SQLite;
using TradiesToolbox.Models;
using TradiesToolbox.Data;
using Microsoft.Maui.Storage;
using BCrypt.Net;

namespace TradiesToolbox.Services
{
    public static class AuthService
    {
        private static readonly SQLiteConnection _database = DBConnection.GetConnection();

        static AuthService()
        {
            _database.CreateTable<User>(); // Ensure the Users table exists
            Console.WriteLine("User table initialized");
        }

        // register new user
        public static bool Register(string email, string password)
        {
            var existingUser = _database.Table<User>().FirstOrDefault(u => u.Email == email);
            if (existingUser != null)
            {
                return false; // User already exists
            }

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            var newUser = new User { Email = email, PasswordHash = hashedPassword };
            _database.Insert(newUser);
            return true;
        }

        public static bool Login(string email, string password)
        {
            if (_database == null)
            {
                Console.WriteLine("ERROR: Database connection is NULL in Login!");
                return false;
            }

            var user = _database.Table<User>().FirstOrDefault(u => u.Email == email);
            if (user != null && !string.IsNullOrEmpty(user.PasswordHash) && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                Preferences.Set("IsLoggedIn", true);
                Preferences.Set("UserEmail", email);
                return true;
            }

            Console.WriteLine("Login failed: Invalid email or password.");
            return false;
        }


        // check if user is logged in
        public static bool IsLoggedIn()
        {
            return Preferences.Get("IsLoggedIn", false);
        }

        // logout
        public static void Logout()
        {
            Preferences.Remove("IsLoggedIn");
            Preferences.Remove("UserEmail");
        }
    }
}
