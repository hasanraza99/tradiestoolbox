using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using SQLite;
using TradiesToolbox.Models;
using TradiesToolbox.Data;
using Microsoft.Maui.Storage;

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

        // Simple hash function using SHA256
        private static string HashPassword(string password)
        {
            try
            {
                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                    StringBuilder builder = new StringBuilder();
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        builder.Append(bytes[i].ToString("x2"));
                    }
                    return builder.ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error hashing password: {ex.Message}");
                return string.Empty;
            }
        }

        // register new user
        public static bool Register(string email, string password)
        {
            try
            {
                var existingUser = _database.Table<User>().FirstOrDefault(u => u.Email == email);
                if (existingUser != null)
                {
                    return false; // User already exists
                }

                string hashedPassword = HashPassword(password);
                if (string.IsNullOrEmpty(hashedPassword))
                {
                    return false; // Hashing failed
                }

                var newUser = new User { Email = email, PasswordHash = hashedPassword };
                _database.Insert(newUser);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Register: {ex.Message}");
                return false;
            }
        }

        public static bool Login(string email, string password)
        {
            try
            {
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                    return false;
                }

                var user = _database.Table<User>().FirstOrDefault(u => u.Email == email);
                if (user == null)
                {
                    Console.WriteLine("User not found");
                    return false;
                }

                string hashedPassword = HashPassword(password);
                if (string.IsNullOrEmpty(hashedPassword))
                {
                    return false; // Hashing failed
                }

                if (user.PasswordHash == hashedPassword)
                {
                    Preferences.Set("IsLoggedIn", true);
                    Preferences.Set("UserEmail", email);
                    return true;
                }

                Console.WriteLine("Login failed: Invalid password");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Login: {ex.Message}");
                return false;
            }
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