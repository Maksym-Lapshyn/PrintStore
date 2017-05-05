using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintStore.Domain.Entities
{
    public class Cart
    {
        public List<CartLine> CartLines { get; set; }
        public decimal TotalPrice { get { return CartLines.Sum(c => c.Product.Price * c.Quantity); } }
        public int Quantity { get { return CartLines.Count; } }
        public Cart()
        {
            CartLines = new List<CartLine>();
        }

        public void AddCartLine(Product product, int quantity)
        {
            CartLine cartLine = CartLines.Where(c => c.Product.ProductId == product.ProductId).FirstOrDefault();
            if (cartLine == null)
            {
                CartLine newCartLine = new CartLine() { Product = product, Quantity = quantity };
                CartLines.Add(newCartLine);
            }
            else
            {
                cartLine.Quantity += quantity;
            }
        }

        public void RemoveCartLine(Product product)
        {
            CartLines.RemoveAll(c => c.Product.ProductId == product.ProductId);
        }

        public void Clear()
        {
            CartLines.Clear();
        }
    }
}
