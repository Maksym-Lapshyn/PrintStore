using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PrintStore.Domain.Infrastructure.Abstract;
using PrintStore.Domain.Infrastructure.Concrete;
using PrintStore.Domain.Entities;

namespace PrintStore.Models
{
    public class CartViewModel
    {
        public string UserId { get; set; }
        public bool UserIsBlocked { get; set; }
        public List<CartLineViewModel> CartLineViewModels { get; set; }
        public decimal TotalPrice { get { return CartLineViewModels.Sum(c => c.Product.Price * c.Quantity); } }

        private IBusinessLogicLayer businessLayer;

        public CartViewModel(IBusinessLogicLayer businessLogicLayerParam)
        {
            CartLineViewModels = new List<CartLineViewModel>();
            businessLayer = businessLogicLayerParam;
        }

        public void AddCartLineViewModel(int productId, int quantity)
        {
            Product product = businessLayer.Products.Where(p => p.ProductId == productId).First();
            CartLineViewModel cartLineViewModel = CartLineViewModels.Where(c => c.ProductId == productId).FirstOrDefault();
            if (cartLineViewModel == null)
            {
                CartLineViewModel newCartLineViewModel = new CartLineViewModel() { ProductId = productId, Product = product, Quantity = quantity };
                CartLineViewModels.Add(newCartLineViewModel);
            }
            else
            {
                cartLineViewModel.Quantity += quantity;
            }
        }

        public void RemoveCartLineViewModel(int productId)
        {
            CartLineViewModels.RemoveAll(c => c.ProductId == productId);
        }

        public void Clear()
        {
            CartLineViewModels.Clear();
        }
    }
}