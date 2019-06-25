using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TemplateProject.EnumsAndConstants
{
    public static class AccountClaims
    {
        public const string Home = "Account:Register";

        public const string Create = "Account:Register:Post";

        public const string Delete = "Account:Remove";

        public const string UserListTable = "Account:GetUserListByRole";

        public const string UserPermissionView = "Account:ClaimViewAsync";

        public const string ChangePermission = "Account:TogglePermission";

        public const string UserRolesView = "Account:UserRole";

        public const string AddUserToRole = "Account:UserRole:Post";

        public const string RemoveUserFromRole = "Account:RemoveFromRole";
    }

    public static class RoleClaims
    {
        public const string Home = "Role:Index";

        public const string Create = "Role:Create";

        public const string Delete = "Role:RemoveAsync";

        public const string RoleListTable = "Role:GetRoleList";

        public const string RolePermissionView = "Role:ClaimViewAsync";

        public const string ChangePermission = "Role:TogglePermission";
    }

    public static class EmployeeClaims
    {
        public const string Home = "Employee:Index";

        public const string Create = "Employee:Index:Post";

        public const string EditView = "Employee:EditView";

        public const string Edit = "Employee:Edit";

        public const string Delete = "Employee:DeleteEmployee";

        public const string Clear = "Employee:Clear";

        public const string EmployeeListTable = "Employee:EmployeeListGridView";

        public const string EmployeeSelectList = "Employee:EmployeeList";

        public const string EmployeeUserAccessView = "Employee:UserAccess";

        public const string EmployeeUserAccess = "Employee:UserAccess:Post";

        public const string EmployeesAssignedToUsersWhoIsInRoleDoctor = "Employee:EmployeesOfDoctorDepartment";
    }


}
