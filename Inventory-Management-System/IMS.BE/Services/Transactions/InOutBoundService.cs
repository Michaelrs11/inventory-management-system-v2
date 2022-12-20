using Dapper;
using IMS.BE.Commons.Services;
using IMS.BE.Models.Masters;
using IMS.BE.Models.Transactions;
using IMS.Entities;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;
using System.Text;

namespace IMS.BE.Services.Transactions
{
    public class InOutBoundService
    {
        private readonly IMSDBContext db;
        private readonly UserIdentityService userIdentity;

        public InOutBoundService(IMSDBContext db, UserIdentityService userIdentity)
        {
            this.db = db;
            this.userIdentity = userIdentity;
        }
        
        

        /// <summary>
        /// Insert Stock
        /// </summary>
        /// <param name="insert"></param>
        /// <returns></returns>
        public async Task InsertInbound(InboundTransaction insert)
        {
            var tolowerSkuCode = insert.SkuCode.ToLower();

            var tolowerGudangCode = insert.GudangCode.ToLower();

            var userLogin = userIdentity.UserCode ?? "SYSTEM";

            var datetimeOffset = DateTime.UtcNow;

            var getStockByCode = await this.db.StockTransactions
                .OrderByDescending(Q => Q.CreatedAt)
                .FirstOrDefaultAsync(Q => Q.GudangCode.ToLower() == tolowerGudangCode && Q.SKUID.ToLower() == tolowerSkuCode);

            if (getStockByCode == null)
            {
                this.db.StockTransactions.Add(new StockTransaction
                {
                    SKUID = insert.SkuCode.ToUpper(),
                    GudangCode = insert.GudangCode.ToUpper(),
                    StockBefore = 0,
                    StockIn = insert.StockIn,
                    StockOut = 0,
                    StockAfter = 0 + insert.StockIn,
                    CreatedAt = datetimeOffset,
                    CreatedBy = userLogin,
                    UpdatedAt = datetimeOffset,
                    UpdatedBy = userLogin
                });
            }
            else
            {
                this.db.StockTransactions.Add(new StockTransaction
                {
                    SKUID = insert.SkuCode.ToUpper(),
                    GudangCode = insert.GudangCode.ToUpper(),
                    StockBefore = getStockByCode.StockAfter,
                    StockIn = insert.StockIn,
                    StockOut = 0,
                    StockAfter = getStockByCode.StockAfter + insert.StockIn,
                    CreatedAt = datetimeOffset,
                    CreatedBy = userLogin,
                    UpdatedAt = datetimeOffset,
                    UpdatedBy = userLogin
                });

            }
            await db.SaveChangesAsync();

        }

        public string GenerateInitialQuery(bool isInbound)
        {
            _ = nameof(StockTransaction.SKUID);
            _ = nameof(StockTransaction.GudangCode);
            _ = nameof(StockTransaction.StockIn);
            _ = nameof(StockTransaction.StockOut);
            _ = nameof(MasterBarang.Name);

            if (isInbound)
            {
                return @"SELECT
	                    st.SKUID AS [SkuId], 
	                    mb.Name AS [Name],
	                    SUM(CAST(st.StockIn AS NUMERIC)) AS [StockIn]
                    FROM StockTransaction st
                    JOIN MasterBarang mb ON st.SKUID = mb.SKUID";
            }
            return @"SELECT
	                    st.SKUID AS [SkuId], 
	                    mb.Name AS [Name],
	                    SUM(CAST(st.StockOut AS NUMERIC)) AS [StockOut]
                    FROM StockTransaction st
                    JOIN MasterBarang mb ON st.SKUID = mb.SKUID";
           
        }

        public string GenerateFilterQuery(string? gudangCode, DateTime? dateFrom,
            DateTime? dateTo)
        {
            var filterQuery = new StringBuilder();
            filterQuery.Append("WHERE ");

            if (!string.IsNullOrEmpty(gudangCode))
            {
                filterQuery.AppendLine("st.GudangCode = @gudangCode AND");
            }

            if (dateFrom != null)
            {
                filterQuery.AppendLine("CONVERT(DATE, st.CreatedAt) >= @dateFrom AND");
            }

            if (dateTo != null)
            {
                filterQuery.AppendLine("CONVERT(DATE, st.CreatedAt) <= @dateTo AND");
            }

            return filterQuery.Length > 6 ? filterQuery.Remove(filterQuery.Length - 5, 5).ToString() : string.Empty;
        }

