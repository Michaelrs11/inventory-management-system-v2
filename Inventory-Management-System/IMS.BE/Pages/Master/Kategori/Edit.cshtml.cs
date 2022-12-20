using IMS.BE.Models.Masters;
using IMS.BE.Services.Masters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IMS.BE.Pages.Master.Kategori
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly CategoryService categoryService;

        [BindProperty]
        public UpdateKategori CategoryModel { get; set; }

        public string ErrorMessage { get; set; } = string.Empty;

        public EditModel(CategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        public async Task<IActionResult> OnGet(string id)
        {
            CategoryModel = await this.categoryService.GetSelectedKategoriAsync(id);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            CategoryModel.CategoryId = id;

            if (ModelState.IsValid)
            {
                var isNameExists = await this.categoryService.IsUpdateNameExist(CategoryModel.Name, id);

                if (isNameExists)
                {
                    ErrorMessage = "Name already Exists";
                    return Page();
                }

                await this.categoryService.UpdateAsync(CategoryModel, id);
                ErrorMessage = string.Empty;
                return RedirectToPage("/Welcome");
            }

            return Page();
        }
    }
}
