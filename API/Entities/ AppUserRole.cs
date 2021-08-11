using Microsoft.AspNetCore.Identity;

namespace API.Entities
{
    //This is Joining table where it will bridging the User with Role.
    public class AppUserRole : IdentityUserRole<int>
    {
        public AppUser User { get; set; }
        public AppRole Role { get; set; }
    }
}