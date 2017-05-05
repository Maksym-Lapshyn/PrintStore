using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PrintStore.Domain.Concrete;
using PrintStore.Domain.Entities;
using PrintStore.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace PrintStore.Models
{
    [Filter(ErrorMessage = "Select correct minimum and maximum prices")]
    public class FilterViewModel
    {
        EFBusinessLogicLayer layer = new EFBusinessLogicLayer();

        public int CategoryId { get; set; }

        public decimal SelectedMinimum { get; set; }

        public decimal MinimumLimit { get; set; }

        public decimal SelectedMaximum { get; set; }

        public decimal MaximumLimit { get; set; }

        public Material Material { get; set; }

        public Texture Texture { get; set; }

        public Size Size { get; set; }

        public SortOrder SortOrder { get; set; }

        public FilterViewModel()
        {
            MinimumLimit = layer.GetPriceLimit(true);
            MaximumLimit = layer.GetPriceLimit(false);
        }
    }

    public enum Material
    {
        All,
        Plastic,
        Metal,
        Ceramic,
        Paper
    }

    public enum Size
    {
        All,
        Tiny,
        Small,
        Medium,
        Large
    }

    public enum Texture
    {
        All,
        Flat,
        Dotted,
        Squares,
        Triangles
    }

    public enum SortOrder
    {
        None,
        NameAsc,
        NameDesc,
        PriceAsc,
        PriceDesc,
        DateAsc,
        DateDesc
    }
}