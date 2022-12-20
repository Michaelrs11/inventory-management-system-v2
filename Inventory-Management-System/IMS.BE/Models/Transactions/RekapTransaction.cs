namespace IMS.BE.Models.Transactions
{
    public class RekapTransaction
    {
        public string SkuId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Stock { get; set; }
    }
}
