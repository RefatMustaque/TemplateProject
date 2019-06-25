using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace TemplateProject.Models.EmployeeViewModels
{
    public class UserAccessVm
    {
        public string UserAssignedName { get; set; }

        [Required]
        public string UserId { get; set; }

        public List<SelectListItem> UserList { get; set; }

        public string UserTypeId { get; set; }
    }
}
