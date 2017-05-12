using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PrintStore.Domain.Infrastructure.Concrete;
using PrintStore.Domain.Entities;
using PrintStore.Models;
using PrintStore.Infrastructure.Attributes;

namespace PrintStore.Controllers
{
    [ActionLogging]

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
            ProductsViewModel model = new ProductsViewModel();
            model.Products = products;
            model.Filter = new FilterViewModel();
            model.Filter.CategoryId = categoryId;
            return View(model);
        }

        [HttpPost]
        public ActionResult GetProducts(ProductsViewModel model)
        {
            model.Products = layer.Products.Where(p => p.CategoryId == model.Filter.CategoryId && p.IsDeleted == false);
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.Products = ApplyFilters(model.Products, model.Filter);
            return View(model);
        }

        public ActionResult GetProductDetails(int productId)
        {
            Product product = layer.Products.Where(p => p.ProductId == productId).First();
            return View(product);
        }

        private IEnumerable<Product> ApplyFilters(IEnumerable<Product> products, FilterViewModel filter)
        {
            products = products.Where(p => p.Price >= filter.SelectedMinimum && p.Price <= filter.SelectedMaximum);

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
                products = SortProducts(products, filter.SortOrder);
            }

            return products;
        }

        private IEnumerable<Product> SortProducts(IEnumerable<Product> products, PrintStore.Models.SortOrder sortOrder)
        {
            if (sortOrder == PrintStore.Models.SortOrder.NameAsc)
            {
                products = products.OrderBy(p => p.Name);
            }
            else if (sortOrder == PrintStore.Models.SortOrder.NameDesc)
            {
                products = products.OrderByDescending(p => p.Name);
            }
            else if (sortOrder == PrintStore.Models.SortOrder.PriceAsc)
            {
                products = products.OrderBy(p => p.Price);
            }
            else if (sortOrder == PrintStore.Models.SortOrder.PriceDesc)
            {
                products = products.OrderByDescending(p => p.Price);
            }
            else if (sortOrder == PrintStore.Models.SortOrder.DateAsc)
            {
                products = products.OrderBy(p => p.DateAdded);
            }
            else if (sortOrder == PrintStore.Models.SortOrder.DateDesc)
            {
                products = products.OrderByDescending(p => p.DateAdded);
            }

            return products;
        }
    }
}