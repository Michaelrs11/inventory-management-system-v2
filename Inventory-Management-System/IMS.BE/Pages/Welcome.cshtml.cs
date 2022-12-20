using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IMS.BE.Pages
{
    [Authorize]
    public class WelcomeModel : PageModel
    {
        private readonly ILogger<WelcomeModel> logger;

        public WelcomeModel(ILogger<WelcomeModel> logger)
        {
            this.logger = logger;
        }
        public void OnGet()
        {
        }
    }
}
