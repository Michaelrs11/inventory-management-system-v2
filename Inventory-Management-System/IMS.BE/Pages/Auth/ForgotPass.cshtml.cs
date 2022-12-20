using IMS.BE.Models;
using IMS.BE.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IMS.BE.Pages.Auth
{
    public class ForgotPassModel : PageModel
    {
        private readonly LoginService service;

        public ForgotPassModel(LoginService service)
        {
            this.service = service;
        }
        [BindProperty]
        public ChangePassword ChangePassForm { get; set; }
        [TempData]
        public string ErrorMessage { get; set; } = string.Empty;
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(ChangePassForm.UserCode))
            {
                ErrorMessage = "User code harus diisi";
                return Page();
            }
            if (ModelState.IsValid)
            {
                var isValid = await this.service.ChangePasswordAsync(true, ChangePassForm);
                if (!isValid)
                {
                    ErrorMessage = "Old Password Invalid";
                    return Page();
                }
                ErrorMessage = string.Empty;
                return RedirectToPage("/Welcome");
            }
            return Page();
        }
    }
}
