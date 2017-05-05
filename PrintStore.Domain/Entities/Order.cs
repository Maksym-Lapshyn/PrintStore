using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintStore.Domain.Entities
{
    public class Order
    {
        public int OrderId { get; set; }
        public string UserId { get; set; }
        public virtual ICollection<CartLine> CartLines { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime DateAdded { get; set; }
        public bool IsDeleted { get; set; }

        public Order()
        {
            CartLines = new List<CartLine>();
        }
    }

    public enum OrderStatus
    {
        Registered,
        Processed
    }
}
