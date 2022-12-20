using IMS.BE.Commons.Services;
using IMS.BE.Models.Masters;
using IMS.BE.Services.Masters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IMS.BE.Pages.Master.Gudang
{
    [Authorize]
    public class AddModel : PageModel
    {
        private readonly GudangService gudangService;
        private readonly DropdownService dropdown;

        public AddModel(GudangService gudangService, DropdownService dropdown)
        {
            this.gudangService = gudangService;
            this.dropdown = dropdown;
        }

        [BindProperty]
        public CreateGudang GudangModel { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public List<SelectListItem> OutletCodeDropdown { get; set; }

        public async Task OnGet()
        {
            this.OutletCodeDropdown = await this.dropdown.GetOutletDropdownAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var isGudangCodeExists = await this.gudangService.IsGudangCodeExists(GudangModel.GudangCode);

                var isNameExists = await this.gudangService.IsNameExists(GudangModel.Name);

                if (isGudangCodeExists)
                {
                    ErrorMessage = "Gudang Code already Exists";
                    return Page();
                }

                if (isNameExists)
                {
                    ErrorMessage = "Name already Exists";
                    return Page();
                }

                await this.gudangService.InsertAsync(GudangModel);
                ErrorMessage = string.Empty;
                return RedirectToPage("/Welcome");
            }

            return Page();
        }
    }
}
