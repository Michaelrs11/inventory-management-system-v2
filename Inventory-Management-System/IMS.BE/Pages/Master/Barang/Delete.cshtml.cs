using IMS.BE.Services.Masters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IMS.BE.Pages.Master
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly BarangService barangService;

        public string ErrorMessage { get; set; } = string.Empty;

        public DeleteModel(BarangService barangService)
        {
            this.barangService = barangService;
        }

        public async Task<IActionResult> OnGet(string id)
        {
            var (isValid, errorMessage) = await this.barangService.DeleteAsync(id);

            if (isValid)
            {
                return RedirectToPage("/Welcome");
            }

            ErrorMessage = errorMessage;
            return Page();
        }
    }
}
