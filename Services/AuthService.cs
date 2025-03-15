//using System;
//using System.Linq;
//using System.Security.Cryptography;
//using System.Text;
//using SQLite;
//using TradiesToolbox.Models;
//using TradiesToolbox.Data;
//using Microsoft.Maui.Storage;

//namespace TradiesToolbox.Services
//{
//    public static class AuthService
//    {
//        // Hard-coded credentials for simplicity
//        private static readonly string DefaultEmail = "jamie@gmail.com";
//        private static readonly string DefaultPassword = "password";

//        // Simple login check
//        public static bool Login(string email, string password)
//        {
//            // Basic validation
//            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
//                return false;

//            // Simple credential check
//            bool isValid = (email == DefaultEmail && password == DefaultPassword);

//            if (isValid)
//            {
//                try
//                {
//                    // Store login state
//                    Preferences.Default.Set("IsLoggedIn", true);
//                    Console.WriteLine("Login successful");
//                }
//                catch (Exception ex)
//                {
//                    Console.WriteLine($"Preference error: {ex.Message}");
//                }
//            }

//            return isValid;
//        }

//        // Simple check for login status
//        public static bool IsLoggedIn()
//        {
//            try
//            {
//                return Preferences.Default.Get("IsLoggedIn", false);
//            }
//            catch
//            {
//                return false;
//            }
//        }

//        // Simple logout
//        public static void Logout()
//        {
//            try
//            {
//                Preferences.Default.Set("IsLoggedIn", false);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Logout error: {ex.Message}");
//            }
//        }
//    }
//}