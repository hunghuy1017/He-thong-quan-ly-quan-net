using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using Microsoft.EntityFrameworkCore;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp1.Models;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for SessionWindow.xaml
    /// </summary>
    public partial class SessionWindow : Window
    {
        private readonly User _loggedInUser;

        public SessionWindow(User user)
        {
            InitializeComponent();
            _loggedInUser = user;

            LoadComputersByType();
        }


        private void LoadComputersByType()
        {
            TypePanel.Children.Clear();

            using var context = new NetManagementContext();

            var grouped = context.Computers
                .Include(c => c.Type)
                .AsEnumerable()
                .GroupBy(c => c.Type.TypeName)
                .OrderBy(g => g.Key);

            foreach (var group in grouped)
            {
                var groupBox = new GroupBox
                {
                    Header = $"💻 {group.Key}",
                    FontWeight = FontWeights.SemiBold,
                    FontSize = 16,
                    Padding = new Thickness(10),
                    Margin = new Thickness(0, 0, 0, 20),
                    BorderBrush = Brushes.Gray,
                    BorderThickness = new Thickness(1)
                };

                var wrap = new WrapPanel();

                foreach (var comp in group)
                {
                    var btn = new Button
                    {
                        Content = new StackPanel
                        {
                            Orientation = Orientation.Vertical,
                            Children =
                            {
                                new TextBlock
                                {
                                    Text = comp.ComputerName,
                                    FontWeight = FontWeights.Bold,
                                    FontSize = 14,
                                    HorizontalAlignment = HorizontalAlignment.Center
                                },
                                new TextBlock
                                {
                                    Text = comp.Status,
                                    FontSize = 12,
                                    Foreground = Brushes.White,
                                    HorizontalAlignment = HorizontalAlignment.Center
                                }
                            }
                        },
                        Width = 100,
                        Height = 80,
                        Margin = new Thickness(6),
                        Cursor = Cursors.Hand,
                        Tag = comp,
                        ToolTip = $"Loại: {comp.Type.TypeName}\nGiá: {comp.Type.HourlyRate:N0}đ/h"
                    };

                    // Màu nền theo trạng thái
                    switch (comp.Status)
                    {
                        case "Available":
                            btn.Background = new SolidColorBrush(Color.FromRgb(76, 175, 80)); // xanh lá
                            break;
                        case "InUse":
                            btn.Background = new SolidColorBrush(Color.FromRgb(255, 152, 0)); // cam
                            break;
                        case "Maintenance":
                            btn.Background = new SolidColorBrush(Color.FromRgb(158, 158, 158)); // xám
                            break;
                        default:
                            btn.Background = Brushes.LightGray;
                            break;
                    }

                    // Gắn sự kiện khi click vào máy
                    btn.Click += Computer_Click;

                    wrap.Children.Add(btn);
                }

                groupBox.Content = wrap;
                TypePanel.Children.Add(groupBox);
            }
        }

        private void Computer_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Computer selectedComp)
            {
                using var context = new NetManagementContext();
                var computer = context.Computers
                    .Include(c => c.Sessions)
                    .FirstOrDefault(c => c.ComputerId == selectedComp.ComputerId);

                if (computer == null)
                {
                    MessageBox.Show("Không tìm thấy máy.");
                    return;
                }

                if (computer.Status == "Maintenance")
                {
                    if (_loggedInUser.RoleId == 1) // Admin được quyền bật máy
                    {
                        if (MessageBox.Show("Bạn có muốn chuyển máy sang trạng thái hoạt động?", "Xác nhận", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            computer.Status = "Available";
                            context.SaveChanges();
                            LoadComputersByType();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Máy đang bảo trì.");
                    }
                    return;
                }

                if (computer.Status == "Available")
                {
                    var newSession = new Session
                    {
                        ComputerId = computer.ComputerId,
                        StaffId = _loggedInUser.UserId,
                        StartTime = DateTime.Now
                    };

                    context.Sessions.Add(newSession);
                    computer.Status = "InUse";
                    context.SaveChanges();

                    MessageBox.Show($"Đã bắt đầu phiên mới cho {computer.ComputerName}");
                }
                else if (computer.Status == "InUse")
                {
                    var activeSession = context.Sessions
                        .Include(s => s.Computer)
                        .Include(s => s.Staff)
                        .FirstOrDefault(s => s.ComputerId == computer.ComputerId && s.EndTime == null);

                    if (activeSession != null)
                    {
                        var detailWindow = new SessionDetailWindow(activeSession.SessionId);
                        detailWindow.ShowDialog();
                    }
                }

                LoadComputersByType(); // Refresh lại
            }
        }



        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            LoadComputersByType();
        }
    }
}
