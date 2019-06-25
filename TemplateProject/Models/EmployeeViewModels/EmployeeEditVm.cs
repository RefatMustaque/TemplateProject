using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TemplateProject.Models.EmployeeViewModels
{
    public class EmployeeEditVm
    {
        public EmployeeEditVm()
        {
            GenderList = new List<SelectListItem>();

            BranchList = new List<SelectListItem>();
        }


        [Required]
        [Display(Name = "ID")]
        public string Id { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Gender")]
        public string Gender { get; set; }


        public List<SelectListItem> GenderList { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Joining Date")]
        public DateTime? JoinDate { get; set; }

        [Required]
        [Display(Name = "Branch")]
        public long BranchId { get; set; }

        public string Department { get; set; }


        public List<SelectListItem> DepartmentList { get; set; }


        public List<SelectListItem> BranchList { get; set; }
    }
}
