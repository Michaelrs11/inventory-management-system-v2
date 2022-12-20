using IMS.BE.Models;
using IMS.BE.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IMS.BE.Pages.Auth
{
    [Authorize]
    public class ChangePassModel : PageModel
    {
        private readonly LoginService service;

        public ChangePassModel(LoginService service)
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
            if (string.IsNullOrEmpty(ChangePassForm.OldPassword))
            {
                ErrorMessage = "Old Password harus diisi";
                return Page();
            }
            if (ModelState.IsValid)
            {
                var isValid = await this.service.ChangePasswordAsync(false, ChangePassForm);
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
