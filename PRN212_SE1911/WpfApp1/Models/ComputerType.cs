using System;
using System.Collections.Generic;

namespace WpfApp1.Models;

public partial class ComputerType
{
    public int TypeId { get; set; } 

    public string TypeName { get; set; } = null!;

    public decimal HourlyRate { get; set; }

    public virtual ICollection<Computer> Computers { get; set; } = new List<Computer>();
}
