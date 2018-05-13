using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Fault_handling_system.Models;
using Fault_handling_system.Models.AccountViewModels;
using Fault_handling_system.Services;
using Fault_handling_system.Data;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Fault_handling_system.Controllers
{
    /// <summary>
    /// The AccountController class.
    /// Containts all methods for controlling actions on an account.
    /// </summary>
    [Authorize]
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly ApplicationDbContext _context;
        private ApplicationUser editableUser;

        /// <summary>
        /// AccountController constructor.
        /// Initializes the necessary managers.
        /// </summary>
        /// <param name="userManager">UsermManager</param>
        /// <param name="signInManager">SignInManager</param>
        /// <param name="emailSender">IEmailSender</param>
        /// <param name="logger">ILogger</param>
        /// <param name="context">ApplicationDbContext</param>
        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ILogger<AccountController> logger,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
            _context = context;
        }

        /// <value>Gets and sets the ErrorMessage</value>
        [TempData]
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Initializes login process.
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns>Login view</returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        /// <summary>
        /// The Login method.
        /// Logs user in.
        /// </summary>
        /// <param name="model">LoginViewModel</param>
        /// <param name="returnUrl"></param>
        /// <returns>View after login</returns>
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
                var user = await _userManager.FindByEmailAsync(model.Email);
                var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return RedirectToLocal(returnUrl);
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

        /// <summary>
        /// Admin guide.
        /// </summary>
        /// <returns>Admin guide view</returns>
        [Authorize(Roles = "Admin")]
        public IActionResult AdminGuide()
        {
            return View();
        }

        /// <summary>
        /// Edit roles method.
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>View with user info and interface to edit it's roles</returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditRoles(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var model = new EditableUser();
            var user = await _userManager.FindByIdAsync(id);
            model.UserName = user.UserName;
            model.Email = user.Email;
            model.Roles = await _userManager.GetRolesAsync(user);
            var roles = GetAllRoles();
            model.ERoles = GetSelectListItems(roles);
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        /// <summary>
        /// Assigns role to user.
        /// </summary>
        /// <param name="model">User model</param>
        /// <param name="returnUrl"></param>
        /// <returns>View with updated user info and interface to edit it's roles</returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Assign(EditableUser model, string returnUrl = null)
        {
            var roles = GetAllRoles();
            model.ERoles = GetSelectListItems(roles);
            ViewData["ReturnUrl"] = returnUrl;

            var user = await _userManager.FindByEmailAsync(model.Email);
            await _userManager.AddToRoleAsync(user, model.Role);
            model.Roles = await _userManager.GetRolesAsync(user);

            await EditRoles(user.Id);
            return View("~/Views/Account/EditRoles.cshtml");
        }

        /// <summary>
        /// Unassigns role from user.
        /// </summary>
        /// <param name="model">User model</param>
        /// <param name="returnUrl"></param>
        /// <returns>View with updated user info and interface to edit it's roles</returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unassign(EditableUser model, string returnUrl = null)
        {
            var roles = GetAllRoles();
            model.ERoles = GetSelectListItems(roles);
            ViewData["ReturnUrl"] = returnUrl;

            var user = await _userManager.FindByEmailAsync(model.Email);
            await _userManager.RemoveFromRoleAsync(user, model.Role);
            model.Roles = await _userManager.GetRolesAsync(user);

            await EditRoles(user.Id);
            return View("~/Views/Account/EditRoles.cshtml");
        }

        /// <summary>
        /// Delete user method.
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>View with user info and a button to confirm deletion</returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = new EditableUser();
            var user = await _userManager.FindByIdAsync(id);
            model.UserName = user.UserName;
            model.Email = user.Email;
            model.Roles = await _userManager.GetRolesAsync(user);
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        /// <summary>
        /// Deletes user.
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>View to manage users</returns>
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var query = await (from x in _context.Report
                               where x.RequestorId == id || x.NsnCoordinatorId == id || x.SubcontractorId == id
                               select x).ToListAsync();
            if (query.Count() > 0)
            {
                TempData["ErrorMessage"] = "This user is assigned to an existing report.";
            }

            var user = await _userManager.FindByIdAsync(id);
            user.LockoutEnabled = true;
            user.LockoutEnd = DateTime.UtcNow.AddYears(100);
            await _userManager.UpdateAsync(user);
            //await _userManager.DeleteAsync(user);

            return RedirectToAction(nameof(ManageUsers));
        }

        /// <summary>
        /// Manages users
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns>View with listed users with buttons to edit them</returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ManageUsers(string returnUrl = null)
        {
            var model = new ManageUsersViewModel();
            var roles = GetAllRoles();
            //model.Users = _userManager.Users.ToList();
            model.Users = _userManager.Users.Where(u => u.LockoutEnd == null).ToList();
            model.Roles = new IList<string>[model.Users.Count];
            model.ERoles = GetSelectListItems(roles);
            int counter = 0;
            foreach (var user in model.Users)
            {
                model.Roles[counter] = await _userManager.GetRolesAsync(user);
                counter++;
            }
            ViewData["ReturnUrl"] = returnUrl;
            return View(model);
        }

        /// <summary>
        /// Register method
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns>View with register form</returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Register(string returnUrl = null)
        {
            TempData["result"] = "9";
            var roles = GetAllRoles();
            var model = new RegisterViewModel();
            model.Roles = GetSelectListItems(roles);
            ViewData["ReturnUrl"] = returnUrl;
            return View(model);
        }

        /// <summary>
        /// Registers new user
        /// </summary>
        /// <param name="model">RegisterViewModel</param>
        /// <param name="returnUrl"></param>
        /// <returns>View after user is registered</returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            string password = CreatePassword(5);
            var roles = GetAllRoles();
            model.Roles = GetSelectListItems(roles);
            ViewData["ReturnUrl"] = returnUrl;

            var existing = await _userManager.FindByEmailAsync(model.Email);
            if (existing != null) ModelState.AddModelError("Email", "A user witth given e-mail address already exists.");

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Login, Email = model.Email };
                var result = await _userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, model.Role);
                    _logger.LogInformation("User created a new account with password.");
                    StringBuilder emailMessage = new StringBuilder();
                    emailMessage.Append("Username: ");
                    emailMessage.Append(user.UserName);
                    emailMessage.Append(" | Password to your account: ");
                    emailMessage.Append(password);
                    await _emailSender.SendEmailAsync(user.Email, "Password to your FHS account", emailMessage.ToString());
                    _logger.LogInformation("Email sent!");

                    return RedirectToAction("Register", "Account");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        /// <summary>
        /// Creates random password
        /// </summary>
        /// <param name="length">Password length</param>
        /// <returns>Password string</returns>
        public string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890-!$#@*";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            res.Append("-Aa1");
            return res.ToString();
        }

        /// <summary>
        /// Gets available roles.
        /// </summary>
        /// <returns>List of avaiable roles</returns>
        private IEnumerable<string> GetAllRoles()
        {
            return new List<string>
            {
                "Admin",
                "Requestor",
                "Nokia Coordinator",
                "Subcontractor"
            };
        }

        /// <summary>
        /// Creates a IEnumerable<SelectListItem> from List
        /// </summary>
        /// <param name="elements">List of some string elements</param>
        /// <returns>IEnumerable<SelectListItem> object</returns>
        private IEnumerable<SelectListItem> GetSelectListItems(IEnumerable<string> elements)
        {
            // Create an empty list to hold result of the operation
            var selectList = new List<SelectListItem>();

            // For each string in the 'elements' variable, create a new SelectListItem object
            // that has both its Value and Text properties set to a particular value.
            // This will result in MVC rendering each item as:
            //     <option value="State Name">State Name</option>
            foreach (var element in elements)
            {
                selectList.Add(new SelectListItem
                {
                    Value = element,
                    Text = element
                });
            }

            return selectList;
        }

        /// <summary>
        /// Logs out a user
        /// </summary>
        /// <returns>Redirection to homepage</returns>
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
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
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

        [HttpGet]
        public async Task<IActionResult> UserPage()
        {
            IQueryable<Report> applicationDbContext;

            if (User.IsInRole("Admin"))
            {
                applicationDbContext = _context.Report.Include(r => r.EtrStatus).Include(r => r.EtrType).Include(r => r.NsnCoordinator).Include(r => r.Requestor).Include(r => r.Subcontractor).Include(r => r.Zone).Where(r => r.NsnCoordinatorId == null).OrderByDescending(r => r.DateIssued);
                return View(await applicationDbContext.ToListAsync());
            }

            return View();
        }

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
                return RedirectToAction(nameof(AccountController.UserPage), "Account");
            }
        }

        #endregion
    }
}
