using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace web.Models;

public class UserProject{
    public int UserProjectID { get; set; }
    public int UserID { get; set; }
    public int ProjectID { get; set; }
    
    public User? User { get; set; }
    public Project? Project { get; set; }

}