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
using WpfApp1.Models;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for ComputerTypeWindow.xaml
    /// </summary>
    public partial class ComputerTypeWindow : Window
    {
        private ObservableCollection<ComputerType> _types = new();

        public ComputerTypeWindow()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            using var context = new NetManagementContext();
            _types = new ObservableCollection<ComputerType>(context.ComputerTypes.ToList());
            TypeGrid.ItemsSource = _types;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var newType = new ComputerType { TypeName = "Loại mới", HourlyRate = 10000 };
            _types.Add(newType);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            using var context = new NetManagementContext();

            foreach (var type in _types)
            {
                if (string.IsNullOrWhiteSpace(type.TypeName))
                {
                    MessageBox.Show("Tên loại không được để trống.");
                    return;
                }

                var isDuplicate = context.ComputerTypes
                    .Any(t => t.TypeName.ToLower() == type.TypeName.ToLower() && t.TypeId != type.TypeId);

                if (isDuplicate)
                {
                    MessageBox.Show($"Loại máy \"{type.TypeName}\" đã tồn tại.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var existing = context.ComputerTypes.FirstOrDefault(t => t.TypeId == type.TypeId);
                if (existing != null)
                {
                    existing.TypeName = type.TypeName;
                    existing.HourlyRate = type.HourlyRate;
                }
                else
                {
                    context.ComputerTypes.Add(type);
                }
            }

            context.SaveChanges();
            MessageBox.Show("Đã lưu thay đổi!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            LoadData();
        }


        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is ComputerType selectedType)
            {
                if (MessageBox.Show($"Bạn có chắc muốn xoá loại máy \"{selectedType.TypeName}\"?", "Xác nhận", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    using var context = new NetManagementContext();

                    // Kiểm tra ràng buộc
                    bool hasComputers = context.Computers.Any(c => c.TypeId == selectedType.TypeId);
                    if (hasComputers)
                    {
                        MessageBox.Show("Không thể xoá loại máy đang được sử dụng.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    var dbType = context.ComputerTypes.Find(selectedType.TypeId);
                    if (dbType != null)
                    {
                        context.ComputerTypes.Remove(dbType);
                        context.SaveChanges();
                        _types.Remove(selectedType);
                    }
                }
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