        public string GenerateGroupbyQuery()
        {
            var groupbyQuery = "GROUP BY st.SKUID, mb.Name ";

            return groupbyQuery;
        }

        public StringBuilder GenerateOrderQuery(int? currentPage, StringBuilder query)
        {
            query.Append("ORDER BY st.SKUID ");

            if (currentPage.HasValue == false || currentPage.Value < 1)
            {
                currentPage = 1;
            }

            if (currentPage != 0)
            {
                query.Append(@"
OFFSET @Offset ROWS
FETCH NEXT @PageSize ROWS ONLY");
            }

            return query;
        }

        public async Task<(List<ViewInbound>, int?)> GetInboundTransaction(string? gudangCode, DateTime? dateFrom,
            DateTime? dateTo, int? currentPage)
        {
            if (currentPage.HasValue == false || currentPage.Value < 1)
            {
                currentPage = 1;
            }

            var query = new StringBuilder();

            query.AppendLine(GenerateInitialQuery(true));
            query.AppendLine(GenerateFilterQuery(gudangCode, dateFrom, dateTo));
            query.Append(GenerateGroupbyQuery());

            var pageSize = 10;

            var filterQuery = new
            {
                gudangCode = $@"{gudangCode}",
                dateFrom = dateFrom?.ToString("yyyy-MM-dd"),
                dateTo = dateTo?.ToString("yyyy-MM-dd"),
                Offset = ((currentPage - 1) * 10),
                PageSize = pageSize,
            };

            query = GenerateOrderQuery(currentPage, query);

            var gridData = (await db.Database.GetDbConnection().QueryAsync<ViewInbound>(query.ToString(), filterQuery)).ToList();

            return (gridData, currentPage);
        }

        public async Task<(List<ViewOutbound>, int?)> GetOutboundTransaction(string? gudangCode, DateTime? dateFrom,
            DateTime? dateTo, int? currentPage)
        {
            if (currentPage.HasValue == false || currentPage.Value < 1)
            {
                currentPage = 1;
            }

            var query = new StringBuilder();

            query.AppendLine(GenerateInitialQuery(false));
            query.AppendLine(GenerateFilterQuery(gudangCode, dateFrom, dateTo));
            query.Append(GenerateGroupbyQuery());

            var pageSize = 10;

            var filterQuery = new
            {
                gudangCode = $@"{gudangCode}",
                dateFrom = dateFrom?.ToString("yyyy-MM-dd"),
                dateTo = dateTo?.ToString("yyyy-MM-dd"),
                Offset = ((currentPage - 1) * 10),
                PageSize = pageSize,
            };

            query = GenerateOrderQuery(currentPage, query);

            var gridData = (await db.Database.GetDbConnection().QueryAsync<ViewOutbound>(query.ToString(), filterQuery)).ToList();

            return (gridData, currentPage);
        }

        /// <summary>
        /// Outbound Transaction
        /// </summary>
        /// <param name="insert"></param>
        /// <returns></returns>
        public async Task<bool> InsertOutbound(OutbondTransaction insert)
        {
            var tolowerSkuCode = insert.SkuCode.ToLower();

            var tolowerGudangCode = insert.GudangCode.ToLower();

            var userLogin = userIdentity.UserCode ?? "SYSTEM";

            var datetimeOffset = DateTime.UtcNow;

            var getStockByCode = await this.db.StockTransactions
                .OrderByDescending(Q => Q.CreatedAt)
                .FirstOrDefaultAsync(Q => Q.GudangCode.ToLower() == tolowerGudangCode && Q.SKUID.ToLower() == tolowerSkuCode);

            if (getStockByCode == null)
            {
                return false;
            }
            else
            {
                if (getStockByCode.StockAfter < insert.StockOut)
                {
                    return false;
                }

                this.db.StockTransactions.Add(new StockTransaction
                {
                    SKUID = insert.SkuCode.ToUpper(),
                    GudangCode = insert.GudangCode.ToUpper(),
                    StockBefore = getStockByCode.StockAfter,
                    StockIn = 0,
                    StockOut = insert.StockOut,
                    StockAfter = getStockByCode.StockAfter - insert.StockOut,
                    CreatedAt = datetimeOffset,
                    CreatedBy = userLogin,
                    UpdatedAt = datetimeOffset,
                    UpdatedBy = userLogin
                });
            }
            await db.SaveChangesAsync();
            return true;
        }
    }
}
