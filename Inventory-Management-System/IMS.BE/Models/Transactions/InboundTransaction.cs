namespace IMS.BE.Models.Transactions
{
    public class InboundTransaction
    {
        public string SkuCode { get; set; } = string.Empty;
        public string GudangCode { get; set; } = string.Empty;
        public int StockIn { get; set; }
    }
}
