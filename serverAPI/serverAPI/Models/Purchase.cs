using System;
using System.ComponentModel.DataAnnotations;

namespace serverAPI.Models
{
    public class Purchase
    {
        [Key]
        public uint Id { get; set; }
        public uint UserId { get; set; }
        public string Name { get; set; }
        public DateTime PurchaseDate { get; set; }
        public uint Cost { get; set; }
    }
}
