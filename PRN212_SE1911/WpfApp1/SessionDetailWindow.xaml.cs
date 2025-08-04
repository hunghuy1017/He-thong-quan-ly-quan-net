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
    using System.Windows.Threading;
    using Microsoft.EntityFrameworkCore;
    using WpfApp1.Models;

    namespace WpfApp1
    {
    /// <summary>
    /// Interaction logic for SessionDetailWindow.xaml
    /// </summary>
    public partial class SessionDetailWindow : Window
    {
        private readonly int _sessionId;
        private Session? _session;
        private DispatcherTimer _timer;

        public SessionDetailWindow(int sessionId)
        {
            InitializeComponent();
            _sessionId = sessionId;
            Loaded += SessionDetailWindow_Loaded;
        }

        private void SessionDetailWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadSessionDetails();
            StartTimer();
        }

        private void LoadSessionDetails()
        {
            using var context = new NetManagementContext();

            _session = context.Sessions
                .Include(s => s.Computer).ThenInclude(c => c.Type)
                .Include(s => s.Staff)
                .Include(s => s.Orders).ThenInclude(o => o.OrderDetails).ThenInclude(od => od.Product)
                .FirstOrDefault(s => s.SessionId == _sessionId);

            if (_session == null)
            {
                MessageBox.Show("Không tìm thấy phiên chơi.");
                Close();
                return;
            }

            txtComputer.Text = _session.Computer?.ComputerName ?? "N/A";
            txtStaff.Text = _session.Staff?.FullName ?? "N/A";
            txtStart.Text = _session.StartTime.ToString("g");
            txtEnd.Text = _session.EndTime?.ToString("g") ?? "Đang chơi";

            UpdateDurationAndTotal();

            var orderDetails = _session.Orders.SelectMany(o => o.OrderDetails).ToList();
            dgProducts.ItemsSource = orderDetails;

            // ✅ Cập nhật tổng tiền món đã gọi
            UpdateOrderTotal(orderDetails);
        }

        private void UpdateDurationAndTotal()
        {
            if (_session == null) return;

            var now = DateTime.Now;
            var duration = (decimal)(now - _session.StartTime).TotalHours;
            var sessionCost = Math.Round(duration * _session.Computer.Type.HourlyRate, 0);
            var totalFoodCost = _session.Orders.SelectMany(o => o.OrderDetails).Sum(od => od.Quantity * od.UnitPrice);

            txtDuration.Text = $"{Math.Floor(duration)} giờ {Math.Round((duration % 1) * 60)} phút";
            txtCost.Text = $"{(sessionCost + totalFoodCost):N0} VND";
        }

        private void UpdateOrderTotal(IEnumerable<OrderDetail> orderDetails)
        {
            decimal total = orderDetails.Sum(od => od.Quantity * od.UnitPrice);
            txtOrderTotal.Text = $"{total:N0} VNĐ";
        }

        private void StartTimer()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += (s, e) => UpdateDurationAndTotal();
            _timer.Start();
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            using var context = new NetManagementContext();
            var session = context.Sessions
                .Include(s => s.Orders).ThenInclude(o => o.OrderDetails)
                .FirstOrDefault(s => s.SessionId == _sessionId);

            if (session == null) return;

            var order = session.Orders.FirstOrDefault();
            if (order == null)
            {
                order = new Order
                {
                    StaffId = session.StaffId,
                    OrderTime = DateTime.Now,
                    TotalAmount = 0
                };
                context.Orders.Add(order);
                session.Orders.Add(order);
            }

            var selectWindow = new SelectProductWindow();
            if (selectWindow.ShowDialog() == true)
            {
                foreach (var item in selectWindow.SelectedItems)
                {
                    order.OrderDetails.Add(new OrderDetail
                    {
                        ProductId = item.Product.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = item.Product.Price
                    });

                    order.TotalAmount += item.Product.Price * item.Quantity;
                }

                context.SaveChanges();
                LoadSessionDetails();
            }
        }

        private void EditOrder_Click(object sender, RoutedEventArgs e)
        {
            using var context = new NetManagementContext();
            var session = context.Sessions
                .Include(s => s.Orders)
                    .ThenInclude(o => o.OrderDetails)
                        .ThenInclude(od => od.Product)
                .FirstOrDefault(s => s.SessionId == _sessionId);

            if (session == null) return;

            var order = session.Orders.FirstOrDefault();
            if (order == null || order.OrderDetails.Count == 0)
            {
                MessageBox.Show("Không có sản phẩm nào để sửa.");
                return;
            }

            var selectedItems = order.OrderDetails
                .Select(od => new SelectProductWindow.SelectedItem
                {
                    Product = od.Product!,
                    Quantity = od.Quantity
                }).ToList();

            var editWindow = new SelectProductWindow(selectedItems);
            if (editWindow.ShowDialog() == true)
            {
                order.OrderDetails.Clear();
                order.TotalAmount = 0;

                foreach (var item in editWindow.SelectedItems)
                {
                    order.OrderDetails.Add(new OrderDetail
                    {
                        ProductId = item.Product.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = item.Product.Price
                    });

                    order.TotalAmount += item.Product.Price * item.Quantity;
                }

                context.SaveChanges();
                LoadSessionDetails();
            }
        }

        private void EndSession_Click(object sender, RoutedEventArgs e)
        {
            _timer?.Stop();

            using var context = new NetManagementContext();

            var session = context.Sessions
                .Include(s => s.Computer).ThenInclude(c => c.Type)
                .Include(s => s.Orders).ThenInclude(o => o.OrderDetails)
                .FirstOrDefault(s => s.SessionId == _sessionId);

            if (session == null)
            {
                MessageBox.Show("Không tìm thấy phiên chơi.");
                return;
            }

            session.EndTime = DateTime.Now;
            var duration = (decimal)(session.EndTime.Value - session.StartTime).TotalHours;
            var sessionCost = Math.Round(duration * session.Computer.Type.HourlyRate, 0);
            var totalFoodCost = session.Orders.SelectMany(o => o.OrderDetails).Sum(od => od.Quantity * od.UnitPrice);

            session.TotalCost = sessionCost + totalFoodCost;
            session.Computer.Status = "Available";

            context.SaveChanges();

            MessageBox.Show($"Tổng tiền phiên chơi: {session.TotalCost:N0} VND", "Hoàn tất", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        private void DeleteOrder_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is OrderDetail odToDelete)
            {
                var result = MessageBox.Show("Bạn có chắc chắn muốn xoá món này?", "Xác nhận xoá", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    using var context = new NetManagementContext();
                    var orderDetail = context.OrderDetails.FirstOrDefault(od => od.OrderDetailId == odToDelete.OrderDetailId);
                    if (orderDetail != null)
                    {
                        context.OrderDetails.Remove(orderDetail);

                        var order = context.Orders.FirstOrDefault(o => o.OrderId == orderDetail.OrderId);
                        if (order != null)
                        {
                            order.TotalAmount -= orderDetail.UnitPrice * orderDetail.Quantity;
                        }

                        context.SaveChanges();
                        LoadSessionDetails();
                    }
                }
            }
        }
    }
}
