using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TemplateProject.Models.EmployeeViewModels
{
    public class EmployeeGridVm
    {
        [Display(Name = "ID")]
        public string Id { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Joining Date")]
        public DateTime JoinDate { get; set; }

        public string Department { get; set; }

    }
}
