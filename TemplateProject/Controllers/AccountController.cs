using TemplateProject.EntityModels;
using TemplateProject.EnumsAndConstants;
using TemplateProject.Extensions;
using TemplateProject.Helpers;
using TemplateProject.Manager.Managers;
using TemplateProject.Models.AccountViewModels;
using TemplateProject.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TemplateProject.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly DateTimeHelper _datetimeHelper;
        private readonly LoginHistoryManager _loginHistoryManager;
        private readonly NetworkHelper _networkHelper;
        private readonly ClaimHelper _claimHelper;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ILogger<AccountController> logger,
            RoleManager<IdentityRole> roleManager,
            DateTimeHelper datetimeHelper,
            LoginHistoryManager loginHistoryManager,
            IHttpContextAccessor contextAccessor,
            NetworkHelper networkHelper,
            ClaimHelper claimHelper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
            _roleManager = roleManager;
            _datetimeHelper = datetimeHelper;
            _networkHelper = networkHelper;
            _loginHistoryManager = loginHistoryManager;
            _claimHelper = claimHelper;
        }

        [TempData]
        public string ErrorMessage { get; set; }

        /******************************** Custom Method ************************************/

        #region Custom Method

        private RegisterViewModel GetRegisterViewModelAdditionalInfo(RegisterViewModel registerViewModel)
        {
            var roleList = _roleManager.Roles.ToList();

            registerViewModel.RoleList =
                roleList.Select(x => new SelectListItem() { Text = x.Name, Value = x.Name }).ToList();

            var registeredUserList = _userManager.Users.ToList();

            registerViewModel.RegisteredUserList = registeredUserList.Select(x => new ApplicationUserVm() { Id = x.Id, UserName = x.UserName }).ToList();

            return registerViewModel;
        }


        private async Task<UserRoleVm> GetUserRoleVmAdditionalInfoAsync(ApplicationUser user, UserRoleVm userRoleVm)
        {
            userRoleVm.UserId = user.Id;

            var userAssignedRole = await _userManager.GetRolesAsync(user);

            userRoleVm.UserRoleGridVmList = await GetRoleListAsUserGridVmListAsync(userAssignedRole);

            var allRole = _roleManager.Roles.ToList();

            allRole = RemoveUserAssignedRoleFromRoleList(userAssignedRole, allRole);

            userRoleVm.RoleList = allRole.Select(x => new SelectListItem() { Text = x.Name, Value = x.Name }).ToList();

            return userRoleVm;
        }

        private async Task<List<UserRoleGridVm>> GetRoleListAsUserGridVmListAsync(IList<string> userAssignedRole)
        {
            List<UserRoleGridVm> userRoleGridVmList = new List<UserRoleGridVm>();

            foreach (var roleName in userAssignedRole)
            {
                var role = await _roleManager.FindByNameAsync(roleName);

                userRoleGridVmList.Add(new UserRoleGridVm() { RoleId = role.Id, RoleName = role.Name });
            }

            return userRoleGridVmList;
        }

        private List<IdentityRole> RemoveUserAssignedRoleFromRoleList(IList<string> userAssignedRole, List<IdentityRole> roleList)
        {
            foreach (var role in roleList.ToList())
            {
                if (userAssignedRole.Any(c => c.Contains(role.Name)))
                {
                    roleList.Remove(role);
                }
            }

            return roleList;
        }

        #endregion


        /************************************* Action **************************************/

        #region Action
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewData["ReturnUrl"] = returnUrl;

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: true);

                var logInAttemptUser = await _userManager.FindByNameAsync(model.Email);

                LoginHistory loginHistory = new LoginHistory();

                if (logInAttemptUser != null)
                {
                    loginHistory.IpAddress = _networkHelper.GetRemoteIpAddress(Request);
                    loginHistory.MacAddress = _networkHelper.GetPhysicalAddress();
                    loginHistory.DeviceInfo = _networkHelper.GetBrowserInfo(Request);
                    loginHistory.LoginDateTime = _datetimeHelper.BDDateTime();
                    loginHistory.ApplicationUserId = logInAttemptUser.Id;
                }

                if (result.Succeeded)
                {
                    _loginHistoryManager.Save(loginHistory);

                    _logger.LogInformation("User logged in.");

                    if (String.IsNullOrEmpty(returnUrl))
                    {
                        return RedirectToAction("Index", "Dashboard");
                    }
                    else
                    {
                        return RedirectToLocal(returnUrl);
                    }

                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToAction(nameof(LoginWith2fa), new { returnUrl, model.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToAction(nameof(Lockout));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> LoginWith2fa(bool rememberMe, string returnUrl = null)
        {
            // Ensure the user has gone through the username & password screen first
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();

            if (user == null)
            {
                throw new ApplicationException($"Unable to load two-factor authentication user.");
            }

            var model = new LoginWith2faViewModel { RememberMe = rememberMe };
            ViewData["ReturnUrl"] = returnUrl;

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginWith2fa(LoginWith2faViewModel model, bool rememberMe, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var authenticatorCode = model.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);

            var result = await _signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, rememberMe, model.RememberMachine);

            if (result.Succeeded)
            {
                _logger.LogInformation("User with ID {UserId} logged in with 2fa.", user.Id);
                return RedirectToLocal(returnUrl);
            }
            else if (result.IsLockedOut)
            {
                _logger.LogWarning("User with ID {UserId} account locked out.", user.Id);
                return RedirectToAction(nameof(Lockout));
            }
            else
            {
                _logger.LogWarning("Invalid authenticator code entered for user with ID {UserId}.", user.Id);
                ModelState.AddModelError(string.Empty, "Invalid authenticator code.");
                return View();
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> LoginWithRecoveryCode(string returnUrl = null)
        {
            // Ensure the user has gone through the username & password screen first
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new ApplicationException($"Unable to load two-factor authentication user.");
            }

            ViewData["ReturnUrl"] = returnUrl;

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginWithRecoveryCode(LoginWithRecoveryCodeViewModel model, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new ApplicationException($"Unable to load two-factor authentication user.");
            }

            var recoveryCode = model.RecoveryCode.Replace(" ", string.Empty);

            var result = await _signInManager.TwoFactorRecoveryCodeSignInAsync(recoveryCode);

            if (result.Succeeded)
            {
                _logger.LogInformation("User with ID {UserId} logged in with a recovery code.", user.Id);
                return RedirectToLocal(returnUrl);
            }
            if (result.IsLockedOut)
            {
                _logger.LogWarning("User with ID {UserId} account locked out.", user.Id);
                return RedirectToAction(nameof(Lockout));
            }
            else
            {
                _logger.LogWarning("Invalid recovery code entered for user with ID {UserId}", user.Id);
                ModelState.AddModelError(string.Empty, "Invalid recovery code entered.");
                return View();
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Lockout()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Policy = AccountClaims.Home)]
        public IActionResult Register()
        {
            RegisterViewModel registerViewModel = new RegisterViewModel();

            registerViewModel = GetRegisterViewModelAdditionalInfo(registerViewModel);

            return View(registerViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToAction(nameof(Login));
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in with {Name} provider.", info.LoginProvider);
                return RedirectToLocal(returnUrl);
            }
            if (result.IsLockedOut)
            {
                return RedirectToAction(nameof(Lockout));
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                ViewData["ReturnUrl"] = returnUrl;
                ViewData["LoginProvider"] = info.LoginProvider;
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                return View("ExternalLogin", new ExternalLoginViewModel { Email = email });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await _signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    throw new ApplicationException("Error loading external login information during confirmation.");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(nameof(ExternalLogin), model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{userId}'.");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToAction(nameof(ForgotPasswordConfirmation));
                }

                // For more information on how to enable account confirmation and password reset please
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.ResetPasswordCallbackLink(user.Id, code, Request.Scheme);
                await _emailSender.SendEmailAsync(model.Email, "Reset Password",
                   $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");
                return RedirectToAction(nameof(ForgotPasswordConfirmation));
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            if (code == null)
            {
                throw new ApplicationException("A code must be supplied for password reset.");
            }
            var model = new ResetPasswordViewModel { Code = code };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }
            AddErrors(result);
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }


        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult ChangePassword() { return View(); }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(HttpContext.User);
                var result = await _userManager.ChangePasswordAsync(currentUser, model.OldPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Dashboard");
                }
            }
            return View();
        }



        #endregion

        /************************************* Ajax Call **************************************/

        #region AjaxCall

        [HttpPost]
        [Authorize(Policy = AccountClaims.Create)]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var loggedInUser = User.FindFirstValue(ClaimTypes.NameIdentifier);

                    var user = new ApplicationUser { UserName = model.Email, Email = model.Email, RegisteredById = loggedInUser, RegistrationDateTime = _datetimeHelper.BDDateTime() };

                    var result = await _userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User created a new account with password.");

                        //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        //var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
                        //await _emailSender.SendEmailConfirmationAsync(model.Email, callbackUrl);

                        //await _signInManager.SignInAsync(user, isPersistent: false);
                        //_logger.LogInformation("User created a new account with password.");

                        RegisterViewModel registerViewModel = new RegisterViewModel();

                        registerViewModel = GetRegisterViewModelAdditionalInfo(registerViewModel);

                        return PartialView("Register", registerViewModel);
                    }
                    AddErrors(result);

                    ModelState.AddModelError("", "Duplicate email!");

                    model = GetRegisterViewModelAdditionalInfo(model);
                    // If we got this far, something failed, redisplay form
                    return PartialView("Register", model);
                }
                catch (Exception)
                {
                    return PartialView("Error");
                }
            }

            ModelState.AddModelError("", "Validation Failed");

            model = GetRegisterViewModelAdditionalInfo(model);
            // If we got this far, something failed, redisplay form
            return PartialView("Register", model);
        }


        [Authorize(Policy = AccountClaims.UserListTable)]
        public async Task<IActionResult> GetUserListByRole(string roleName)
        {
            try
            {
                List<ApplicationUserVm> userVmList = new List<ApplicationUserVm>();

                if (roleName != null)
                {
                    var userList = await _userManager.GetUsersInRoleAsync(roleName);

                    userVmList = userList
                        .Select(x => new ApplicationUserVm() { Id = x.Id, UserName = x.UserName }).ToList();
                }

                else
                {
                    var allUser = _userManager.Users.ToList();

                    userVmList = allUser.Select(x => new ApplicationUserVm() { Id = x.Id, UserName = x.UserName }).ToList();
                }

                return PartialView("_userGridView", userVmList);
            }
            catch (Exception)
            {
                return PartialView("Error");
            }
        }

        [Authorize(Policy = AccountClaims.Delete)]
        public async Task<JsonResult> Remove(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);

                var userRoles = await _userManager.GetRolesAsync(user);

                await _userManager.RemoveFromRolesAsync(user, userRoles);

                var result = await _userManager.DeleteAsync(user);

                return Json(result.Succeeded);
            }
            catch (Exception e)
            {
                return Json(e.Message);
            }
        }

        [Authorize(Policy = AccountClaims.UserPermissionView)]
        public async Task<IActionResult> ClaimViewAsync(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);

                var permittedClaimListForUser = _userManager.GetClaimsAsync(user);

                UserClaimVm userClaimVm = new UserClaimVm();

                userClaimVm.UserId = userId;

                userClaimVm.ClaimList = _claimHelper.GetAllAvailableClaims();

                userClaimVm.ClaimList = _claimHelper.SetGivenPermissionInClaimList(userClaimVm.ClaimList, permittedClaimListForUser);

                return PartialView("_ClaimView", userClaimVm);
            }
            catch (Exception)
            {
                return PartialView("Error");
            }
        }

        [Authorize(Policy = AccountClaims.ChangePermission)]
        public async Task<IActionResult> TogglePermission(string claimValue, string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);

                var claimList = _userManager.GetClaimsAsync(user);

                var result = new IdentityResult();

                if (claimList.Result.Any(x => x.Type == claimValue))
                {
                    result = await _userManager.RemoveClaimAsync(user, new Claim(claimValue, user.Id));
                }
                else
                {
                    result = await _userManager.AddClaimAsync(user, new Claim(claimValue, user.Id));
                }

                return Json(result.Succeeded);
            }
            catch (Exception)
            {
                return PartialView("Error");
            }
        }


        [Authorize(Policy = AccountClaims.UserRolesView)]
        public async Task<IActionResult> UserRole(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);

                UserRoleVm userRoleVm = new UserRoleVm();

                userRoleVm = await GetUserRoleVmAdditionalInfoAsync(user, userRoleVm);

                return PartialView("_UserRole", userRoleVm);
            }
            catch (Exception)
            {
                return PartialView("Error");
            }
        }


        [HttpPost]
        [Authorize(Policy = AccountClaims.AddUserToRole)]
        public async Task<IActionResult> UserRole(UserRoleVm userRoleVm)
        {
            var user = await _userManager.FindByIdAsync(userRoleVm.UserId);

            if (ModelState.IsValid)
            {
                try
                {
                    var result = _userManager.AddToRoleAsync(user, userRoleVm.RoleId);

                    if (result.Result.Succeeded)
                    {
                        ModelState.Clear();

                        UserRoleVm model = new UserRoleVm();

                        model = await GetUserRoleVmAdditionalInfoAsync(user, userRoleVm);

                        return PartialView("_UserRole", userRoleVm);
                    }
                }
                catch (Exception)
                {
                    return PartialView("Error");
                }
            }

            ModelState.AddModelError("", "Validation Failed!");

            userRoleVm = await GetUserRoleVmAdditionalInfoAsync(user, userRoleVm);

            return PartialView("_UserRole", userRoleVm);
        }

        [Authorize(Policy = AccountClaims.RemoveUserFromRole)]
        public async Task<JsonResult> RemoveFromRole(string userId, string roleId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);

                var role = await _roleManager.FindByIdAsync(roleId);

                var result = await _userManager.RemoveFromRoleAsync(user, role.Name);

                return Json(result.Succeeded);
            }
            catch (Exception e)
            {
                return Json(e.Message);
            }
        }

        #endregion

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        #endregion

    }
}

