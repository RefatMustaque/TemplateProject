using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TemplateProject.Models.RoleViewModels
{
    public class RoleIndexVm
    {
        public RoleIndexVm()
        {
            RoleCreateVm = new RoleCreateVm();

            RoleList = new List<RoleGridVm>();
        }

        public RoleCreateVm RoleCreateVm { get; set; }

        public List<RoleGridVm> RoleList { get; set; }
    }
}
