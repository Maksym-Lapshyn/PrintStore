using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrintStore.Domain.Entities;

namespace PrintStore.Domain.Infrastructure.Abstract
{
    /// <summary>
    /// Basic functionality that should be persistent throughout a web-store
    /// </summary>
    public interface IBusinessLogicLayer
    {
        IEnumerable<Product> Products { get; }

        IEnumerable<Category> Categories { get; }

        IEnumerable<Order> Orders { get; }

        void SaveProduct(Product product);

        Product DeleteProduct(int productId);

        void SaveCategory(Category category);

        Category DeleteCategory(int categoryId);

        decimal GetPriceLimit(bool minimum);

        void SaveOrder(List<CartLine> cartLines, string userId);

        Order DeleteOrder(int orderId);

        Order RestoreOrder(int orderId);

        void ChangeOrderStatus(int orderId, string orderStatus);
    }
}
