using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using TemplateProject.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TemplateProject.EntityModels;
using TemplateProject.EnumsAndConstants;
using TemplateProject.Manager.Managers;
using TemplateProject.Models.EmployeeViewModels;
using TemplateProject.UnitOfWorks;
using Microsoft.AspNetCore.Identity;

namespace TemplateProject.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly UnitOfWork _unitOfWork;

        private readonly IServiceProvider _serviceProvider;

        public EmployeeController
        (
            UnitOfWork unitOfWork,
            
            IServiceProvider serviceProvider
        )
        {
            _unitOfWork = unitOfWork;

            _serviceProvider = serviceProvider;
        }

        #region Custom Methods
        private EmployeeCreateVm GetEmployeeCreateVmAdditionalInfo(EmployeeCreateVm employeeCreateVm)
        {
            var BranchList = _unitOfWork.BranchManager.GetAll();

            employeeCreateVm.BranchList =
                BranchList.Select(x => new SelectListItem() {Text = x.Name, Value = x.Id.ToString()}).ToList();

            employeeCreateVm.GenderList = _unitOfWork.DropdownHelper.GetGenderDropdownList();

            var employeeList = _unitOfWork.EmployeeManager.GetAll();

            employeeCreateVm.EmployeGridVms = Mapper.Map<List<EmployeeGridVm>>(employeeList);

            employeeCreateVm.Id = _unitOfWork.EmployeeManager.GenerateId("EMP", 10);

            var roles = _unitOfWork.RoleManager(_serviceProvider).Roles.ToList();

            employeeCreateVm.DepartmentList = roles.Select(x => new SelectListItem() { Text = x.Name, Value = x.Name }).ToList();

            return employeeCreateVm;
        }

        private EmployeeCreateVm GetEmployeeCreateVmAdditionalInfoWithoutEmployeeList(EmployeeCreateVm employeeCreateVm)
        {
            var BranchList = _unitOfWork.BranchManager.GetAll();

            employeeCreateVm.BranchList =
                BranchList.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }).ToList();

            employeeCreateVm.GenderList = _unitOfWork.DropdownHelper.GetGenderDropdownList();

            employeeCreateVm.Id = _unitOfWork.EmployeeManager.GenerateId("EMP", 10);

            var roles = _unitOfWork.RoleManager(_serviceProvider).Roles.ToList();

            employeeCreateVm.DepartmentList = roles.Select(x => new SelectListItem() { Text = x.Name, Value = x.Name }).ToList();

            return employeeCreateVm;
        }

        private EmployeeEditVm GetEmployeeEditVmAdditionalInfo(EmployeeEditVm employeeEditVm)
        {
            var BranchList = _unitOfWork.BranchManager.GetAll();

            employeeEditVm.BranchList =
                BranchList.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }).ToList();

            employeeEditVm.GenderList = _unitOfWork.DropdownHelper.GetGenderDropdownList();

            var roles = _unitOfWork.RoleManager(_serviceProvider).Roles.ToList();

            employeeEditVm.DepartmentList = roles.Select(x => new SelectListItem() { Text = x.Name, Value = x.Name }).ToList();

            return employeeEditVm;
        }

        private Employee GetEmployeeUpdateInfo(Employee employee)
        {
            employee.UpdatedByUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            employee.UpdatedDateTime = _unitOfWork.DateTimeHelper.BDDateTime();

            return employee;
        }

        private Employee GetEmployeeCreateInfo(Employee employee)
        {
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            employee.CreatedByUserId = loggedInUserId;

            employee.CreatedDateTime = _unitOfWork.DateTimeHelper.BDDateTime();

            employee.UpdatedByUserId = loggedInUserId;

            employee.UpdatedDateTime = _unitOfWork.DateTimeHelper.BDDateTime();

            return employee;
        }

        private UserAccessVm GetUserAccessVmAdditionalInfo(UserAccessVm userAccessVm)
        {
            var allUser = _unitOfWork.UserManager(_serviceProvider).Users.Where(c => c.UserTypeId == null);

            userAccessVm.UserList =
                allUser.Select(x => new SelectListItem() { Text = x.UserName, Value = x.Id }).ToList();

            return userAccessVm;
        }

        #endregion

        #region Actions
        [Authorize(Policy = EmployeeClaims.Home)]
        public IActionResult Index()
        {
            EmployeeCreateVm employeeCreateVm = new EmployeeCreateVm();

            employeeCreateVm = GetEmployeeCreateVmAdditionalInfo(employeeCreateVm);

            return View(employeeCreateVm);
        }


        #endregion

        #region Ajax Call


        [HttpPost]
        [Authorize(Policy = EmployeeClaims.Create)]
        public IActionResult Index(EmployeeCreateVm employeeCreateVm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Employee employee = Mapper.Map<Employee>(employeeCreateVm);

                    employee = GetEmployeeCreateInfo(employee);

                    bool isCreated = _unitOfWork.EmployeeManager.Save(employee);

                    if (isCreated)
                    {
                        ModelState.Clear();

                        EmployeeCreateVm model = new EmployeeCreateVm();

                        model = GetEmployeeCreateVmAdditionalInfoWithoutEmployeeList(model);

                        return PartialView("_IndexView", model);
                    }
                }
                catch (Exception e)
                {
                    return PartialView("Error");
                }
            }

            ModelState.AddModelError("", "Validation Failed!");

            employeeCreateVm = GetEmployeeCreateVmAdditionalInfoWithoutEmployeeList(employeeCreateVm);

            return PartialView("_IndexView", employeeCreateVm);
        }

        [Authorize(Policy = EmployeeClaims.EditView)]
        public ActionResult EditView(string employeeId)
        {
            try
            {
                Employee employee = _unitOfWork.EmployeeManager.GetById(employeeId);

                EmployeeEditVm employeeEditVm = Mapper.Map<EmployeeEditVm>(employee);

                employeeEditVm = GetEmployeeEditVmAdditionalInfo(employeeEditVm);

                return PartialView("_EditView", employeeEditVm);
            }
            catch (Exception ex)
            {
                return PartialView("Error");
            }
        }

        [HttpPost]
        [Authorize(Policy = EmployeeClaims.Edit)]
        public ActionResult Edit(EmployeeEditVm employeeEditVm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Employee employee = _unitOfWork.EmployeeManager.GetById(employeeEditVm.Id);

                    employee = Mapper.Map(employeeEditVm, employee);

                    employee = GetEmployeeUpdateInfo(employee);

                    bool isUpdated = _unitOfWork.EmployeeManager.Update(employee);

                    if (isUpdated)
                    {
                        employeeEditVm = GetEmployeeEditVmAdditionalInfo(employeeEditVm);

                        return PartialView("_EditView", employeeEditVm);
                    }
                }
                catch (Exception ex)
                {
                    return PartialView("Error");
                }
            }

            ModelState.AddModelError("", "Validation Failed!");

            employeeEditVm = GetEmployeeEditVmAdditionalInfo(employeeEditVm);

            return PartialView("_EditView", employeeEditVm);
        }

        [Authorize(Policy = EmployeeClaims.Delete)]
        public JsonResult DeleteEmployee(string employeeId)
        {
            try
            {
                Employee employee = _unitOfWork.EmployeeManager.GetById(employeeId);

                bool isDeleted = _unitOfWork.EmployeeManager.Remove(employee);

                return Json(isDeleted);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        [Authorize(Policy = EmployeeClaims.Clear)]
        public IActionResult Clear()
        {
            try
            {
                EmployeeCreateVm employeeCreateVm = new EmployeeCreateVm();

                employeeCreateVm = GetEmployeeCreateVmAdditionalInfoWithoutEmployeeList(employeeCreateVm);

                return PartialView("_IndexView", employeeCreateVm);
            }
            catch (Exception ex)
            {
                return PartialView("Error");
            }

        }

        [Authorize(Policy = EmployeeClaims.EmployeeListTable)]
        public IActionResult EmployeeListGridView(long BranchId)
        {
            try
            {
                if (BranchId == 0)
                {
                    var employeeList = _unitOfWork.EmployeeManager.GetAll();

                    List<EmployeeGridVm> employeGridVms = new List<EmployeeGridVm>();

                    employeGridVms = Mapper.Map<List<EmployeeGridVm>>(employeeList);

                    return PartialView("_EmployeeGridView", employeGridVms);
                }
                else
                {
                    var employeeList = _unitOfWork.EmployeeManager.Get(c => c.BranchId == BranchId);

                    List<EmployeeGridVm> employeGridVms = new List<EmployeeGridVm>();

                    employeGridVms = Mapper.Map<List<EmployeeGridVm>>(employeeList);

                    return PartialView("_EmployeeGridView", employeGridVms);
                }

            }
            catch (Exception ex)
            {
                return PartialView("Error");
            }
        }

        [Authorize(Policy = EmployeeClaims.EmployeeSelectList)]
        public JsonResult EmployeeList(long BranchId)
        {
            try
            {
                List<SelectListItem> employees = new List<SelectListItem>();

                if (BranchId == 0)
                {
                    var employeeList = _unitOfWork.EmployeeManager.GetAll();

                    employees = employeeList
                        .Select(x => new SelectListItem() {Text = x.Name, Value = x.Id}).ToList();

                    return Json(employees);
                }
                else
                {
                    var employeeList = _unitOfWork.EmployeeManager.Get(c => c.BranchId == BranchId);

                    employees = employeeList
                        .Select(x => new SelectListItem() { Text = x.Name, Value = x.Id }).ToList();

                    return Json(employees);
                }

            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        [Authorize(Policy = EmployeeClaims.EmployeesAssignedToUsersWhoIsInRoleDoctor)]
        public JsonResult EmployeesOfDoctorDepartment(long branchId)
        {
            try
            {
                List<SelectListItem> employees = new List<SelectListItem>();

                if(branchId == 0)
                {
                    var employeeList =
                        _unitOfWork.EmployeeManager.Get(c => c.Department == RoleConstants.Doctor);

                    employees = employeeList
                        .Select(x => new SelectListItem() { Text = x.Name, Value = x.Id }).ToList();

                    return Json(employees);
                }
                else
                {
                    var employeeList =
                        _unitOfWork.EmployeeManager.Get(c => c.BranchId == branchId && c.Department == RoleConstants.Doctor);

                    employees = employeeList
                        .Select(x => new SelectListItem() { Text = x.Name, Value = x.Id }).ToList();

                    return Json(employees);
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        [Authorize(Policy = EmployeeClaims.EmployeeUserAccessView)]
        public IActionResult UserAccess(string employeeId)
        {
            try
            {
                UserAccessVm userAccessVm = new UserAccessVm();

                var user = _unitOfWork.UserManager(_serviceProvider).Users.FirstOrDefault(c => c.UserTypeId == employeeId);

                if (user != null)
                {
                    userAccessVm.UserAssignedName = user.UserName;
                }

                userAccessVm.UserTypeId = employeeId;

                userAccessVm = GetUserAccessVmAdditionalInfo(userAccessVm);

                return PartialView("_UserAccess", userAccessVm);
            }
            catch (Exception e)
            {
                return PartialView("Error");
            }
        }

        [HttpPost]
        [Authorize(Policy = EmployeeClaims.EmployeeUserAccess)]
        public async Task<IActionResult> UserAccess(UserAccessVm userAccessVm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = new IdentityResult();

                    var previousAssignedUser = _unitOfWork.UserManager(_serviceProvider).Users
                        .FirstOrDefault(c => c.UserTypeId == userAccessVm.UserTypeId);

                    if (previousAssignedUser != null)
                    {
                        previousAssignedUser.UserTypeId = null;

                        previousAssignedUser.Type = null;

                        await _unitOfWork.UserManager(_serviceProvider).UpdateAsync(previousAssignedUser);
                    }

                    var user = await _unitOfWork.UserManager(_serviceProvider).FindByIdAsync(userAccessVm.UserId);

                    user.Type = UserTypeConstants.Employee;

                    user.UserTypeId = userAccessVm.UserTypeId;

                    var newResult = await _unitOfWork.UserManager(_serviceProvider).UpdateAsync(user);

                    if (newResult.Succeeded)
                    {

                        var newUserAssigned = _unitOfWork.UserManager(_serviceProvider).Users.FirstOrDefault(c => c.UserTypeId == userAccessVm.UserTypeId);

                        if (newUserAssigned != null)
                        {
                            userAccessVm.UserAssignedName = newUserAssigned.UserName;
                        }

                        userAccessVm = GetUserAccessVmAdditionalInfo(userAccessVm);

                        return PartialView("_UserAccess", userAccessVm);
                    }
                    

                }
                catch (Exception e)
                {
                    return PartialView("Error");
                }
            }

            ModelState.AddModelError("", "Validation Failed!");

            var previouserUserAssigned = _unitOfWork.UserManager(_serviceProvider).Users.FirstOrDefault(c => c.UserTypeId == userAccessVm.UserTypeId);

            if (previouserUserAssigned != null)
            {
                userAccessVm.UserAssignedName = previouserUserAssigned.UserName;
            }

            userAccessVm = GetUserAccessVmAdditionalInfo(userAccessVm);

            return PartialView("_UserAccess", userAccessVm);
        }

        public IActionResult GetEmployeeInfo(string serviceId)
        {
            var employee = _unitOfWork.EmployeeManager.GetById(serviceId);
            return Json(employee);
        }

        #endregion


    }
}