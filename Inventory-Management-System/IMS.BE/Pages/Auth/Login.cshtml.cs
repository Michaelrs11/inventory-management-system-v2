using IMS.BE.Commons.Constants;
using IMS.BE.Models;
using IMS.BE.Services;
using IMS.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IMS.BE.Pages
{
    public class LoginModel : PageModel
    {
        private readonly LoginService loginService;

        [BindProperty]
        public Login LoginForm { get; set; }

        [TempData]
        public string ErrorMessage { get; set; } = string.Empty;
        public LoginModel(LoginService loginService)
        {
            this.loginService = loginService;
        }

        public IActionResult OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl)
        {
            if (LoginForm.Username != null && LoginForm.Password != null)
            {
                var claimsPrincipal = await loginService.Login(Clients.WebApp, LoginForm.Username, LoginForm.Password);

                if (claimsPrincipal == null)
                {
                    ErrorMessage = "Username or Password invalid";

                    return Page();
                }

                await HttpContext.SignInAsync(Clients.WebApp, claimsPrincipal);

                if (!string.IsNullOrEmpty(returnUrl))
                {
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                }

                ErrorMessage = string.Empty;

                return Redirect("/Welcome");
            } 

            return Page();
        }
    }
}
