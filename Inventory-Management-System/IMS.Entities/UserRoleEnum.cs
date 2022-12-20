using System;
using System.Collections.Generic;

namespace IMS.Entities
{
    public partial class UserRoleEnum
    {
        public UserRoleEnum()
        {
            MasterUsers = new HashSet<MasterUser>();
        }

        public int UserRoleEnumId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<MasterUser> MasterUsers { get; set; }
    }
}
