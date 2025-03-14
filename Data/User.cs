using SQLite;

namespace TradiesToolbox.Models
{
    [Table("Users")]
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Email { get; set; }
        public string PasswordHash { get; set; } // Hashed password for security
    }
}
