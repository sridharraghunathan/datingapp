using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace API.Entities
{
    public class AppUser : IdentityUser<int>
    {
        /*
        since we moved to Identity Module below columns will be inhertied from Identity
        public int Id { get; set; }
        public string UserName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        */

        public DateTime DateOfBirth { get; set; }
        public string KnownAs { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime LastActive { get; set; } = DateTime.Now;
        public string Gender { get; set; }
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public ICollection<Photo> Photos { get; set; }

        public ICollection<UserLike> LikedByUsers { get; set; }
        public ICollection<UserLike> LikedUsers { get; set; }
        public ICollection<Message> Sender { get; set; }
        public ICollection<Message> Recipient { get; set; }
        public ICollection<AppUserRole> UserRoles { get; set; }

        // Below is an Extension Method to calc the age and the Getxxx naming convention
        // // is very important when we need to use the auotmapper.
        // public int GetAge()
        // {
        //     return DateOfBirth.CalculateAge();
        // }

    }
}