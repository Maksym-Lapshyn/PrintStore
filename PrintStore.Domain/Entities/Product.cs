using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PrintStore.Domain.Entities
{
    /// <summary>
    /// Basic selling unit of PrintStore
    /// </summary>
    /// <param name='DateAdded'>Generates when product is added to db</param>
    /// <param name='BigImagePath'>Path to high-resolution picture of product</param>
    /// <param name='SmallImagePath'>Path to low-resolution picture of product</param>
    /// <param name='ImageGuid'>Unique id for naming entity's images</param>
    public class Product
    {
        public int ProductId { get; set; }

        public Guid ImageGuid { get; set; }

        public bool IsDeleted { get; set; }

        [Required(ErrorMessage = "Enter name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Specify price")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Enter positive price")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Enter description")]
        public string Description { get; set; }

        public DateTime DateAdded { get; set; }

        public string SmallImagePath { get; set; }

        public string BigImagePath { get; set; }

        [Required(ErrorMessage = "Select material")]
        public Material Material { get; set; }

        [Required(ErrorMessage = "Select size")]
        public Size Size { get; set; }

        [Required(ErrorMessage = "Select texture")]
        public Texture Texture { get; set; }

        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }

    public enum Material
    {
        Plastic,
        Metal,
        Ceramic,
        Paper
    }

    public enum Size
    {
        Tiny,
        Small,
        Medium,
        Large
    }

    public enum Texture
    {
        Flat,
        Dotted,
        Squares,
        Triangles
    }
}
