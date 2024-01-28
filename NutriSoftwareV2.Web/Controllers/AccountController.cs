using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI.Common;
using NutriSoftwareV2.Web.Identity;
using Org.BouncyCastle.Asn1.Crmf;
using System.Threading.Tasks;

namespace NutriSoftwareV2.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel pLogin) 
        {
            if (ModelState.IsValid) 
            {

                var user = new ApplicationUser
                {
                    UserName = pLogin.Email,
                    Email = pLogin.Email,
                    UsuarioSistema = pLogin.UsuarioSistema,
                    
                    
                };
                var result = await userManager.CreateAsync(user, pLogin.PassWord);

                if (result.Succeeded) 
                {
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Login");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult>Login(Login pLogin)
        {
            if (ModelState.IsValid)
            {

                //pLogin.PassWord = "AQAAAAEAACcQAAAAEFfwVUGnRtnJ+Rj/sIi5GA7G3qyMN2r9EEwtpQXVfoO/WAb8QmVvN5sFoX64ClNzWQ==";
                var result = await signInManager.PasswordSignInAsync(pLogin.Email, pLogin.PassWord, pLogin.Relembrar, true);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Paciente");
                }
                if (result.RequiresTwoFactor) 
                {
                    var x = result.RequiresTwoFactor;
                }
                if (result.IsLockedOut) 
                { var t = result.IsLockedOut; }
                
                ModelState.AddModelError(string.Empty, "Login ou Senha inválido");
            }
            return View(pLogin);
        }

        [HttpPost]
        public async Task<ActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}
