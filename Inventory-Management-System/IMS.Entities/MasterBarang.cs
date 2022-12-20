using System;
using System.Collections.Generic;

namespace IMS.Entities
{
    public partial class MasterBarang
    {
        public MasterBarang()
        {
            StockTransactions = new HashSet<StockTransaction>();
        }

        public string SKUID { get; set; }
        public string KategoriCode { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }

        public virtual MasterKategori KategoriCodeNavigation { get; set; }
        public virtual ICollection<StockTransaction> StockTransactions { get; set; }
    }
}
