using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScraperCase.Models
{
    public class Product
    {
        public int ID { get; set; }
        public string URL { get; set; }
        public string SKU { get; set; }
        public string ProductName { get; set; }
        public string Availability { get; set; }
        public string Offer { get; set; }
        public string ProductPrice { get; set; }
        public string ProductSalePrice { get; set; }
        public string UserEmail { get; set; }
        public string ProductCode { get; set; }
    }
}
