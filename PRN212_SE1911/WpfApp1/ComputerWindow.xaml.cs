using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for ComputerWindow.xaml
    /// </summary>
    public partial class ComputerWindow : Window
    {
        private readonly User _loggedInUser;
        public bool IsAdmin => _loggedInUser.RoleId == 1;

        public ObservableCollection<Computer> Computers { get; set; } = new();
        public ObservableCollection<ComputerType> ComputerTypes { get; set; } = new();
        public ObservableCollection<string> StatusList { get; set; } = new()
        {
            "Available", "InUse", "Maintenance"
        };

        public ComputerWindow(User user)
        {
            InitializeComponent();
            _loggedInUser = user;
            DataContext = this;
            LoadData();

            if (!IsAdmin)
            {
                btnAddComputer.Visibility = Visibility.Collapsed;
                btnAddType.Visibility = Visibility.Collapsed;
                btnSave.Visibility = Visibility.Collapsed;
                DeleteColumn.Visibility = Visibility.Collapsed;
            }
        }

        private void LoadData()
        {
            using var context = new NetManagementContext();

            // Lấy đầy đủ loại máy và máy từ database
            var comps = context.Computers
                .Include(c => c.Type) // Ensure Type is loaded
                .AsNoTracking()       // Không cache để đảm bảo dữ liệu mới nhất
                .OrderBy(c => c.ComputerName)
                .ToList();

            var types = context.ComputerTypes
                .AsNoTracking()
                .OrderBy(t => t.TypeName)
                .ToList();

            // Cập nhật lại 2 danh sách
            Computers = new ObservableCollection<Computer>(comps);
            ComputerTypes = new ObservableCollection<ComputerType>(types);

            // Gán lại DataContext nếu cần thiết
            DataContext = null;
            DataContext = this;

            // Gán lại nguồn cho DataGrid
            ComputerGrid.ItemsSource = null;
            ComputerGrid.ItemsSource = Computers;
            
        }


        private void Refresh_Click(object sender, RoutedEventArgs e) => LoadData();

        private void AddComputer_Click(object sender, RoutedEventArgs e)
        {
            using var context = new NetManagementContext();

            var existingNames = context.Computers
                .Where(c => EF.Functions.Like(c.ComputerName, "PC%"))
                .Select(c => c.ComputerName)
                .ToList();

            int nextIndex = 1;
            while (true)
            {
                string candidateName = $"PC{nextIndex:D2}";
                if (!existingNames.Contains(candidateName) &&
                    !Computers.Any(c => c.ComputerName == candidateName))
                {
                    var newComp = new Computer
                    {
                        ComputerName = candidateName,
                        TypeId = ComputerTypes.FirstOrDefault()?.TypeId,
                        Status = "Available"
                    };

                    Computers.Add(newComp);
                    break;
                }
                nextIndex++;
            }

            Computers = new ObservableCollection<Computer>(
                Computers.OrderBy(c => c.ComputerName, StringComparer.OrdinalIgnoreCase)
            );
            ComputerGrid.ItemsSource = Computers;
        }

        private void DeleteComputer_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is Computer comp)
            {
                var confirm = MessageBox.Show($"Xoá máy {comp.ComputerName}?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (confirm != MessageBoxResult.Yes) return;

                try
                {
                    using var context = new NetManagementContext();
                    var dbComp = context.Computers.Find(comp.ComputerId);

                    if (dbComp != null)
                    {
                        bool isReferenced = context.Sessions.Any(s => s.ComputerId == comp.ComputerId);
                        if (isReferenced)
                        {
                            MessageBox.Show("Không thể xóa vì máy đã được sử dụng trước đây.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }

                        context.Computers.Remove(dbComp);
                        context.SaveChanges();
                    }
                    else
                    {
                        Computers.Remove(comp);
                    }

                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa máy: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void AddType_Click(object sender, RoutedEventArgs e)
        {
            var window = new ComputerTypeWindow();
            window.ShowDialog();
            LoadData();
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            using var context = new NetManagementContext();

            foreach (var comp in Computers)
            {
                if (comp.ComputerId == 0)
                {
                    context.Computers.Add(comp);
                }
                else
                {
                    var dbComp = context.Computers.FirstOrDefault(c => c.ComputerId == comp.ComputerId);
                    if (dbComp != null)
                    {
                        dbComp.ComputerName = comp.ComputerName;
                        dbComp.TypeId = comp.TypeId;
                        dbComp.Status = comp.Status;
                    }
                }
            }

            context.SaveChanges();
            MessageBox.Show("Đã lưu thay đổi!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            LoadData();
        }

        private void Status_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.DataContext is Computer comp && comp.ComputerId != 0)
            {
                try
                {
                    using var context = new NetManagementContext();
                    var dbComp = context.Computers.FirstOrDefault(c => c.ComputerId == comp.ComputerId);
                    if (dbComp != null)
                    {
                        dbComp.Status = comp.Status;
                        context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi cập nhật trạng thái: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ComputerGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void BtnViewTypeComputer_Click(object sender, RoutedEventArgs e)
        {
            string keyword = txtSearchType.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(keyword))
            {
                // Nếu ô tìm kiếm trống thì load lại toàn bộ
                LoadData();
                return;
            }

            var filtered = Computers.Where(c =>
                c.Type != null &&
                !string.IsNullOrEmpty(c.Type.TypeName) &&
                c.Type.TypeName.ToLower().Contains(keyword)
            ).ToList();

            if (filtered.Count == 0)
            {
                MessageBox.Show("Không tìm thấy máy thuộc loại: " + keyword, "Kết quả", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            ComputerGrid.ItemsSource = filtered;
        }


    }
}
