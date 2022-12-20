using IMS.BE.Commons.Models;
using IMS.BE.Models.Masters;
using IMS.BE.Services.Masters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IMS.BE.Pages.Master.Kategori
{
    [Authorize]
    public class ViewModel : PageModel
    {
        private readonly CategoryService categoryService;

        public int? Page { get; set; }
        public Pager Pager { set; get; }
        public int TotalPage { get; set; }
        [BindProperty(SupportsGet = true)]
        public string Parameter { get; set; }
        public List<CreateKategori> CRUDKategoris { get; set; }

        public ViewModel(CategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        public async Task<IActionResult> OnGet(int? p,
            string? param)
        {
            if (!string.IsNullOrEmpty(Parameter))
            {
                param = Parameter;
            }

            var requests = await this.categoryService.GetCategoryAsync(param: param, currentPage: p);

            CRUDKategoris = requests;

            TotalPage = requests.TotalPage;
            Page = requests.PageIndex;
            var pager = new Pager(TotalPage, (int)Page);
            Pager = pager;
            return Page();
        }
    }
}
