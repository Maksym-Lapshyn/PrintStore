﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrintStore.Domain.Infrastructure.Concrete;

namespace PrintStore.Domain.Entities
{
    public class CartLine
    {
        public int CartLineId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public decimal ComputeTotalPrice()
        {
            EFBusinessLogicLayer layer = new EFBusinessLogicLayer();
            decimal totalPrice = layer.Products.Where(p => p.ProductId == ProductId).First().Price * Quantity;
            return totalPrice;
        }
    }
}
