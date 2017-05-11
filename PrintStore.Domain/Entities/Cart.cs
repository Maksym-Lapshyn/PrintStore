using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrintStore.Domain.Concrete;
using System.Threading.Tasks;

namespace PrintStore.Domain.Entities
{
    public class Cart
    {
        public List<CartLine> CartLines { get; set; }
        public int Quantity { get { return CartLines.Count; } }
        public Cart()
        {
            CartLines = new List<CartLine>();
        }

        public void AddCartLine(int productId, int quantity)
        {
            EFBusinessLogicLayer layer = new EFBusinessLogicLayer();
            Product product = layer.Products.Where(p => p.ProductId == productId).First();
            CartLine cartLine = CartLines.Where(c => c.ProductId == productId).FirstOrDefault();
            if (cartLine == null)
            {
                CartLine newCartLine = new CartLine() { ProductId = productId, Product = product, Quantity = quantity };
                CartLines.Add(newCartLine);
            }
            else
            {
                cartLine.Quantity += quantity;
            }
        }

        public decimal ComputeTotalPrice()
        {
            EFBusinessLogicLayer layer = new EFBusinessLogicLayer();
            decimal totalPrice = CartLines.Sum(c => c.Product.Price * c.Quantity);
            return totalPrice;
        }

        public void RemoveCartLine(int productId)
        {
            CartLines.RemoveAll(c => c.ProductId == productId);
        }

        public void Clear()
        {
            CartLines.Clear();
        }
    }
}
