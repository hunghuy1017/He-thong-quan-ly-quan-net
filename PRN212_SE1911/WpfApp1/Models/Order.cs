using System;
using System.Collections.Generic;

namespace WpfApp1.Models
{
    public partial class Order
    {
        public int OrderId { get; set; }

        public int? StaffId { get; set; }

        public int? SessionId { get; set; } // ✅ Liên kết tới Session

        public DateTime? OrderTime { get; set; }

        public decimal? TotalAmount { get; set; }

        public virtual User? Staff { get; set; }

        public virtual Session? Session { get; set; } // ✅ Navigation tới Session

        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
