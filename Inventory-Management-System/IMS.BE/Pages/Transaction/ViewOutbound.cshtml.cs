using IMS.BE.Commons.Models;
using IMS.BE.Commons.Services;
using IMS.BE.Models.Transactions;
using IMS.BE.Services.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IMS.BE.Pages.Transaction
{
    [Authorize]
    public class ViewOutboundModel : PageModel
    {
        private readonly InOutBoundService service;
        private readonly DropdownService dropdown;

        public ViewOutboundModel(InOutBoundService service, DropdownService dropdown)
        {
            this.service = service;
            this.dropdown = dropdown;
        }
        public int? Page { get; set; }
        public Pager Pager { set; get; }
        public int TotalPage { get; set; }
        public string? GudangCode { get; set; }
        public List<ViewOutbound> RequestList { set; get; }
        [TempData]
        public string ErrorMessage { get; set; }
        public DateTime? DateFrom { set; get; }
        public DateTime? DateTo { set; get; }

        public Dictionary<string, string> GudangCodeDropdown { get; set; }

        public string GetFormatedDate(DateTime? date)
        {
            if (date.HasValue)
            {
                return date.Value.ToString("yyyy-MM-dd");
            }

            return string.Empty;
        }

        public async Task OnGet(string? gudangCode,
            DateTime? dateFrom,
            DateTime? dateTo,
            int? p)
        {
            GudangCodeDropdown = await this.dropdown.GetGudangDict();

            if (dateFrom > dateTo)
            {
                dateTo = null;
            }

            var requestList = await service.GetOutboundTransaction(gudangCode: gudangCode,
                dateFrom: dateFrom,
                dateTo: dateTo, currentPage: p);

            RequestList = requestList.Item1;
            GudangCode = gudangCode;
            DateFrom = dateFrom;
            DateTo = dateTo;

            TotalPage = (int)Math.Ceiling(requestList.Item2.Value / (double)10);
            Page = requestList.Item1.Skip((requestList.Item2.Value - 1) * 10)
                .Take(10).Count();
            var pager = new Pager(TotalPage, (int)Page);
            Pager = pager;
        }
    }
}
