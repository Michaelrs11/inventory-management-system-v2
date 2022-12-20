using IMS.BE.Services.Masters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IMS.BE.Pages.Master.Kategori
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly CategoryService categoryService;

        public string ErrorMessage { get; set; } = string.Empty;

        public DeleteModel(CategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        public async Task<IActionResult> OnGet(string id)
        {
            var (isValid, errorMessage) = await this.categoryService.DeleteAsync(id);

            if (isValid)
            {
                return RedirectToPage("/Welcome");
            }

            ErrorMessage = errorMessage;
            return Page();
        }
    }
}
