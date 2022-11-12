using Castle.Core.Internal;
using ITI.LibSys.Models.Data;
using ITI.LibSys.Presentation.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Mail;
using System.Net;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace ITI.LibSys.Presentation
{

    public class UserController : Controller
    {
        private UserManager<User> UserManager;
        private SignInManager<User> SignInManager;
        private RoleManager<IdentityRole> RoleManager;
        public UserController(UserManager<User> userManager,
            SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            RoleManager = roleManager;
        }

        #region Registration
        [HttpGet]
        public IActionResult Register()
        {
            ViewBag.Roles = RoleManager.Roles
                .Select(r => new SelectListItem(r.Name, r.Name));
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterModel model)
        {
            model.Role = "Viewer";
            if (!ModelState.IsValid)
                return View();
            else
            {
                User user = new User()
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                };
                //using CreateAsync is better than Add, because in this case CreateAsync will hash the password
                IdentityResult result = await UserManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                {
                    result.Errors.ToList().ForEach(e =>
                    {
                        ModelState.AddModelError("", e.Description);
                    });
                    return View();
                }
                else
                {
                    await UserManager.AddToRoleAsync(user, model.Role);
                    string token = await UserManager.GenerateEmailConfirmationTokenAsync(user);
                    var client = new SmtpClient("smtp.mailtrap.io", 2525)
                    {
                        Credentials = new NetworkCredential("56bdc16a453b68", "5fe941e73b0acf"),
                        EnableSsl = true
                    };
                    client.Send("from@example.com", "to@example.com", "Hello world", "testbody");
                    string body = $"Please Click Here For Verification: <a href='https://localhost:59750/User/ConfirmEmail?uid={user.Id}&token={token}'>Verify</a>";
                    client.Send("ahmd@iti.edu", model.Email, "Email Confirmation", body);
                    return View("EmailSent", user.Email);
                }
            }
        }
        #endregion

        #region Login
        [HttpGet]
        public IActionResult Login(string ReturnUrl = null)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(UserLoginModel model)
        {
            if (!ModelState.IsValid)
                return View();
            else
            {
                //Validate the username with its own password
                SignInResult result = await SignInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, true);
                if (result.IsNotAllowed == true)
                {
                    ModelState.AddModelError("", "Invalid username or password");
                    return View();
                }
                else if(result.IsLockedOut){
                    ModelState.AddModelError("",
                        "Sorry, you were locked out because your trying login 2 times. Try again after 20 minutes");
                    return View();
                }
                else
                {
                    if (!string.IsNullOrEmpty(model.ReturnUrl))
                    {
                        return LocalRedirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
        }
        #endregion

        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await SignInManager.SignOutAsync();
            return RedirectToAction("Login", "User");
        }
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            if(!ModelState.IsValid) return View();
            else
            {
                var user = await UserManager.GetUserAsync(User);
                await UserManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                return RedirectToAction("LogOut");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string uid, string token)
        {
            token = token.Replace(' ', '+');
            var user = await UserManager.FindByIdAsync(uid);
            await UserManager.ConfirmEmailAsync(user, token);
            return RedirectToAction("Login");
        }
    }
}
