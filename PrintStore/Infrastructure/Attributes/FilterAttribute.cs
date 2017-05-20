using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PrintStore.Models;
using System.ComponentModel.DataAnnotations;

namespace PrintStore.Infrastructure.Attributes
{
    /// <summary>
    /// Attribute for validation of filter
    /// </summary>
    public class FilterAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            FilterViewModel filter = value as FilterViewModel;
            if (filter.SelectedMinimum < filter.MinimumLimit || filter.SelectedMinimum > filter.MaximumLimit
                || filter.SelectedMaximum > filter.MaximumLimit || filter.SelectedMaximum < filter.MinimumLimit
                || (filter.SelectedMaximum == filter.SelectedMinimum && filter.SelectedMinimum == 0))
            {
                return false;
            }

            return true;
        }
    }
}