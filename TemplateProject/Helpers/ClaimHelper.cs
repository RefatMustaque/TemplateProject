using TemplateProject.EnumsAndConstants;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TemplateProject.Helpers
{
    public class ClaimHelper
    {
        public List<SelectListItem> GetAllAvailableClaims()
        {
            List<SelectListItem> selectListItems = new List<SelectListItem>
            {

                //AccountController
                new SelectListItem() { Text = "Can View Home Page Of User Registration", Value = AccountClaims.Home },

                new SelectListItem() { Text = "Can Register New User", Value = AccountClaims.Create },

                new SelectListItem() { Text = "Can View User List Table", Value = AccountClaims.UserListTable },

                new SelectListItem() { Text = "Can See Permission For Users", Value = AccountClaims.UserPermissionView },

                new SelectListItem() { Text = "Can Change Permission Of Users", Value = AccountClaims.ChangePermission },

                new SelectListItem() { Text = "Can Remove User", Value = AccountClaims.Delete },

                new SelectListItem() { Text = "Can View User Role ", Value = AccountClaims.UserRolesView },

                new SelectListItem() { Text = "Can Add Role To User ", Value = AccountClaims.AddUserToRole },

                new SelectListItem() { Text = "Can Remove User From Role", Value = AccountClaims.RemoveUserFromRole },

                //RoleControleer
                new SelectListItem() { Text = "Can View Home Page Of Role", Value = RoleClaims.Home },

                new SelectListItem() { Text = "Can Create Role", Value = RoleClaims.Create },

                new SelectListItem() { Text = "Can View Role List Table", Value = RoleClaims.RoleListTable },

                new SelectListItem() { Text = "Can View Permission of Roles", Value = RoleClaims.RolePermissionView },

                new SelectListItem() { Text = "Can Change Permission Of Roles", Value = RoleClaims.ChangePermission },

                new SelectListItem() { Text = "Can Remove Roles", Value = RoleClaims.Delete },

                //EmployeeController
                new SelectListItem() { Text = "Can View Employe Home Page", Value = EmployeeClaims.Home },

                new SelectListItem() { Text = "Can Add New Employee", Value = EmployeeClaims.Create },

                new SelectListItem() { Text = "Can View Employee Edit Page", Value = EmployeeClaims.EditView },

                new SelectListItem() { Text = "Can Edit Employee", Value = EmployeeClaims.Edit },

                new SelectListItem() { Text = "Can Delete Employee", Value = EmployeeClaims.Delete },

                new SelectListItem() { Text = "Can Access The Clear Button Of Employee Create View", Value = EmployeeClaims.Clear },

                new SelectListItem() { Text = "Can View The Employee Table", Value = EmployeeClaims.EmployeeListTable },

                new SelectListItem() { Text = "Can Access The Employee List In Other View", Value = EmployeeClaims.EmployeeSelectList },

                new SelectListItem() { Text = "Can View The Employee User Access View", Value = EmployeeClaims.EmployeeUserAccessView },

                new SelectListItem() { Text = "Can Set The Employee User Access", Value = EmployeeClaims.EmployeeUserAccess },

                new SelectListItem() { Text = "Can Get Employee List Who is in Role Doctor", Value = EmployeeClaims.EmployeesAssignedToUsersWhoIsInRoleDoctor },

            };

            return selectListItems;
        }

        internal List<SelectListItem> SetGivenPermissionInClaimList(List<SelectListItem> claimSelectList, Task<IList<Claim>> permittedClaimList)
        {
            foreach (var claim in claimSelectList)
            {
                if (permittedClaimList.Result.Any(a => a.Type == claim.Value))
                {
                    claim.Selected = true;
                }
            }

            return claimSelectList;
        }
    }
}
