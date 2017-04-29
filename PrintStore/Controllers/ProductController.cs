using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PrintStore.Domain.Concrete;
using PrintStore.Domain.Entities;

namespace PrintStore.Controllers
{
    public class ProductController : Controller
    {
        EFBusinessLogicLayer layer = new EFBusinessLogicLayer();

        public ActionResult GetCategories()
        {
            IEnumerable<Category> categories = layer.Categories.Where(c => c.IsDeleted == false);
            return View(categories);
        }

        public ActionResult GetProducts(int categoryId)
        {
            IEnumerable<Product> products = layer.Products.Where(p => p.CategoryId == categoryId && p.IsDeleted == false);
            return View(products);
        }

        public ActionResult GetProductDetails(int productId)
        {
            Product product = layer.Products.Where(p => p.ProductId == productId).First();
            return View(product);
        }

        public ActionResult GetSortedProducts(int categoryId, string sortOption)
        {
            EFBusinessLogicLayer.SortingOptions option = (EFBusinessLogicLayer.SortingOptions)Enum.Parse(typeof(EFBusinessLogicLayer.SortingOptions), sortOption);
            IEnumerable<Product> products = layer.SortProducts(categoryId, option);
            return View("GetProducts", products);
        }

        public PartialViewResult ApplyFiltersToProducts()
        {
            return PartialView();
        }
    }
}