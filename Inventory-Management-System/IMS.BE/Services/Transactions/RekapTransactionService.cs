using ClosedXML.Excel;
using Dapper;
using DocumentFormat.OpenXml.Wordprocessing;
using IMS.BE.Models.Transactions;
using IMS.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Threading;

namespace IMS.BE.Services.Transactions
{
    public class RekapTransactionService
    {
        private readonly IMSDBContext db;
        private readonly List<string> Header = new() { "SkuId", "Name", "Stock"};
        public RekapTransactionService(IMSDBContext db)
        {
            this.db = db;
        }

        public string GenerateInitialQuery()
        {
            _ = nameof(StockTransaction.SKUID);
            _ = nameof(StockTransaction.GudangCode);
            _ = nameof(StockTransaction.StockIn);
            _ = nameof(StockTransaction.StockOut);
            _ = nameof(MasterBarang.Name);

            return @"SELECT
	                    st.SKUID AS [SkuId], 
	                    mb.Name AS [Name],
	                    SUM(CAST(st.StockIn AS NUMERIC)) - SUM(CAST(st.StockOut AS NUMERIC)) AS [Stock]
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

        public async Task<(List<RekapTransaction>, int?)> GetRekapTransaction(string? gudangCode, DateTime? dateFrom,
            DateTime? dateTo, int? currentPage)
        {
            if (currentPage.HasValue == false || currentPage.Value < 1)
            {
                currentPage = 1;
            }

            var pageSize = 10;

            var query = new StringBuilder();

            query.AppendLine(GenerateInitialQuery());
            query.AppendLine(GenerateFilterQuery(gudangCode, dateFrom, dateTo));
            query.Append(GenerateGroupbyQuery());

            var filterQuery = new
            {
                gudangCode = $@"{gudangCode}",
                dateFrom = dateFrom?.ToString("yyyy-MM-dd"),
                dateTo = dateTo?.ToString("yyyy-MM-dd"),
                Offset = ((currentPage - 1) * 10),
                PageSize = pageSize,
            };

            query = GenerateOrderQuery(currentPage, query);

            var gridData = (await db.Database.GetDbConnection().QueryAsync<RekapTransaction>(query.ToString(), filterQuery)).ToList();

            return (gridData, currentPage);
        }

        public async Task<byte[]> DownloadRekapTransaction(string? gudangCode, DateTime? dateFrom,
            DateTime? dateTo)
        {
            var query = new StringBuilder();

            query.AppendLine(GenerateInitialQuery());
            query.AppendLine(GenerateFilterQuery(gudangCode, dateFrom, dateTo));
            query.Append(GenerateGroupbyQuery());

            var filterQuery = new
            {
                gudangCode = $@"{gudangCode}",
                dateFrom = dateFrom?.ToString("yyyy-MM-dd"),
                dateTo = dateTo?.ToString("yyyy-MM-dd"),
            };

            var gridData = (await db.Database.GetDbConnection().QueryAsync<RekapTransaction>(query.ToString(), filterQuery)).ToList();

            IXLWorkbook workbook = new XLWorkbook();
            IXLWorksheet worksheet = workbook.Worksheets.Add("Rekap Transaction");
            var Title = $"Rekap Data";
            var headerCount = Header.Count;
            var startColumn = 2;
            var startRow = 5;
            var startRowTitle = 2;
            worksheet.Cell(startColumn, startRowTitle).Value = Title;
            worksheet.Cell(startColumn, startRowTitle).Style.Font.FontSize = 20;
            worksheet.Cell(startColumn, startRowTitle).Style.Font.Bold = true;
            worksheet.Range(startRowTitle, startColumn, startRowTitle, startColumn + 3).Merge();
            for (var i = startColumn; i < startColumn + headerCount; i++)
            {
                worksheet.Cell(startRow, i).Style.Font.FontSize = 14;
                worksheet.Cell(startRow, i).Style.Font.Bold = true;
                worksheet.Cell(startRow, i).Style.Font.FontColor = XLColor.White;
                worksheet.Cell(startRow, i).Style.Fill.BackgroundColor = XLColor.Black;
                worksheet.Cell(startRow, i).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            }
            // Isi Header
            var columnHeader = startColumn;
            foreach (var header in this.Header)
            {
                worksheet.Cell(startRow, columnHeader).Value = header;
                columnHeader++;
            }
            startRow++;
            var index = 0;
            for (var i = startRow; i < gridData.Count + startRow; i++)
            {
                var column = startColumn;
                worksheet.Cell(i, column).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(i, column++).Value = gridData[index].SkuId;
                worksheet.Cell(i, column).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(i, column++).Value = gridData[index].Name;
                worksheet.Cell(i, column).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(i, column++).Value = gridData[index].Stock;
                index++;
            }
            //adjust width column
            worksheet.Columns(startColumn, startColumn + (headerCount - 1)).AdjustToContents();
            using (var stream = new System.IO.MemoryStream())
            {
                workbook.SaveAs(stream);
                var content = stream.ToArray();
                return content;
            }
        }
    }
}
