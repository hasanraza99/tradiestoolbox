using SQLite;

namespace TradiesToolbox.Models
{
    [Table("QuoteItems")]
    public class QuoteItem
    {
        [PrimaryKey, AutoIncrement]
        public int ItemID { get; set; }
        public int QuoteID { get; set; } // Foreign Key to Quotes
        public string Description { get; set; }
        public decimal Quantity { get; set; } = 1;
        public string Unit { get; set; } = "Item";
        public decimal UnitPrice { get; set; }
        public decimal Total => Quantity * UnitPrice;
    }
}
