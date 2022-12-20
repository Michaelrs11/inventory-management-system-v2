using IMS.BE.Commons.Services;
using IMS.BE.Models.Masters;
using IMS.BE.Services.Masters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IMS.BE.Pages.Master
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly BarangService barangService;
        private readonly DropdownService dropdown;

        [BindProperty]
        public UpdateBarang BarangModel { get; set; }

        public string ErrorMessage { get; set; } = string.Empty;

        public List<SelectListItem> OutletCodeDropdown { get; set; }

        public EditModel(BarangService barangService, DropdownService dropdown)
        {
            this.barangService = barangService;
            this.dropdown = dropdown;
        }

        public async Task<IActionResult> OnGet(string id)
        {
            this.OutletCodeDropdown = await this.dropdown.GetOutletDropdownAsync();
            BarangModel = await this.barangService.GetSelectedBarangAsync(id);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            BarangModel.SkuId = id;

            if (ModelState.IsValid)
            {
                var isNameExists = await this.barangService.IsUpdateNameExist(BarangModel.Name, id);

                if (isNameExists)
                {
                    ErrorMessage = "Name already Exists";
                    return Page();
                }

                await this.barangService.UpdateAsync(BarangModel, id);
                ErrorMessage = string.Empty;
                return RedirectToPage("/Welcome");
            }

            return Page();
        }
    }
}
