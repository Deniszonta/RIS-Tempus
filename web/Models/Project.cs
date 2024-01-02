using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace web.Models;

public class Project{
    public int ProjectID { get; set; }
    public string Ime { get; set; }
    public string Opis { get; set; }
    public ICollection<UserProject>? Projekti { get; set; }
    public ICollection<Ticket>? Ticketi { get; set; }
}