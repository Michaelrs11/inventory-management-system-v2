using IMS.BE.Commons.Services;
using IMS.BE.Models.Masters;
using IMS.Entities;
using Microsoft.EntityFrameworkCore;

namespace IMS.BE.Services.Masters
{
    public class CategoryService
    {
        private readonly IMSDBContext db;
        private readonly UserIdentityService userIdentityService;

        public CategoryService(IMSDBContext iMSDBContext, UserIdentityService userIdentityService)
        {
            this.db = iMSDBContext;
            this.userIdentityService = userIdentityService;
        }

        /// <summary>
        /// Get Selected Barang for Edit
        /// </summary>
        /// <param name="skuId"></param>
        /// <returns></returns>
        public async Task<UpdateKategori> GetSelectedKategoriAsync(string categoryId)
        {
            var toLowerCategoryId = categoryId.ToLower();

            var selectedBarang = await this.db.MasterKategoris
                .Where(Q => Q.KategoriCode.ToLower() == toLowerCategoryId)
                .Select(Q => new UpdateKategori
                {
                    CategoryId = Q.KategoriCode,
                    Name = Q.Name
                }).FirstOrDefaultAsync();

            return selectedBarang;
        }

        /// <summary>
        /// Get List barang
        /// </summary>
        /// <param name="param"></param>
        /// <param name="currentPage"></param>
        /// <returns></returns>
        public async Task<PaginatedList<CreateKategori>> GetCategoryAsync(string? param, int? currentPage)
        {
            var tolowerParam = param?.ToLower();

            if (currentPage.HasValue == false || currentPage.Value < 1)
            {
                currentPage = 1;
            }

            var query = from mb in this.db.MasterKategoris
                        where
                               // If null, don't check the condition
                               tolowerParam == null || tolowerParam.Contains(mb.KategoriCode.ToLower())
                               ||
                               tolowerParam.Contains(mb.Name.ToLower())
                        orderby mb.UpdatedAt descending
                        select new CreateKategori
                        {
                            CategoryId = mb.KategoriCode,
                            Name = mb.Name
                        };

            var pageSize = 10;

            var requests = await PaginatedList<CreateKategori>.CreateAsync(query, currentPage.Value, pageSize);

            return requests;
        }

        /// <summary>
        /// Insert Barang
        /// </summary>
        /// <param name="insert"></param>
        /// <returns></returns>
        public async Task InsertAsync(CreateKategori insert)
        {
            var userLogin = userIdentityService.UserCode ?? "SYSTEM";

            var datetimeOffset = DateTime.UtcNow;

            this.db.MasterKategoris.Add(new MasterKategori
            {
                KategoriCode = insert.CategoryId.ToUpper(),
                Name = insert.Name.ToUpper(),
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
        public async Task UpdateAsync(UpdateKategori update, string categoryId)
        {
            var tolowerCategoryId = categoryId.ToLower();

            var userLogin = userIdentityService.UserCode ?? "SYSTEM";

            var datetimeOffset = DateTime.UtcNow;

            var updateBarang = await this.db.MasterKategoris
                .Where(Q => Q.KategoriCode.ToLower() == tolowerCategoryId)
                .FirstOrDefaultAsync();

            updateBarang!.Name = update.Name.ToUpper();
            updateBarang.UpdatedAt = datetimeOffset;
            updateBarang.UpdatedBy = userLogin;

            await this.db.SaveChangesAsync();
        }

        public async Task<(bool, string)> DeleteAsync(string categoryId)
        {
            var tolowerCategoryId = categoryId.ToLower();

            try
            {
                var deleteData = await this.db.MasterKategoris
                    .FirstOrDefaultAsync(Q => Q.KategoriCode.ToLower() == tolowerCategoryId);

                this.db.MasterKategoris.Remove(deleteData);
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
        public async Task<bool> IsKategoriCodeExists(string kategoriCode)
        {
            var tolowerCategoryCode = kategoriCode.ToLower();

            var isExists = await this.db.MasterKategoris
                .AnyAsync(Q => Q.KategoriCode.ToLower() == tolowerCategoryCode);

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

            var isExists = await this.db.MasterKategoris
                .AnyAsync(Q => Q.Name.ToLower() == tolowerName);

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
            var tolowerCategoryId = id.ToLower();

            var isExists = await this.db.MasterKategoris
               .AnyAsync(Q => Q.Name.ToLower() == tolowerName && Q.KategoriCode != tolowerCategoryId);

            return isExists;
        }
    }
}
