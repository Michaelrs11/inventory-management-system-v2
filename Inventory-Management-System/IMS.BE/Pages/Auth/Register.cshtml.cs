using IMS.BE.Models;
using IMS.BE.Services;
using IMS.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace IMS.BE.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly RegisterService registerService;

        [BindProperty]
        public Register RegisterForm { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;

        public List<SelectListItem> UserRoleDropdown { get; set; }

        public RegisterModel(RegisterService registerService)
        {
            this.registerService = registerService;
        }

        public async Task OnGet()
        {
            this.UserRoleDropdown = await this.registerService.GetDropdownAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var isUserCodeExists = await this.registerService.IsUserCodeExists(RegisterForm.UserCode);

                var isEmailExists = await this.registerService.IsEmailExists(RegisterForm.Email);

                if (isUserCodeExists)
                {
                    ErrorMessage = "UserCode already Exists";
                    return Page();
                }

                if (isEmailExists)
                {
                    ErrorMessage = "Email already Exists";
                    return Page();
                }

                await this.registerService.CreateNewUserAsync(RegisterForm);
                return RedirectToPage("/Welcome");
            }
            return Page();
        }
    }
}
