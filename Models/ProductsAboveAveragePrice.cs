using System;
using System.Collections.Generic;

#nullable disable

namespace Capstone7ProductAPI.Models
{
    public partial class ProductsAboveAveragePrice
    {
        public string ProductName { get; set; }
        public decimal? UnitPrice { get; set; }
    }
}
