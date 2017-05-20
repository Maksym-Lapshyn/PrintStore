using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrintStore.Domain.Infrastructure.Abstract;
using PrintStore.Domain.Infrastructure.Contexts;
using PrintStore.Domain.Entities;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web;

namespace PrintStore.Domain.Infrastructure.Concrete
{
    /// <summary>
    /// Functionality embodied with the help of Entity Framework
    /// </summary>
    public class EFBusinessLogicLayer : IBusinessLogicLayer
    {
        //Database context
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
                return context.Orders;
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
                if (forSave != null)
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

        /// <summary>
        /// Deletes category by changing its property IsDeleted to true
        /// </summary>
        /// <param name="categoryId">Id of category to delete</param>
        /// <returns></returns>
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
        /// If imageguid is not default, it is used to save images of product
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
                if (forSave != null)
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
            //Change resolution for bigger picture in the next line
            System.Drawing.Size imageSize = new System.Drawing.Size(1200, 800);
            Bitmap bigImage = new Bitmap(tempImage, imageSize);
            bigImage.Save(HttpContext.Current.Server.MapPath(string.Format("~/Images/big_{0}.jpg", product.ImageGuid)), ImageFormat.Jpeg);
            bigImage.Dispose();
            product.BigImagePath = string.Format("big_{0}.jpg", product.ImageGuid);
            //Change resolution for smaller picture in the next line
            imageSize = new System.Drawing.Size(300, 200);
            Bitmap smallImage = new Bitmap(tempImage, imageSize);
            smallImage.Save(HttpContext.Current.Server.MapPath(string.Format("~/Images/small_{0}.jpg", product.ImageGuid)), ImageFormat.Jpeg);
            smallImage.Dispose();
            tempImage.Dispose();
            product.SmallImagePath = string.Format("small_{0}.jpg", product.ImageGuid);
            System.IO.File.Delete(tempImageLocation);
            return product;
        }

        /// <summary>
        /// Deletes category by changing its property IsDeleted to true
        /// </summary>
        /// <param name="productId">Id of product to delete</param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets price limits from a product's table
        /// </summary>
        /// <param name="minimum">True for minimum limit, false for maximum</param>
        /// <returns></returns>
        public decimal GetPriceLimit(bool minimum)
        {
            decimal priceLimit = 0;
            if (minimum)
            {
                priceLimit = context.Products.Where(p => p.IsDeleted == false).Min(p => p.Price);
            }
            else
            {
                priceLimit = context.Products.Where(p => p.IsDeleted == false).Max(p => p.Price);
            }

            return priceLimit;
        }

        /// <summary>
        /// Saves orders to database
        /// </summary>
        /// <param name="cartLines">Collection of cartlines from a user's cart</param>
        /// <param name="userId">Id of a user who checked out</param>
        public void SaveOrder(List<CartLine> cartLines, string userId)
        {
            Order order = new Order();
            order.DateAdded = DateTime.UtcNow;
            order.UserId = userId;
            order.CartLines = cartLines;
            order.TotalPrice = order.CartLines.Sum(c => context.Products.Where(p => p.ProductId == c.ProductId).First().Price * c.Quantity);
            context.Orders.Add(order);
            context.SaveChanges();
        }

        /// <summary>
        /// Deletes order by changing its property IsDeleted to true
        /// </summary>
        /// <param name="orderId">Id of order to delete</param>
        /// <returns></returns>
        public Order DeleteOrder(int orderId)
        {
            Order order = context.Orders.Find(orderId);
            order.IsDeleted = true;
            context.SaveChanges();
            return order;
        }

        /// <summary>
        /// Restores order from by changing its property IsDeleted to true
        /// </summary>
        /// <param name="orderId">Id of order to delete</param>
        /// <returns></returns>
        public Order RestoreOrder(int orderId)
        {
            Order order = context.Orders.Find(orderId);
            order.IsDeleted = false;
            context.SaveChanges();
            return order;
        }

        /// <summary>
        /// Changes status of order
        /// </summary>
        /// <param name="orderId">Id of order to change</param>
        /// <param name="orderStatus">Selected status</param>
        public void ChangeOrderStatus(int orderId, string orderStatus)
        {
            Order order = context.Orders.Find(orderId);
            if (orderStatus == OrderStatus.Paid.ToString())
            {
                order.OrderStatus = OrderStatus.Paid;
            }
            else if (orderStatus == OrderStatus.Canceled.ToString())
            {
                order.OrderStatus = OrderStatus.Canceled;
            }
            else
            {
                order.OrderStatus = OrderStatus.Registered;
            }

            context.SaveChanges();
        }
    }
}
