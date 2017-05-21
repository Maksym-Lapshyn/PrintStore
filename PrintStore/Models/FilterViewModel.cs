using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PrintStore.Domain.Infrastructure.Concrete;
using PrintStore.Domain.Entities;
using PrintStore.Infrastructure.Attributes;
using System.ComponentModel.DataAnnotations;
using PrintStore.Domain.Infrastructure.Abstract;

namespace PrintStore.Models
{
    /// <summary>
    /// View model of filter
    /// </summary>
    [Filter(ErrorMessage = "Select correct minimum and maximum prices")]
    public class FilterViewModel
    {
        IBusinessLogicLayer businessLayer;

        public int CategoryId { get; set; }

        public decimal SelectedMinimum { get; set; }

        public decimal MinimumLimit { get; set; }

        public decimal SelectedMaximum { get; set; }

        public decimal MaximumLimit { get; set; }

        public Material Material { get; set; }

        public Texture Texture { get; set; }

        public Size Size { get; set; }

        public SortOrder SortOrder { get; set; }

        public FilterViewModel(IBusinessLogicLayer businessLayerParam)
        {
            businessLayer = businessLayerParam;
            MinimumLimit = businessLayer.GetPriceLimit(true);
            MaximumLimit = businessLayer.GetPriceLimit(false);
        }

        public FilterViewModel()
        {
            businessLayer = new EFBusinessLogicLayer();
            MinimumLimit = businessLayer.GetPriceLimit(true);
            MaximumLimit = businessLayer.GetPriceLimit(false);
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