using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TemplateProject.Models.AccountViewModels
{
    public class UserClaimVm
    {
        public string UserId { get; set; }

        public List<SelectListItem> ClaimList { get; set; }
    }
}
