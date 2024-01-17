using MaximApp.Context;
using MaximApp.Helpers;
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
                   
                    ModelState.AddModelError(string.Empty, "the password requirements are at least one uppercase one digit and one special chracaters");
                }           
            }
            await _userManager.AddToRoleAsync(user, UserRole.Admin.ToString());
            return RedirectToAction("Login");
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginvm)
        {
            var user = await _userManager.FindByEmailAsync(loginvm.EmailOrUsername);
            if(user == null)
            {
                user = await _userManager.FindByNameAsync(loginvm.EmailOrUsername);
                if(user is null)
                {
                    ModelState.AddModelError("", " username or password is incorrect");
                    return View();

                }
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginvm.Password, false);

            if(!result.Succeeded)
            {
                ModelState.AddModelError("", "username or password is incorrect");
                return View();
            }
            if(result.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "try it after a few minutes");
            }

            await _signInManager.SignInAsync(user, false);
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> CreateRole()
        {
            foreach (UserRole item in Enum.GetValues(typeof(UserRole)))
            {

               if(await _roleManager.FindByNameAsync(item.ToString()) == null)                      
               {
                    await _roleManager.CreateAsync(new IdentityRole()
                    {
                        Name = item.ToString(),
                    });
                }
            }

            return RedirectToAction("Index", "Home");   
           
        }

    }
}
