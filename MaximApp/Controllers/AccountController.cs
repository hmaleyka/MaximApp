using MaximApp.Context;
using MaximApp.Models;
using MaximApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MaximApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(AppDbContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM register)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            AppUser user = new AppUser()
            {
                Name = register.Name,
                Email = register.Email,
                Surname = register.Surname,
                UserName = register.Username
            };

            var result = await _userManager.CreateAsync(user , register.Password);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }           
            }
            //await _userManager.AddToRoleAsync(user, )
            return RedirectToAction("Login");
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginvm)
        {
            AppUser user = await _userManager.FindByEmailAsync(loginvm.EmailOrUsername);
            if(user == null)
            {
                user = await _userManager.FindByNameAsync(loginvm.EmailOrUsername);
                if(user is null)
                {
                    throw new Exception("the username-email or password is incorrect");
                }
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginvm.Password, false);

            if(!result.Succeeded)
            {
                throw new Exception("the username-email or password is incorrect");
            }
            if(result.IsLockedOut)
            {
                throw new Exception("try it again after a few minutes");
            }

            await _signInManager.SignInAsync(user, false);
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

    }
}
