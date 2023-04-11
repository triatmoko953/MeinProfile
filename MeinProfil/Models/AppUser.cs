using Microsoft.AspNetCore.Identity;

namespace MeinProfil.Models
{
    public class AppUser : IdentityUser
    {
       public string FullName { get; set; }
        public string Address { get; set; } = null!;

        public string Job { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public string ProfilePicture { get; set; } = null!;

        public string IdentityNumber { get; set; } = null!;
    }
}
