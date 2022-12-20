using IMS.BE.Models.Masters;
using IMS.BE.Services.Masters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IMS.BE.Pages.Master.Kategori
{
    [Authorize]
    public class AddModel : PageModel
    {
        private readonly CategoryService categoryService;

        public AddModel(CategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        [BindProperty]
        public CreateKategori CategoryModel { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var isSkuCodeExists = await this.categoryService.IsKategoriCodeExists(CategoryModel.CategoryId);

                var isNameExists = await this.categoryService.IsNameExists(CategoryModel.Name);

                if (isSkuCodeExists)
                {
                    ErrorMessage = "UserCode already Exists";
                    return Page();
                }

                if (isNameExists)
                {
                    ErrorMessage = "Name already Exists";
                    return Page();
                }

                await this.categoryService.InsertAsync(CategoryModel);
                ErrorMessage = string.Empty;
                return RedirectToPage("/Welcome");
            }

            return Page();
        }
    }
}
