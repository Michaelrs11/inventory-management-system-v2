using System;
using System.Collections.Generic;

namespace IMS.Entities
{
    public partial class Outlet
    {
        public Outlet()
        {
            MasterGudangs = new HashSet<MasterGudang>();
        }

        public string OutletCode { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }

        public virtual ICollection<MasterGudang> MasterGudangs { get; set; }
    }
}
