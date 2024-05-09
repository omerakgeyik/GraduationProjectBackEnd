using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtPulseAPI.Models
{
    internal class Offer
    {
        public int Id { get; set; }
        public string OfferName { get; set; }
        public string OfferDescription { get; set; }
        public ICollection<Product> OfferedProducts { get; set; }
    }
}
