using System;
using System.Collections.Generic;

namespace IMS.Entities
{
    public partial class MasterKategori
    {
        public MasterKategori()
        {
            MasterBarangs = new HashSet<MasterBarang>();
        }

        public string KategoriCode { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }

        public virtual ICollection<MasterBarang> MasterBarangs { get; set; }
    }
}
