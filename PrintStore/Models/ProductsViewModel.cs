using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PrintStore.Domain.Entities;
using PrintStore.Domain.Infrastructure.Abstract;

namespace PrintStore.Models
{
    /// <summary>
    /// View model for filtered products
    /// </summary>
    public class ProductsViewModel
    {
        public FilterViewModel Filter { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}