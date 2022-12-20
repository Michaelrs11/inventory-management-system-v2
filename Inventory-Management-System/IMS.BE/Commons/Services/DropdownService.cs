using IMS.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace IMS.BE.Commons.Services
{
    public class DropdownService
    {
        private readonly IMSDBContext db;

        public DropdownService(IMSDBContext db)
        {
            this.db = db;
        }

        public async Task<Dictionary<string, string>> GetGudangDict()
        {
            var masterGudangs = await db.MasterGudangs
                .AsNoTracking()
                .ToDictionaryAsync(Q => Q.GudangCode, Q => Q.Name);

            return masterGudangs;
        }

        /// <summary>
        /// Get Outlet Dropdown
        /// </summary>
        /// <returns></returns>
        public async Task<List<SelectListItem>> GetKategoriDropdownAsync()
        {
            var dropdowns = await this.db.MasterKategoris
                 .Select(Q => new SelectListItem
                 {
                     Value = Q.KategoriCode,
                     Text = Q.Name
                 })
                 .ToListAsync();

            return dropdowns;
        }

        /// <summary>
        /// Get Outlet Dropdown
        /// </summary>
        /// <returns></returns>
        public async Task<List<SelectListItem>> GetBarangDropdownAsync()
        {
            var dropdowns = await this.db.MasterBarangs
                 .Select(Q => new SelectListItem
                 {
                     Value = Q.SKUID,
                     Text = Q.Name
                 })
                 .ToListAsync();

            return dropdowns;
        }

        public async Task<List<SelectListItem>> GetGudangDropdownAsync()
        {
            var dropdowns = await this.db.MasterGudangs
                 .Select(Q => new SelectListItem
                 {
                     Value = Q.GudangCode,
                     Text = Q.Name
                 })
                 .ToListAsync();

            return dropdowns;
        }

        /// <summary>
        /// Get Outlet Dropdown
        /// </summary>
        /// <returns></returns>
        public async Task<List<SelectListItem>> GetOutletDropdownAsync()
        {
            var dropdowns = await this.db.Outlets
                 .Select(Q => new SelectListItem
                 {
                     Value = Q.OutletCode.ToString(),
                     Text = Q.Name
                 })
                 .ToListAsync();

            return dropdowns;
        }

        /// <summary>
        /// Get Roles Dropdown
        /// </summary>
        /// <returns></returns>
        public async Task<List<SelectListItem>> GetUserRoleDropdownAsync()
        {
            var dropdowns = await this.db.UserRoleEnums
                 .Select(Q => new SelectListItem
                 {
                     Value = Q.UserRoleEnumId.ToString(),
                     Text = Q.Name
                 })
                 .ToListAsync();

            return dropdowns;
        }
    }
}
