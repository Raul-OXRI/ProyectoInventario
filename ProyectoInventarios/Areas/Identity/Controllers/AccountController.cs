using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ProyectoInventarios.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> Register(string email, string password, string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            var user = new IdentityUser { UserName = email, Email = email };
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                // Asignar automáticamente el rol "User"
                await _userManager.AddToRoleAsync(user, "User");

                await _signInManager.SignInAsync(user, isPersistent: false);
                return Redirect(returnUrl);
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View();
        }
    }
}
