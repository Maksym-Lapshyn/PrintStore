using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PrintStore.Domain.Entities;

namespace PrintStore.Models
{
    /// <summary>
    /// View model of cartline
    /// </summary>
    public class CartLineViewModel
    {
        public int CartLineId { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get { return Product.Price * Quantity; } }
    }
}