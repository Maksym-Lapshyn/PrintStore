using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrintStore.Domain.Infrastructure.Concrete;

namespace PrintStore.Domain.Entities
{
    /// <summary>
    /// Basic unit of customer's order - product's id and its quantity
    /// </summary>
    public class CartLine
    {
        public int CartLineId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
