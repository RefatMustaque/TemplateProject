using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TemplateProject.Models.RoleViewModels
{
    public class RoleClaimVm
    {
        public string RoleId { get; set; }

        public List<SelectListItem> ClaimList { get; set; }
    }
}
