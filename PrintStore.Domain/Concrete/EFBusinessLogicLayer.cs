using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrintStore.Domain.Abstract;
using PrintStore.Domain.Entities;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web;

namespace PrintStore.Domain.Concrete
{
    /// <summary>
    /// Business logic layer based on Entity Framework
    /// </summary>
    public class EFBusinessLogicLayer : IBusinessLogicLayer
    {
        private EFDbContext context = new EFDbContext();

        public IEnumerable<Product> Products
        {
            get
            {
                return context.Products.Where(p => !p.IsDeleted);
            }
        }

        public IEnumerable<Category> Categories
        {
            get
            {
                return context.Categories.Where(c => !c.IsDeleted);
            }
        }

        public IEnumerable<Order> Orders
        {
            get
            {
                return context.Orders.Where(o => !o.IsDeleted);
            }
        }

        /// <summary>
        /// Location for temporary image that is uploaded with product
        /// </summary>
        public const string TempImagePath = "~/Images/temp.jpg";

        /// <summary>
        /// If id of uploaded category is 0, it is saved as new entity
        /// </summary>
        /// <param name="category">Category to save</param>
        public void SaveCategory(Category category)
        {
            //Add new
            if (category.CategoryId == 0)
            {
                context.Categories.Add(category);
            }
            //Update
            else
            {
                Category forSave = context.Categories.Find(category.CategoryId);
                if(forSave != null)
                {
                    forSave.CategoryId = category.CategoryId;
                    forSave.Name = category.Name;
                    forSave.Description = category.Description;
                    IEnumerable<Product> productsOfCategory = Products.Where(p => p.Category == category).ToList();
                    forSave.Products = (ICollection<Product>)productsOfCategory;
                }
            }

            context.SaveChanges();
        }

        public Category DeleteCategory(int categoryId)
        {
            Category category = context.Categories.Find(categoryId);
            if (category != null)
            {
                category.IsDeleted = true;
                category.Products.ToList().ForEach(p => p.IsDeleted = true);
            }

            context.SaveChanges();
            return category;
        }

        /// <summary>
        /// If id of uploaded product is 0, it is saved as new entity
        /// Imageguid is generated when image of product is uploaded
        /// If imageguid is not null, it is used to save images of product
        /// </summary>
        /// <param name="product">Product to save</param>
        public void SaveProduct(Product product)
        {
            //Add new
            if (product.ProductId == 0)
            {
                product.DateAdded = DateTime.UtcNow;
                if (product.ImageGuid != default(Guid))
                {
                    product = SaveImages(product);
                }

                Category category = context.Categories.Where(c => c.CategoryId == product.CategoryId).First();
                category.Products.Add(product);
                product.Category = category;
                context.Products.Add(product);
            }
            //Update
            else
            {
                Product forSave = context.Products.Find(product.ProductId);
                if(forSave != null)
                {
                    forSave.ProductId = product.ProductId;
                    if (product.ImageGuid != default(Guid))
                    {
                        forSave.ImageGuid = product.ImageGuid;
                        forSave = SaveImages(forSave);
                    }

                    forSave.Name = product.Name;
                    forSave.Description = product.Description;
                    forSave.Price = product.Price;
                    forSave.Material = product.Material;
                    forSave.Texture = product.Texture;
                    forSave.Size = product.Size;
                    forSave.CategoryId = product.CategoryId;
                    Category category = context.Categories.Where(c => c.CategoryId == forSave.CategoryId).First();
                    forSave.Category = category;
                    category.Products.Add(forSave);
                }
            }

            context.SaveChanges();
        }

        /// <summary>
        /// Creates two bitmaps from temporary image and saves them using Imageguid as jpeg files
        /// </summary>
        /// <param name="product">Product to save images for</param>
        /// <returns>Updated product</returns>
        private Product SaveImages(Product product)
        {
            String tempImageLocation = HttpContext.Current.Server.MapPath(TempImagePath);
            Bitmap tempImage = new Bitmap(tempImageLocation);
            System.Drawing.Size imageSize = new System.Drawing.Size(1200, 800);
            Bitmap bigImage = new Bitmap(tempImage, imageSize);
            bigImage.Save(HttpContext.Current.Server.MapPath(string.Format("~/Images/big_{0}.jpg", product.ImageGuid)), ImageFormat.Jpeg);
            bigImage.Dispose();
            product.BigImagePath = string.Format("big_{0}.jpg", product.ImageGuid);
            imageSize = new System.Drawing.Size(300, 200);
            Bitmap smallImage = new Bitmap(tempImage, imageSize);
            smallImage.Save(HttpContext.Current.Server.MapPath(string.Format("~/Images/small_{0}.jpg", product.ImageGuid)), ImageFormat.Jpeg);
            smallImage.Dispose();
            tempImage.Dispose();
            product.SmallImagePath = string.Format("small_{0}.jpg", product.ImageGuid);
            System.IO.File.Delete(tempImageLocation);
            return product;
        }

        public Product DeleteProduct(int productId)
        {
            Product product = context.Products.Find(productId);
            if (product != null)
            {
                product.IsDeleted = true;
                context.SaveChanges();
            }

            context.SaveChanges();
            return product;
        }

        public decimal GetPriceLimit(bool minimum)
        {
            decimal priceLimit = 0;
            if (minimum)
            {
                priceLimit = context.Products.Min(p => p.Price);
            }
            else
            {
                priceLimit = context.Products.Max(p => p.Price);
            }

            return priceLimit;
        }

        public void SaveOrder(Cart cart, string userId)
        {
            Order order = new Order();
            order.DateAdded = DateTime.UtcNow;
            order.UserId = userId;
            order.CartLines = cart.CartLines;
            order.TotalPrice = cart.ComputeTotalPrice();
            context.Orders.Add(order);
            context.SaveChanges();
        }
    }
}
