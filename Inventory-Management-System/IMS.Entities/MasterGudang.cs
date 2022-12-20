using System;
using System.Collections.Generic;

namespace IMS.Entities
{
    public partial class MasterGudang
    {
        public MasterGudang()
        {
            StockTransactions = new HashSet<StockTransaction>();
        }

        public string GudangCode { get; set; }
        public string OutletCode { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }

        public virtual Outlet OutletCodeNavigation { get; set; }
        public virtual ICollection<StockTransaction> StockTransactions { get; set; }
    }
}
