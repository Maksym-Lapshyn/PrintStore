using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PrintStore.Models;
using System.ComponentModel.DataAnnotations;

namespace PrintStore.Infrastructure
{
    public class FilterAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            FilterViewModel filters = value as FilterViewModel;
            if (filters.SelectedMinimum < filters.MinimumLimit || filters.SelectedMinimum > filters.MaximumLimit
                || filters.SelectedMaximum > filters.MaximumLimit || filters.SelectedMaximum < filters.MinimumLimit
                || (filters.SelectedMaximum == filters.SelectedMinimum && filters.SelectedMinimum == 0))
            {
                return false;
            }

            return true;
        }
    }
}