namespace IMS.BE.Models.Transactions
{
    public class ViewInbound
    {
        public string SkuId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int StockIn { get; set; }
    }
}
