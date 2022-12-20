namespace IMS.BE.Models.Transactions
{
    public class OutbondTransaction
    {
        public string SkuCode { get; set; } = string.Empty;
        public string GudangCode { get; set; } = string.Empty;
        public int StockOut { get; set; }
    }
}
