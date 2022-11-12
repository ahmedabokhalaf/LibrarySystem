using ITI.LibSys.Models.Data;
using ITI.LibSys.Presentation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ITI.LibSys.Presentation.Controllers.UserController
{
    [Authorize(Roles ="Editor")]
    public class RoleController : Controller
    {
        RoleManager<IdentityRole> RoleManager;
        UserManager<User> UserManager;
        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            RoleManager = roleManager;
            UserManager = userManager;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(RoleModel model)
        {
            if (!ModelState.IsValid)
                return View();
            else
            {
                IdentityResult result= await RoleManager.CreateAsync(new IdentityRole { Name=model.Name });
                if (!result.Succeeded)
                {
                    result.Errors.ToList().ForEach(e => { ModelState.AddModelError("", e.Description); });
                    return View();
                }
                else
                {
                    return RedirectToAction("Register", "User");
                }
            }
        }
        [HttpGet]
        public IActionResult AddEditor()
        {
            return RedirectToAction("Register", "User");
        }

        [HttpPost]
        public async Task<IActionResult> AddEditor(UserRegisterModel model)
        {
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
                    return RedirectToAction("Index", "Home");
                }
            }
        }
    }
}
