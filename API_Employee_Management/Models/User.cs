using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace API_Employee_Management.Models
{
    public partial class User
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string Login { get; set; }
        public string Pass { get; set; }

        public virtual Role Role { get; set; }
    }
}
