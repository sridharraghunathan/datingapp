using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace API.Entities
{
    //This will hold list of role available.
    public class AppRole : IdentityRole<int>
    {
        public ICollection<AppUserRole> UserRoles { get; set; }
    }
}