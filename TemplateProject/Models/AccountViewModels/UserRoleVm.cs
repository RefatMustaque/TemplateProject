using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TemplateProject.Models.AccountViewModels
{
    public class UserRoleVm
    {
        public UserRoleVm()
        {
            RoleList = new List<SelectListItem>();

            UserRoleGridVmList = new List<UserRoleGridVm>();
        }

        [Required]
        public string UserId { get; set; }

        [Required]
        [Display(Name = "Role")]
        public string RoleId { get; set; }

        public List<SelectListItem> RoleList { get; set; }

        public List<UserRoleGridVm> UserRoleGridVmList { get; set; }
    }
}
