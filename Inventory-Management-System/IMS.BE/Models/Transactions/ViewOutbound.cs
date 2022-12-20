namespace IMS.BE.Models.Transactions
{
    public class ViewOutbound
    {
        public string SkuId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int StockOut { get; set; }
    }
}
