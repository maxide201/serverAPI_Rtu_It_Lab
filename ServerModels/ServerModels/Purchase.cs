using System;
using System.ComponentModel.DataAnnotations;

namespace ServerModels
{
    public class Purchase
    {
        [Key]
        public uint Id { get; set; }
        public uint CheckId { get; set; }
        public uint Count { get; set; }
        public double Cost { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string PaymentMethod { get; set; }

    }
}
