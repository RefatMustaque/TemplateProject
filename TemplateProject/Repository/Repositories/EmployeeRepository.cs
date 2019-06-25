using TemplateProject.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TemplateProject.Data;
using TemplateProject.EntityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TemplateProject.EnumsAndConstants;

namespace TemplateProject.Repository.Repositories
{
    public class EmployeeRepository : Repository<Employee>
    {
        public TemplateProjectDbContext TemplateProjectDbContext { get; set; }

        public EmployeeRepository(TemplateProjectDbContext dbContext) : base(dbContext)
        {
            TemplateProjectDbContext = dbContext;
        }

        internal List<Employee> GetEmployeesAssignedToUsersWhoIsInRole(string roleName)
        {
            var role = TemplateProjectDbContext.Roles.FirstOrDefault(c => c.Name == roleName);

            var userRoles = TemplateProjectDbContext.UserRoles.Where(c => c.RoleId == role.Id);

            var userList = TemplateProjectDbContext.Users.Where(c =>
                c.Type == "Employee" && userRoles.Any(d => d.UserId == c.Id));

            return TemplateProjectDbContext.Employees.Where(c => userList.Any(d => d.UserTypeId == c.Id)).ToList();
        }

        internal ApplicationUser GetApplicationUserByEmployeeId(string employeeId)
        {
            return TemplateProjectDbContext.Users.FirstOrDefault(c => c.UserTypeId == employeeId);
        }

        internal List<Employee> GetEmployeesAssignedToUsersWhoIsInRoleAndBranch(string roleName, long BranchId)
        {
            var role = TemplateProjectDbContext.Roles.FirstOrDefault(c => c.Name == roleName);

            var userRoles = TemplateProjectDbContext.UserRoles.Where(c => c.RoleId == role.Id);

            var userList = TemplateProjectDbContext.Users.Where(c =>
                c.Type == "Employee" && userRoles.Any(d => d.UserId == c.Id));

            return TemplateProjectDbContext.Employees.Where(c => c.BranchId == BranchId && userList.Any(d => d.UserTypeId == c.Id)).ToList();
        }

        public List<Employee> GetAllDoctor()
        {
            return Table.Where(x => x.Department == RoleConstants.Doctor).ToList();
        }
    }
}
