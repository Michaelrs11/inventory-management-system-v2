using System;
using System.Collections.Generic;

namespace IMS.Entities
{
    public partial class StockTransaction
    {
        public int StockTransactionId { get; set; }
        public string SKUID { get; set; }
        public string GudangCode { get; set; }
        public int StockBefore { get; set; }
        public int StockAfter { get; set; }
        public int? StockIn { get; set; }
        public int? StockOut { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }

        public virtual MasterGudang GudangCodeNavigation { get; set; }
        public virtual MasterBarang SKU { get; set; }
    }
}
