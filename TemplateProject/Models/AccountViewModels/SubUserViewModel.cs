using TemplateProject.EntityModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TemplateProject.Models.AccountViewModels
{
    public class SubUserViewModel
    {
        public SubUserViewModel()
        {
            ApplicationUsers = new List<SelectListItem>();
        }
        public string Id { get; set; }
        [Display(Name ="Select Parent User")]
        public string ParentUserEmail { get; set; }
        public List<SelectListItem> ApplicationUsers { get; set; }
    }
}
