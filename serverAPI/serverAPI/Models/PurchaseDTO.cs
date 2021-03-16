
namespace serverAPI.Models
{
    public class PurchaseDTO
    {
        public uint Id { get; set; }
        public uint Count { get; set; }
        public double Cost { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string PaymentMethod { get; set; }

    }
}
