using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JucieAndFlower.Data.Enities.User
{
    namespace JucieAndFlower.Data.Enities.User
    {
        public class UserProfileDto
        {
            public string FullName { get; set; } = null!;
            public string Email { get; set; } = null!;
            public string? Phone { get; set; }
            public string? Address { get; set; }
            public string? Gender { get; set; }
            public DateOnly? BirthDate { get; set; }
            public int RoleId { get; set; } // Nếu bạn muốn hiện quyền
        }
    }

}
