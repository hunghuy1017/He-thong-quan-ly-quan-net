using System;
using System.Collections.Generic;

namespace WpfApp1.Models;

public partial class Payment
{
    public int PaymentId { get; set; }

    public DateTime? PaymentTime { get; set; }

    public string? PaymentType { get; set; }

    public int RefId { get; set; }

    public decimal? Amount { get; set; }

    public int? StaffId { get; set; }

    public virtual User? Staff { get; set; }
}
