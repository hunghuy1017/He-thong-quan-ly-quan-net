using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Models;

namespace WpfApp1.ViewModels
{
    public class SessionViewModel
    {
        public Session Session { get; set; }

        public string ComputerName => Session.Computer?.ComputerName ?? "Không rõ";

        public DateTime StartTime => Session.StartTime;
        public DateTime? EndTime => Session.EndTime;
        public decimal? TotalCost => Session.TotalCost;

        public List<OrderDetail> AllOrderDetails =>
            Session.Orders?.SelectMany(o => o.OrderDetails).ToList() ?? new();
    }
}