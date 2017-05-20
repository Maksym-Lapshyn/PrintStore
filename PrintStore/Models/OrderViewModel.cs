using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PrintStore.Domain.Entities;

namespace PrintStore.Models
{
    /// <summary>
    /// View model for displaying order
    /// </summary>
    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public virtual ICollection<CartLineViewModel> CartLineViewModels { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime DateAdded { get; set; }

        public OrderViewModel()
        {
            CartLineViewModels = new List<CartLineViewModel>();
        }
    }
}