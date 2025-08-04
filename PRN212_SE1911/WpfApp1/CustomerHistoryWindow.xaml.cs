using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.EntityFrameworkCore;
using WpfApp1.Models;
using WpfApp1.ViewModels;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for CustomerHistoryWindow.xaml
    /// </summary>
    public partial class CustomerHistoryWindow : Window
    {
        public CustomerHistoryWindow()
        {
            InitializeComponent();
            LoadEndedSessions();
        }

        private void LoadEndedSessions()
        {
            using var context = new NetManagementContext();
            var sessions = context.Sessions
                .Include(s => s.Computer)
                .Include(s => s.Staff) // ✅ Đảm bảo có include Staff
                .Include(s => s.Orders)
                    .ThenInclude(o => o.OrderDetails)
                        .ThenInclude(od => od.Product)
                .Include(s => s.Orders)
                    .ThenInclude(o => o.Staff)
                .Where(s => s.EndTime != null)
                .OrderByDescending(s => s.EndTime)
                .ToList();

            var viewModels = sessions.Select(s => new SessionViewModel { Session = s }).ToList();
            dgHistory.ItemsSource = viewModels;
        }

        private void ViewOrders_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is SessionViewModel vm)
            {
                var allDetails = vm.AllOrderDetails;

                if (!allDetails.Any())
                {
                    MessageBox.Show("Không có món nào được gọi trong phiên này.");
                    return;
                }

                string message = string.Join("\n", allDetails.Select(detail =>
                {
                    string name = detail.Product?.ProductName ?? "Không rõ";
                    int qty = detail.Quantity;
                    decimal unitPrice = detail.Product?.Price ?? 0;
                    decimal total = unitPrice * qty;
                    string staffName = detail.Order?.Staff?.FullName ?? "Không rõ";

                    return $"{name} x{qty} ({total:N0} VNĐ) - Nhân viên: {staffName}";
                }));

                MessageBox.Show(message, $"Món đã gọi (Phiên #{vm.Session.SessionId})", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }

}
