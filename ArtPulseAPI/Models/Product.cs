using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtPulseAPI.Models
{
    internal class Product
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public int RatingScaledBy10 { get; set; }
        public Category Category { get; set; }
        public decimal Cost { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }
        public Seller Seller { get; set; }
        public ICollection<Customer> CustomersWithThisProductInShoppingCart { get; set; }
    }

    public enum Category : ushort
    {
        Category1 = 0,
        Category2 = 1,
        Category3 = 2,
        Category4 = 3,
        Category5 = 4,
    }
}
