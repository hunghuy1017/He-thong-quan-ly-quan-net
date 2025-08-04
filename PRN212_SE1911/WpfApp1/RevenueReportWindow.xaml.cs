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
using Microsoft.EntityFrameworkCore;
using System.Windows.Shapes;
using WpfApp1.Models;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for RevenueReportWindow.xaml
    /// </summary>
    public partial class RevenueReportWindow : Window
    {
        private User _loggedInUser;

        public RevenueReportWindow(User user)
        {
            InitializeComponent();
            _loggedInUser = user;

            // Chặn truy cập nếu không phải admin
            if (_loggedInUser.RoleId != 1)
            {
                MessageBox.Show("Chức năng này chỉ dành cho quản trị viên!", "Không có quyền", MessageBoxButton.OK, MessageBoxImage.Warning);
                Close();
                return;
            }

            dpReportDate.SelectedDate = DateTime.Today;
            LoadReport(DateTime.Today);
        }

        private void BtnViewReport_Click(object sender, RoutedEventArgs e)
        {
            if (dpReportDate.SelectedDate.HasValue)
            {
                LoadReport(dpReportDate.SelectedDate.Value);
            }
        }

        private void DpReportDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dpReportDate.SelectedDate.HasValue)
            {
                LoadReport(dpReportDate.SelectedDate.Value);
            }
        }

        private void LoadReport(DateTime selectedDate)
        {
            using var context = new NetManagementContext();

            var sessions = context.Sessions
                .Include(s => s.Computer)
                .Include(s => s.Staff)
                .Where(s => s.EndTime != null && s.EndTime.Value.Date == selectedDate.Date)
                .ToList();

            dgEndedSessions.ItemsSource = sessions;

            var dailyRevenue = sessions.Sum(s => s.TotalCost ?? 0);

            lblDailyRevenue.Text = $"Doanh thu ngày: {dailyRevenue:N0} VNĐ";
            lblReportStatus.Text = $"Có {sessions.Count} phiên kết thúc trong ngày {selectedDate:dd/MM/yyyy}.";
        }
    }
}
