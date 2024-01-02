using Microsoft.AspNetCore.Identity;

namespace web.Models;

public class ApplicationUser : IdentityUser
{
     public string? Ime { get; set; }
     public string? Priimek { get; set; }

      // Foreign key for User
//     public int UserID { get; set; }

//     // Navigation property
//     public User? User { get; set; }
}
