using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrintStore.Domain.Entities;

namespace PrintStore.Domain.Infrastructure.Abstract
{
    /// <summary>
    /// Basic functionality of a business logic layer
    /// </summary>
    public interface IBusinessLogicLayer
    {
        void SaveProduct(Product product);

        Product DeleteProduct(int productId);

        void SaveCategory(Category category);

        Category DeleteCategory(int categoryId);
    }
}
