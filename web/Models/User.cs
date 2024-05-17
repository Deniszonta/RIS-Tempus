using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace web.Models;

public class User{
    public int UserID { get; set; }
    public string Ime { get; set; }
    public string Priimek { get; set; }
    public string Email { get; set; }

    // Navigation property
    public ApplicationUser? appUser { get; set; }
    public ICollection<UserProject>? Projekti { get; set; }
    public ICollection<Ticket>? Ticketi { get; set; }

}