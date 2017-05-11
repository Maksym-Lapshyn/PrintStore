﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrintStore.Domain.Concrete;

namespace PrintStore.Domain.Entities
{
    public class CartLine
    {
        public int CartLineId { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }

        public decimal ComputeTotalPrice()
        {
            decimal totalPrice = Quantity * Product.Price;
            return totalPrice;
        }
    }
}
