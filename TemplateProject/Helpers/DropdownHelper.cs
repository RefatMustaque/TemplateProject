using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TemplateProject.Helpers
{
    public class DropdownHelper
    {
        internal List<SelectListItem> GetYearDropdown()
        {
            List<SelectListItem> dropdownList = new List<SelectListItem>();

            for (int i = DateTime.Now.Year; i >= 1900; i--)
            {
                dropdownList.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }

            return dropdownList;
        }

        internal List<SelectListItem> GetGenderDropdownList()
        {
            List<SelectListItem> dropdownList = new List<SelectListItem>();

            dropdownList.Add(new SelectListItem() { Text = "Male", Value = "Male" });

            dropdownList.Add(new SelectListItem() { Text = "Female", Value = "Female" });

            dropdownList.Add(new SelectListItem() { Text = "Other", Value = "Other" });

            return dropdownList;
        }

        internal List<SelectListItem> GetMaritalStatusDropdownList()
        {
            List<SelectListItem> dropdownList = new List<SelectListItem>();

            dropdownList.Add(new SelectListItem() { Text = "Married", Value = "Married" });

            dropdownList.Add(new SelectListItem() { Text = "Unmarried", Value = "Unmarried" });

            return dropdownList;
        }

        internal List<SelectListItem> GetIntervalDropdown()
        {
            List<SelectListItem> dropdownList = new List<SelectListItem>();

            dropdownList.Add(new SelectListItem() { Text = "7 Days", Value = "7" });

            dropdownList.Add(new SelectListItem() { Text = "10 Days", Value = "10" });

            dropdownList.Add(new SelectListItem() { Text = "15 Days", Value = "15" });

            return dropdownList;
        }

        internal List<SelectListItem> GetNumberDropdownList(int from, int to)
        {
            List<SelectListItem> dropdownList = new List<SelectListItem>();

            for(int i = from; i <= to; i++ )
            {
                dropdownList.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }

            return dropdownList;
        }
    }
}
