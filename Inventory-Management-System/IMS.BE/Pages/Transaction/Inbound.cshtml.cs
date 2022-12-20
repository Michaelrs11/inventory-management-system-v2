using IMS.BE.Commons.Services;
using IMS.BE.Models.Transactions;
using IMS.BE.Services.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IMS.BE.Pages.Transaction
{
    [Authorize]
    public class InboundModel : PageModel
    {
        private readonly InOutBoundService service;
        private readonly DropdownService dropdown;

        [BindProperty]
        public InboundTransaction InboundData { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public List<SelectListItem> GudangCodeDropdown { get; set; }
        public List<SelectListItem> BarangCodeDropdown { get; set; }

        public InboundModel(InOutBoundService service, DropdownService dropdown)
        {
            this.service = service;
            this.dropdown = dropdown;
        }

        public async Task OnGet()
        {
            this.GudangCodeDropdown = await this.dropdown.GetGudangDropdownAsync();
            this.BarangCodeDropdown = await this.dropdown.GetBarangDropdownAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                await this.service.InsertInbound(InboundData);
                return RedirectToPage("/Welcome");
            }

            return Page();
        }
    }
}
