using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PrintStore.Domain.Infrastructure.Abstract;
using PrintStore.Domain.Infrastructure.Concrete;
using PrintStore.Domain.Entities;

namespace PrintStore.Models
{
    /// <summary>
    /// View model of cart
    /// </summary>
    public class CartViewModel
    {
        public string UserId { get; set; }
        public bool UserIsBlocked { get; set; }
        public List<CartLineViewModel> CartLineViewModels { get; set; }
        public decimal TotalPrice { get { return CartLineViewModels.Sum(c => c.Product.Price * c.Quantity); } }

        private IBusinessLogicLayer businessLayer;

        //Contstuctor that takes arguments from Ninject
        public CartViewModel(IBusinessLogicLayer businessLogicLayerParam)
        {
            CartLineViewModels = new List<CartLineViewModel>();
            businessLayer = businessLogicLayerParam;
        }

        /// <summary>
        /// Adds view model of cartline to List
        /// </summary>
        /// <param name="productId">Id of product</param>
        /// <param name="quantity">Quantity of product</param>
        public void AddCartLineViewModel(int productId, int quantity)
        {
            Product product = businessLayer.Products.Where(p => p.ProductId == productId).First();
            CartLineViewModel cartLineViewModel = CartLineViewModels.Where(c => c.ProductId == productId).FirstOrDefault();
            //If there is no such cartline in List, creates new
            if (cartLineViewModel == null)
            {
                CartLineViewModel newCartLineViewModel = new CartLineViewModel() { ProductId = productId, Product = product, Quantity = quantity };
                CartLineViewModels.Add(newCartLineViewModel);
            }
            //If there is such cartline in List, adds to its quantity
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