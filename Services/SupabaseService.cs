using Supabase;
using SupabaseClient = Supabase.Client;
using Supabase.Gotrue;
using Supabase.Gotrue.Responses;
using Microsoft.Maui.Storage;

namespace TradiesToolbox.Services
{
    public class SupabaseService
    {
        private readonly SupabaseClient _supabaseClient;

        private const string SUPABASE_URL = "https://ycukgsrkxiomqabxftys.supabase.co";
        private const string SUPABASE_KEY = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InljdWtnc3JreGlvbXFhYnhmdHlzIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDIwMDY0MzAsImV4cCI6MjA1NzU4MjQzMH0.dyvZAmZ0hy8chLJZBq31Brady_W8Ei6cONenMaM0ixI";

        public SupabaseService()
        {
            var options = new SupabaseOptions
            {
                AutoRefreshToken = true,
                AutoConnectRealtime = true
            };

            _supabaseClient = new SupabaseClient(SUPABASE_URL, SUPABASE_KEY, options);
        }

        public async Task<Session> SignUpAsync(string email, string password)
        {
            var response = await _supabaseClient.Auth.SignUp(email, password);

            // Store user email in preferences after successful signup
            if (response?.User?.Email != null)
            {
                Preferences.Default.Set("UserEmail", response.User.Email);
            }

            return response;
        }

        public async Task<Session> SignInAsync(string email, string password)
        {
            var response = await _supabaseClient.Auth.SignIn(email, password);

            // Store user email in preferences after successful login
            if (response?.User?.Email != null)
            {
                Preferences.Default.Set("UserEmail", response.User.Email);
            }

            return response;
        }

        public async Task SignOutAsync()
        {
            // Clear stored user email on logout
            Preferences.Default.Remove("UserEmail");
            await _supabaseClient.Auth.SignOut();
        }

        public bool IsAuthenticated()
        {
            return _supabaseClient.Auth.CurrentSession != null;
        }

        public User? GetCurrentUser()
        {
            return _supabaseClient.Auth.CurrentUser;
        }

        public string? GetCurrentUserEmail()
        {
            return _supabaseClient.Auth.CurrentUser?.Email ??
                   Preferences.Default.Get<string>("UserEmail", null);
        }
    }
}