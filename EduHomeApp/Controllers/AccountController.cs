using EduHomeApp.Helpers;
using EduHomeApp.Models;
using EduHomeApp.Services.Interfaces;
using EduHomeApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHomeApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailService _emailService;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager, IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailService = emailService;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Register(RegisterVm registerVM)
        {
            if (!ModelState.IsValid) return View(registerVM);
            AppUser user = new()
            {
                FullName = registerVM.FullName,
                UserName = registerVM.UserName,
                Email = registerVM.Email,
            };
            var result = await _userManager.CreateAsync(user, registerVM.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(registerVM);
            }
            await _userManager.AddToRoleAsync(user, RolesEnum.member.ToString());
            //send mail
            string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            string link = Url.Action(nameof(VerifyEmail), "Account", new { email = user.Email, token = token },
                Request.Scheme, Request.Host.ToString());
            string body = string.Empty;
            using (StreamReader streamReader = new StreamReader("wwwroot/emailTemplate/emailConfirm.html"))
            {
                body = streamReader.ReadToEnd();
            };
            body = body.Replace("{{link}}", link);
            body = body.Replace("{{username}}", user.UserName);
            _emailService.SendEmail(body, new() { user.Email }, "Email vertfication", "Verify email");



            return RedirectToAction("RegisterVerifyMessage", "account");
        }

        public IActionResult RegisterVerifyMessage()
        {
            return View();
        }
        public async Task<IActionResult> VerifyEmail(string email, string token)
        {
            AppUser appUser = await _userManager.FindByEmailAsync(email);
            if (appUser == null) return NotFound();
            await _userManager.ConfirmEmailAsync(appUser, token);
            await _signInManager.SignInAsync(appUser, true);
            return RedirectToAction("index", "Home");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login(LoginVm loginVM, string? ReturnUrl)
        {
            if (!ModelState.IsValid) return View(loginVM);
            var user = await _userManager.FindByEmailAsync(loginVM.UserNameOrEmail);
            if (user == null)
            {
                user = await _userManager.FindByNameAsync(loginVM.UserNameOrEmail);
                if (user == null)
                {
                    ModelState.AddModelError("", "(username or email) or password is wrong...");
                    return View(loginVM);
                }
            }
            var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, loginVM.RememberMe, true);
            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "user is lockout...");
                return View(loginVM);
            }
            if (!user.EmailConfirmed)
            {
                ModelState.AddModelError("", "go to verify email");
                return View(loginVM);
            }
            if (user.IsBlocked)
            {
                ModelState.AddModelError("", "user is blocked...");
                return View(loginVM);
            }
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "(username or email) or password is wrong...");
                return View(loginVM);
            }
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains("admin")) return RedirectToAction("Index", "dashboard", new { area = "adminarea" });
            if (ReturnUrl == null)
                return RedirectToAction("index", "home");
            return Redirect(ReturnUrl);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }

        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            AppUser appUser = await _userManager.FindByEmailAsync(email);
            if (appUser == null)
            {
                ModelState.AddModelError("Error1", "Emaile aid istifadeci tapilmadi");
                return View();
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(appUser);
            string url = Url.Action(nameof(ResetPassword), "Account",
                new { email = appUser.Email, token }, Request.Scheme, Request.Host.ToString());

            string body = string.Empty;
            using (StreamReader streamReader = new StreamReader("wwwroot/emailTemplate/forgotPassword.html"))
            {
                body = streamReader.ReadToEnd();
            };
            body = body.Replace("{{link}}", url);
            _emailService.SendEmail(body, new() { appUser.Email }, "Forgot password", "reset pasword");


            return RedirectToAction(nameof(Index), "home");
        }
        public async Task<IActionResult> ResetPassword(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return NotFound();
            bool result = await _userManager.
                   VerifyUserTokenAsync(user, _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", token);
            if (!result)
            {
                return BadRequest();
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(string email, string token, ResetPasswordVM resetPasswordVM)
        {
            var appUser = await _userManager.FindByEmailAsync(email);
            if (!ModelState.IsValid) return View();
            await _userManager.ResetPasswordAsync(appUser, token, resetPasswordVM.Password);
            await _userManager.UpdateSecurityStampAsync(appUser);
            await _signInManager.SignInAsync(appUser, false);
            return RedirectToAction("login", "account");
        }

        public async Task<IActionResult> UpdateProfile()
        {
            if (!User.Identity.IsAuthenticated) RedirectToAction("login");
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            UserProfileUpdateVm userProfileUpdateVm = new();
            userProfileUpdateVm.FullName = user.FullName;
            userProfileUpdateVm.UserName = user.UserName;
            userProfileUpdateVm.Email = user.Email;
            return View(userProfileUpdateVm);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> UpdateProfile(UserProfileUpdateVm userProfileUpdateVm)
        {
            if (!User.Identity.IsAuthenticated) RedirectToAction("login");
            if (!ModelState.IsValid) return View(userProfileUpdateVm);
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            if (user == null) return BadRequest();
            userProfileUpdateVm.Email = user.Email;
            var existUser = _userManager.Users.Any(u => u.UserName == userProfileUpdateVm.UserName && u.Id != user.Id);
            if (existUser)
            {
                ModelState.AddModelError("UserName", "Bu username artiq sistemde var");
                return View(userProfileUpdateVm);
            }
            if (!await _userManager.CheckPasswordAsync(user, userProfileUpdateVm.Password))
            {
                ModelState.AddModelError("Password", "Sifre yalnisdir");
                return View(userProfileUpdateVm);
            }
            user.UserName = userProfileUpdateVm.UserName;
            user.FullName = userProfileUpdateVm.FullName;
            if (userProfileUpdateVm.NewPassword != null && userProfileUpdateVm.NewRePassword != null)
            {
                await _userManager.ChangePasswordAsync(user, userProfileUpdateVm.Password, userProfileUpdateVm.NewRePassword);
            }
            await _userManager.UpdateAsync(user);
            await _signInManager.SignOutAsync();
            await _signInManager.SignInAsync(user, true);
            return RedirectToAction("index", "home");
        }


        //public async Task CreateRoles()
        //{
        //    foreach (var role in Enum.GetValues(typeof(RolesEnum)))
        //    {
        //        if (!await _roleManager.RoleExistsAsync(role.ToString()))
        //        {
        //            await _roleManager.CreateAsync(new IdentityRole { Name = role.ToString() });
        //        }
        //    }
        //}
    }
}
