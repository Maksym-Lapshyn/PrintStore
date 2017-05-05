using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PrintStore.Domain.Concrete;
using PrintStore.Domain.Entities;

namespace PrintStore.Controllers
{
    public class AdminController : Controller
    {
        EFBusinessLogicLayer layer = new EFBusinessLogicLayer();

        public ActionResult GetCategories()
        {
            IEnumerable<Category> categories = layer.Categories.Where(c => c.IsDeleted == false);
            return View(categories);
        }

        public ActionResult EditCategory(int categoryId)
        {
            Category category = layer.Categories.Where(c => c.CategoryId == categoryId).First();
            return View(category);
        }

        [HttpPost]
        public ActionResult EditCategory(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            layer.SaveCategory(category);
            TempData["message"] = string.Format("{0} was successfully updated", category.Name);
            return RedirectToAction("GetCategories");
        }

        public ActionResult AddCategory()
        {
            return View("EditCategory", new Category());
        }

        [HttpPost]
        public ActionResult DeleteCategory(int categoryId)
        {
            Category category = layer.DeleteCategory(categoryId);
            if (category != null)
            {
                TempData["message"] = string.Format("{0} was successfully deleted", category.Name);
            }

            return RedirectToAction("GetCategories");
        }

        public ActionResult EditProduct(int productId)
        {
            Product product = layer.Products.Where(p => p.ProductId == productId).First();
            return View(product);
        }

        [HttpPost]
        public ActionResult EditProduct(Product product, HttpPostedFileBase image)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            if (image != null)
            {
                image.SaveAs(Server.MapPath(EFBusinessLogicLayer.TempImagePath));
                product.ImageGuid = Guid.NewGuid();
            }

            layer.SaveProduct(product);
            TempData["message"] = string.Format("{0} was successfully updated", product.Name);
            return RedirectToAction("GetCategories");
        }

        public ActionResult AddProduct(int categoryId)
        {
            Product product = new Product();
            product.CategoryId = categoryId;
            return View("EditProduct", product);
        }

        [HttpPost]
        public ActionResult DeleteProduct(int productId)
        {
            Product product = layer.DeleteProduct(productId);
            if (product != null)
            {
                TempData["message"] = string.Format("{0} was successfully deleted", product.Name);
            }

            return RedirectToAction("GetCategories");
        }
    }
}