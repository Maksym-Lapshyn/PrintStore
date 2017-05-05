using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PrintStore.Domain.Entities;

namespace PrintStore.Models
{
    public class ProductsViewModel
    {
        public FilterViewModel Filter { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}