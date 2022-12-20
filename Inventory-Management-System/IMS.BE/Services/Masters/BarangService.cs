using IMS.BE.Commons.Services;
using IMS.BE.Models.Masters;
using IMS.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace IMS.BE.Services.Masters
{
    public class BarangService
    {
        private readonly IMSDBContext db;
        private readonly UserIdentityService userIdentityService;

        public BarangService(IMSDBContext db, UserIdentityService userIdentityService)
        {
            this.db = db;
            this.userIdentityService = userIdentityService;
        }

        /// <summary>
        /// Get Selected Barang for Edit
        /// </summary>
        /// <param name="skuId"></param>
        /// <returns></returns>
        public async Task<UpdateBarang> GetSelectedBarangAsync(string skuId)
        {
            var toLowerSkuId = skuId.ToLower();

            var selectedBarang = await this.db.MasterBarangs
                .Where(Q => Q.SKUID.ToLower() == toLowerSkuId)
                .Select(Q => new UpdateBarang
                {
                    SkuId = Q.SKUID,
                    Name = Q.Name,
                    Kategori = Q.KategoriCodeNavigation.Name
                }).FirstOrDefaultAsync();

            return selectedBarang;
        }

        /// <summary>
        /// Get List barang
        /// </summary>
        /// <param name="param"></param>
        /// <param name="currentPage"></param>
        /// <returns></returns>
        public async Task<PaginatedList<CreateBarang>> GetBarangAsync(string? param, int? currentPage)
        {
            var tolowerParam = param?.ToLower();

            if (currentPage.HasValue == false || currentPage.Value < 1)
            {
                currentPage = 1;
            }

            var query = from mb in this.db.MasterBarangs
                        where
                               // If null, don't check the condition
                               tolowerParam == null || tolowerParam.Contains(mb.SKUID.ToLower())
                               ||
                               tolowerParam.Contains(mb.Name.ToLower())
                        orderby mb.UpdatedAt descending
                        select new CreateBarang
                        {
                            SkuId = mb.SKUID,
                            Name = mb.Name,
                            Kategori = mb.KategoriCodeNavigation.Name
                        };

            var pageSize = 10;

            var requests = await PaginatedList<CreateBarang>.CreateAsync(query, currentPage.Value, pageSize);

            return requests;
        }

        /// <summary>
        /// Insert Barang
        /// </summary>
        /// <param name="insert"></param>
        /// <returns></returns>
        public async Task InsertAsync(CreateBarang insert)
        {
            var userLogin = userIdentityService.UserCode ?? "SYSTEM";

            var datetimeOffset = DateTime.UtcNow;

            this.db.MasterBarangs.Add(new MasterBarang
            {
                SKUID = insert.SkuId.ToUpper(),
                Name = insert.Name.ToUpper(),
                KategoriCode = insert.Kategori.ToUpper(),
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
        public async Task UpdateAsync(UpdateBarang update, string skuId)
        {
            var tolowerSkuId = skuId.ToLower();

            var userLogin = userIdentityService.UserCode ?? "SYSTEM";

            var datetimeOffset = DateTime.UtcNow;

            var updateBarang = await this.db.MasterBarangs
                .Where(Q => Q.SKUID.ToLower() == tolowerSkuId)
                .FirstOrDefaultAsync();

            updateBarang!.Name = update.Name.ToUpper();
            updateBarang.UpdatedAt = datetimeOffset;
            updateBarang.UpdatedBy = userLogin;

            await this.db.SaveChangesAsync();
        }

        public async Task<(bool, string)> DeleteAsync(string skuId)
        {
            var tolowerSkuId = skuId.ToLower();

            try
            {
                var deleteData = await this.db.MasterBarangs
                    .FirstOrDefaultAsync(Q => Q.SKUID.ToLower() == tolowerSkuId);

                this.db.MasterBarangs.Remove(deleteData);
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
        /// <param name="skuCode"></param>
        /// <returns></returns>
        public async Task<bool> IsSKUCodeExists(string skuCode)
        {
            var tolowerSkuCode = skuCode.ToLower();

            var isExists = await this.db.MasterBarangs
                .AnyAsync(Q => Q.SKUID.ToLower() == tolowerSkuCode);

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
            var tolowerSkuId = id.ToLower();

            var isExists = await this.db.MasterBarangs
               .AnyAsync(Q => Q.Name.ToLower() == tolowerName && Q.SKUID != tolowerSkuId);

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

            var isExists = await this.db.MasterBarangs
                .AnyAsync(Q => Q.Name.ToLower() == tolowerName);

            return isExists;
        }
    }
}
