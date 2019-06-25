using System;
using System.Collections.Generic;
using TemplateProject.EntityModels;
using TemplateProject.Manager.Base;
using TemplateProject.Repository.Repositories;

namespace TemplateProject.Manager.Managers
{
    public class EmployeeManager : Manager<Employee>
    {
        private EmployeeRepository EmployeeRepository { get; set; }

        public EmployeeManager(EmployeeRepository repository) : base(repository)
        {
            EmployeeRepository = repository;
        }

        internal List<Employee> GetEmployeesAssignedToUsersWhoIsInRole(string roleName)
        {
            return EmployeeRepository.GetEmployeesAssignedToUsersWhoIsInRole(roleName);
        }

        internal List<Employee> GetEmployeesAssignedToUsersWhoIsInRoleAndBranch(string roleName, long BranchId)
        {
            return EmployeeRepository.GetEmployeesAssignedToUsersWhoIsInRoleAndBranch(roleName, BranchId);
        }
        public List<Employee> GetAllDoctor()
        {
            return EmployeeRepository.GetAllDoctor();
        }

        internal ApplicationUser GetApplicationUserByEmployeeId(string employeeId)
        {
            return EmployeeRepository.GetApplicationUserByEmployeeId(employeeId);
        }
    }
}
