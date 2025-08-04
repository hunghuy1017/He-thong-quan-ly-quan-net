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
using WpfApp1.Models;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for ProductWindow.xaml
    /// </summary>
    public partial class ProductWindow : Window
    {
        private User _loggedInUser;

        public ProductWindow(User user)
        {
            InitializeComponent();
            _loggedInUser = user;
            LoadProducts();
            ApplyPermissions();
        }

        private void ApplyPermissions()
        {
            if (_loggedInUser.RoleId != 1) // Staff
            {
                btnAdd.Visibility = Visibility.Collapsed;
                ActionColumn.Visibility = Visibility.Collapsed;
            }
        }

        private void LoadProducts()
        {
            using var context = new NetManagementContext();
            var products = context.Products.ToList();
            dgProducts.ItemsSource = products;
        }

        private void Reload_Click(object sender, RoutedEventArgs e)
        {
            LoadProducts();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var editWindow = new ProductEditWindow(null, _loggedInUser); // truyền User vào
            editWindow.ShowDialog();
            LoadProducts();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement fe && fe.Tag is Product selectedProduct)
            {
                var editWindow = new ProductEditWindow(selectedProduct.ProductId, _loggedInUser);
                editWindow.ShowDialog();
                LoadProducts();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (_loggedInUser.RoleId != 1) return; // Chỉ admin được xóa

            if (sender is FrameworkElement fe && fe.Tag is Product productToDelete)
            {
                var confirm = MessageBox.Show($"Bạn có chắc muốn xóa sản phẩm '{productToDelete.ProductName}'?",
                                              "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (confirm != MessageBoxResult.Yes) return;

                try
                {
                    using var context = new NetManagementContext();

                    // ❗ Kiểm tra có tham chiếu trong OrderDetails không
                    bool isReferenced = context.OrderDetails.Any(od => od.ProductId == productToDelete.ProductId);
                    if (isReferenced)
                    {
                        MessageBox.Show("Không thể xóa vì sản phẩm đã từng tồn tại trong đơn hàng.",
                                        "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    var product = context.Products.Find(productToDelete.ProductId);
                    if (product != null)
                    {
                        context.Products.Remove(product);
                        context.SaveChanges();
                        MessageBox.Show("Xóa thành công.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadProducts();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa sản phẩm:\n" + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }



    }
}

