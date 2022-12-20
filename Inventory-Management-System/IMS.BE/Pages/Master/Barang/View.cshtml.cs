using IMS.BE.Commons.Models;
using IMS.BE.Models.Masters;
using IMS.BE.Services.Masters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IMS.BE.Pages.Master
{
    [Authorize]
    public class ViewModel : PageModel
    {
        private readonly BarangService barangService;

        public int? Page { get; set; }
        public Pager Pager { set; get; }
        public int TotalPage { get; set; }
        [BindProperty(SupportsGet = true)]
        public string Parameter { get; set; }
        public List<CreateBarang> CRUDBarangs { get; set; }

        public ViewModel(BarangService barangService)
        {
            this.barangService = barangService;
        }

        public async Task<IActionResult> OnGet(int? p,
            string? param)
        {
            if (!string.IsNullOrEmpty(Parameter))
            {
                param = Parameter;
            }

            var requests = await this.barangService.GetBarangAsync(param: param, currentPage: p);

            CRUDBarangs = requests;

            TotalPage = requests.TotalPage;
            Page = requests.PageIndex;
            var pager = new Pager(TotalPage, (int)Page);
            Pager = pager;
            return Page();
        }
    }
}
