using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TemplateProject.Repository;
using TemplateProject.EntityModels;
using TemplateProject.EnumsAndConstants;
using TemplateProject.Helpers;
using TemplateProject.Models.RoleViewModels;
using TemplateProject.UnitOfWorks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace TemplateProject.Controllers
{
    public class RoleController : Controller
    {
        private readonly UnitOfWork _unitOfWork;

        private readonly IServiceProvider _serviceProvider;

        public RoleController
        (
            UnitOfWork unitOfWork,

            IServiceProvider serviceProvider
        )
        {
            _unitOfWork = unitOfWork;

            _serviceProvider = serviceProvider;
        }


        #region Action

        [Authorize(Policy = RoleClaims.Home)]
        public IActionResult Index()
        {
            try
            {
                RoleIndexVm roleIndexVm = new RoleIndexVm();

                var roleList = _unitOfWork.RoleManager(_serviceProvider).Roles;

                roleIndexVm.RoleList = roleList.Select(x => new RoleGridVm() { Id = x.Id, RoleName = x.Name }).ToList();

                return View(roleIndexVm);

            }
            catch (Exception e)
            {
                return View("Error");
            }

        }

        #endregion

        #region Ajax Call
        [Authorize(Policy = RoleClaims.Create)]
        public IActionResult Create(RoleCreateVm roleCreateVm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    IdentityRole role = new IdentityRole(){ Name = roleCreateVm.RoleName};

                    var result = _unitOfWork.RoleManager(_serviceProvider).CreateAsync(role);

                    if (result.Result.Succeeded)
                    {
                        ModelState.Clear();

                        RoleCreateVm model = new RoleCreateVm();

                        return PartialView("_Create", model);
                    }
                }
                catch (Exception e)
                {
                    return PartialView("Error");
                }
            }

            ModelState.AddModelError("", "Validation Failed!");

            return PartialView("_Create", roleCreateVm);
        }

        [Authorize(Policy = RoleClaims.RoleListTable)]
        public IActionResult GetRoleList()
        {
            try
            {
                var roleList = _unitOfWork.RoleManager(_serviceProvider).Roles;

                List<RoleGridVm> roleGridVmList = new List<RoleGridVm>();

                roleGridVmList = roleList.Select(x => new RoleGridVm() { Id = x.Id, RoleName = x.Name }).ToList();

                return PartialView("_GridView", roleGridVmList);
            }
            catch (Exception e)
            {
                return PartialView("Error");
            }
        }

        [Authorize(Policy = RoleClaims.RolePermissionView)]
        public async Task<IActionResult> ClaimViewAsync(string roleId)
        {
            try
            {
                var role = await _unitOfWork.RoleManager(_serviceProvider).FindByIdAsync(roleId);

                var permittedClaimListForRole = _unitOfWork.RoleManager(_serviceProvider).GetClaimsAsync(role);

                RoleClaimVm roleClaimVm = new RoleClaimVm();

                roleClaimVm.RoleId = roleId;

                roleClaimVm.ClaimList = _unitOfWork.ClaimHelper.GetAllAvailableClaims();

                roleClaimVm.ClaimList =
                    _unitOfWork.ClaimHelper.SetGivenPermissionInClaimList(roleClaimVm.ClaimList, permittedClaimListForRole);

                return PartialView("_ClaimView", roleClaimVm);
            }
            catch (Exception e)
            {
                return PartialView("Error");
            }
        }

        [Authorize(Policy = RoleClaims.ChangePermission)]
        public async Task<IActionResult> TogglePermission(string claimValue, string roleId)
        {
            try
            {
                var role = await _unitOfWork.RoleManager(_serviceProvider).FindByIdAsync(roleId);

                var claimList = _unitOfWork.RoleManager(_serviceProvider).GetClaimsAsync(role);

                var result = new IdentityResult();

                if (claimList.Result.Any(x => x.Type == claimValue))
                {
                    result = await _unitOfWork.RoleManager(_serviceProvider).RemoveClaimAsync(role, new Claim(claimValue, role.Id));
                }
                else
                {
                    result = await _unitOfWork.RoleManager(_serviceProvider).AddClaimAsync(role, new Claim(claimValue, role.Id));
                }

                return Json(result.Succeeded);
            }
            catch (Exception e)
            {
                return PartialView("Error");
            }
        }

        [Authorize(Policy = RoleClaims.Delete)]
        public async Task<JsonResult> RemoveAsync(string roleId)
        {
            try
            {
                var role = await _unitOfWork.RoleManager(_serviceProvider).FindByIdAsync(roleId);

                var result = await _unitOfWork.RoleManager(_serviceProvider).DeleteAsync(role);

                return Json(result.Succeeded);
            }
            catch (Exception e)
            {
                return Json(e);
            }
        }

        #endregion

    }
}