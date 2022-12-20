using IMS.BE.Commons.Services;
using IMS.BE.Models.Masters;
using IMS.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace IMS.BE.Services.Masters
{
    public class GudangService
    {
        private readonly IMSDBContext db;
        private readonly UserIdentityService userIdentityService;

        public GudangService(IMSDBContext db, UserIdentityService userIdentityService)
        {
            this.db = db;
            this.userIdentityService = userIdentityService;
        }

        /// <summary>
        /// Get Selected Gudang for Edit
        /// </summary>
        /// <param name="skuId"></param>
        /// <returns></returns>
        public async Task<UpdateGudang> GetSelectedGudangAsync(string gudangCode)
        {
            var toLowerGudangCode = gudangCode.ToLower();

            var selectedBarang = await (from mg in this.db.MasterGudangs
                                        join o in this.db.Outlets on mg.OutletCode equals o.OutletCode
                                        where mg.GudangCode.ToLower() == toLowerGudangCode
                                        select new UpdateGudang
                                        {
                                            GudangCode = mg.GudangCode,
                                            Name = mg.Name,
                                            Outlet = o.Name
                                        })
                                        .FirstOrDefaultAsync();

            return selectedBarang!;
        }

        /// <summary>
        /// Get List Gudang
        /// </summary>
        /// <param name="param"></param>
        /// <param name="currentPage"></param>
        /// <returns></returns>
        public async Task<PaginatedList<CreateGudang>> GetBarangAsync(string? param, int? currentPage)
        {
            var tolowerParam = param?.ToLower();

            if (currentPage.HasValue == false || currentPage.Value < 1)
            {
                currentPage = 1;
            }

            var query = from mg in this.db.MasterGudangs
                        join o in this.db.Outlets on mg.OutletCode equals o.OutletCode
                        where
                               // If null, don't check the condition
                               tolowerParam == null || tolowerParam.Contains(mg.GudangCode.ToLower())
                               ||
                               tolowerParam.Contains(mg.Name.ToLower())
                               ||
                               tolowerParam.Contains(o.Name.ToLower())
                        orderby mg.UpdatedAt descending
                        select new CreateGudang
                        {
                            GudangCode = mg.GudangCode,
                            Name = mg.Name,
                            Outlet = o.Name
                        };

            var pageSize = 10;

            var requests = await PaginatedList<CreateGudang>.CreateAsync(query, currentPage.Value, pageSize);

            return requests;
        }


        /// <summary>
        /// Insert Barang
        /// </summary>
        /// <param name="insert"></param>
        /// <returns></returns>
        public async Task InsertAsync(CreateGudang insert)
        {
            var userLogin = userIdentityService.UserCode ?? "SYSTEM";

            var datetimeOffset = DateTime.UtcNow;

            this.db.MasterGudangs.Add(new MasterGudang
            {
                GudangCode = insert.GudangCode.ToUpper(),
                Name = insert.Name.ToUpper(),
                OutletCode = insert.Outlet.ToUpper(),
                CreatedAt = datetimeOffset,
                UpdatedAt = datetimeOffset,
                CreatedBy = userLogin,
                UpdatedBy = userLogin
            });

            await this.db.SaveChangesAsync();
        }


        /// <summary>
        /// Update Data
        /// </summary>
        /// <param name="update"></param>
        /// <param name="skuId"></param>
        /// <returns></returns>
        public async Task UpdateAsync(UpdateGudang update, string gudangCode)
        {
            var tolowerGudangCode = gudangCode.ToLower();

            var userLogin = userIdentityService.UserCode ?? "SYSTEM";

            var datetimeOffset = DateTime.UtcNow;

            var updateBarang = await this.db.MasterGudangs
                .Where(Q => Q.GudangCode.ToLower() == tolowerGudangCode)
                .FirstOrDefaultAsync();

            updateBarang!.Name = update.Name.ToUpper();
            updateBarang.UpdatedAt = datetimeOffset;
            updateBarang.UpdatedBy = userLogin;

            await this.db.SaveChangesAsync();
        }

        /// <summary>
        /// Delete data
        /// </summary>
        /// <param name="gudangCode"></param>
        /// <returns></returns>
        public async Task<(bool, string)> DeleteAsync(string gudangCode)
        {
            var tolowerGudangCode = gudangCode.ToLower();

            try
            {
                var deleteData = await this.db.MasterGudangs
                    .FirstOrDefaultAsync(Q => Q.GudangCode.ToLower() == tolowerGudangCode);

                this.db.MasterGudangs.Remove(deleteData!);
                await this.db.SaveChangesAsync();
                return (true, string.Empty);
            }
            catch
            {
                return (false, "Terdapat data pada table lain");
            }
        }

        /// <summary>
        /// Check SKU Code Exists
        /// </summary>
        /// <param name="gudangCode"></param>
        /// <returns></returns>
        public async Task<bool> IsGudangCodeExists(string gudangCode)
        {
            var tolowerGudangCode = gudangCode.ToLower();

            var isExists = await this.db.MasterGudangs
                .AnyAsync(Q => Q.GudangCode.ToLower() == tolowerGudangCode);

            return isExists;
        }

        /// <summary>
        /// Check Update Name exists
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> IsUpdateNameExist(string name, string id)
        {
            var tolowerName = name.ToLower();
            var tolowerGudangCode = id.ToLower();

            var isExists = await this.db.MasterGudangs
               .AnyAsync(Q => Q.Name.ToLower() == tolowerName && Q.GudangCode != tolowerGudangCode);

            return isExists;
        }

        /// <summary>
        /// Check Name Exists
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<bool> IsNameExists(string name)
        {
            var tolowerName = name.ToLower();

            var isExists = await this.db.MasterGudangs
                .AnyAsync(Q => Q.Name.ToLower() == tolowerName);

            return isExists;
        }
    }
}
