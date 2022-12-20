using System;
using System.Collections.Generic;

namespace IMS.Entities
{
    public partial class MasterUser
    {
        public string UserCode { get; set; }
        public int UserRoleEnumId { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }

        public virtual UserRoleEnum UserRoleEnum { get; set; }
    }
}
