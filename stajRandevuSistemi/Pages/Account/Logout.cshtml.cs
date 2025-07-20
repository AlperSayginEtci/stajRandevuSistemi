using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace stajRandevuSistemi.Pages.Account
{
    public class LogoutModel : PageModel
    {
       
        public async Task<IActionResult> OnGetAsync()
        {
            return await SignOutAndRedirect();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            return await SignOutAndRedirect();
        }

        private async Task<IActionResult> SignOutAndRedirect()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

           
            return RedirectToPage("/Account/Login");
        }
    }
}