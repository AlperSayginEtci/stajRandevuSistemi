using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace stajRandevuSistemi.Pages.Admin
{
    [Authorize(Roles = "Admin")]

    public class YonetimModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
