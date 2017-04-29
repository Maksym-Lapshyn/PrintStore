using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PrintStore.Domain.Entities
{
    /// <summary>
    /// Category for products
    /// </summary>
    /// <param name='Products'>Collection of product entities</param>
    public class Category
    {
        public int CategoryId { get; set; }

        public bool IsDeleted { get; set; }

        [Required(ErrorMessage = "Enter name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Enter desciption")]
        public string Description { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        public Category()
        {
            Products = new List<Product>();
        }
    }
}
