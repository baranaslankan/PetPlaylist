using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace PetPlaylist.Controllers
{
    [Route("[controller]")]
    public class AuthController : Controller
    {
        [HttpGet("login")]
        public IActionResult Login(string returnUrl = "/")
        {
            return View();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(string email, string role, string returnUrl = "/")
        {
            // In a real app, validate user credentials from DB
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, email),
                new Claim(ClaimTypes.Role, role)
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties { IsPersistent = true };
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
            return LocalRedirect(returnUrl);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
        }

        [HttpGet("denied")]
        public IActionResult Denied() => Content("Access Denied");
    }
}
