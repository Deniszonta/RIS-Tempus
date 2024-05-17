using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace web.Models;

public enum StanjeTicketa{
    Reported, Working, OnHold, Resolved
}

public class Ticket{
    public int TicketID { get; set; }
    public string Naslov { get; set; }
    public string Opis { get; set; }
    public StanjeTicketa Stanje { get; set; }
    [ForeignKey("User")]public int UserID { get; set; }
    public int ProjectID { get; set; }
    public User? User { get; set; }
    public Project? Project { get; set; }
    public ApplicationUser? appUser { get; set; }
    public float cas { get; set; }
}