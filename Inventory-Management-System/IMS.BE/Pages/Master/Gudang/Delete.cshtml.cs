using IMS.BE.Services.Masters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IMS.BE.Pages.Master.Gudang
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly GudangService gudang;

        public string ErrorMessage { get; set; } = string.Empty;

        public DeleteModel(GudangService gudang)
        {
            this.gudang = gudang;
        }

        public async Task<IActionResult> OnGet(string id)
        {
            var (isValid, errorMessage) = await this.gudang.DeleteAsync(id);

            if (isValid)
            {
                return RedirectToPage("/Welcome");
            }

            ErrorMessage = errorMessage;
            return Page();
        }
    }
}
