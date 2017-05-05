using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintStore.Domain.Entities
{
    public class CartLine
    {
        public int CartLineId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
