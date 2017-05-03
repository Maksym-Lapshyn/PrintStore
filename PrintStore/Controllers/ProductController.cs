using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PrintStore.Domain.Concrete;
using PrintStore.Domain.Entities;
using PrintStore.Models;

namespace PrintStore.Controllers
{
    public class ProductController : Controller
    {
        EFBusinessLogicLayer layer = new EFBusinessLogicLayer();

        public ActionResult GetCategories()
        {
            IEnumerable<Category> categories = layer.Categories.Where(c => c.IsDeleted == false);
            return View(categories.ToList());
        }

        public ActionResult GetProducts(int categoryId)
        {
            IEnumerable<Product> products = layer.Products.Where(p => p.CategoryId == categoryId && p.IsDeleted == false);
            return View(products.ToList());
        }

        public ActionResult GetProductDetails(int productId)
        {
            Product product = layer.Products.Where(p => p.ProductId == productId).First();
            return View(product);
        }

        [HttpGet]
        public PartialViewResult DisplayFilter()
        {
            return PartialView(new FilterViewModel());
        }

        [HttpPost]
        public ActionResult DisplayFilter(FilterViewModel filter)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(filter);
            }

            IEnumerable<Product> products = layer.Products.Where(p => p.IsDeleted == false);
            products = ApplyFilters(products, filter);
            return View("GetProducts", products.ToList());
        }


        private IEnumerable<Product> ApplyFilters(IEnumerable<Product> products, FilterViewModel filter)
        {
            products = products.Where(p => p.Price >= filter.SelectedMinimum && p.Price <= filter.SelectedMaximum);
            if (filter.Category != "All")
            {
                products = products.Where(p => p.CategoryId == Int32.Parse(filter.Category));
            }

            if (filter.Material.ToString() != "All")
            {
                products = products.Where(p => p.Material.ToString() == filter.Material.ToString());
            }

            if (filter.Size.ToString() != "All")
            {
                products = products.Where(p => p.Size.ToString() == filter.Size.ToString());
            }

            if (filter.Texture.ToString() != "All")
            {
                products = products.Where(p => p.Texture.ToString() == filter.Texture.ToString());
            }

            if (filter.SortOrder.ToString() != "None")
            {
                products = layer.SortProducts(products, filter.SortOrder.ToString());
            }

            return products;
        }
    }
}