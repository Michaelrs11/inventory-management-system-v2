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
    public class OutboundModel : PageModel
    {
        private readonly InOutBoundService service;
        private readonly DropdownService dropdown;

        [BindProperty]
        public OutbondTransaction OutboundData { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public List<SelectListItem> GudangCodeDropdown { get; set; }
        public List<SelectListItem> BarangCodeDropdown { get; set; }

        public OutboundModel(InOutBoundService service, DropdownService dropdown)
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
                var isSuccess = await this.service.InsertOutbound(OutboundData);

                if (isSuccess)
                {
                    ErrorMessage = string.Empty;
                    return RedirectToPage("/Welcome");
                }
                else
                {
                    ErrorMessage = "Barang tersebut belum pernah dilakukan inbound";
                    return Page();
                }
            }

            return Page();
        }
    }
}
