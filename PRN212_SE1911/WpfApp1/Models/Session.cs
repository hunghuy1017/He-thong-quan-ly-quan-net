using System;
using System.Collections.Generic;

namespace WpfApp1.Models;

public partial class Session
{
    public int SessionId { get; set; }

    public int? ComputerId { get; set; }

    public int? StaffId { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public decimal? TotalCost { get; set; }

    public virtual Computer? Computer { get; set; }

    public virtual User? Staff { get; set; }
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

}
