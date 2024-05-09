using ArtPulseAPI.Models;

namespace ArtPulseAPI.DTO
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public int RatingScaledBy10 { get; set; }
        public string Category { get; set; }
        public decimal Cost { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }
        public int SellerId { get; set; }
    }
}
