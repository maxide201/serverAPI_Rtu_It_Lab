using System;
using System.ComponentModel.DataAnnotations;

namespace serverAPI.Models
{
    public class Purchase
    {
        [Key]
        public uint Id { get; set; }
        public uint CheckId { get; set; }
        public double Cost { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime PurchaseDate { get; set; }

    }
}
