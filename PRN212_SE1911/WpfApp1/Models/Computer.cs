using System;
using System.Collections.Generic;

namespace WpfApp1.Models;

public partial class Computer
{
    internal object ComputerType;

    public int ComputerId { get; set; }

    public string ComputerName { get; set; } = null!;

    public int? TypeId { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<Session> Sessions { get; set; } = new List<Session>();

    public virtual ComputerType? Type { get; set; }
}
