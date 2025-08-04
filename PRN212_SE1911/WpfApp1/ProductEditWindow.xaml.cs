using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for ProductEditWindow.xaml
    /// </summary>
    public partial class ProductEditWindow : Window
    {
        private int? productId;
        private Product? product;
        private User _loggedInUser;

        public ProductEditWindow(int? id, User user)
        {
            InitializeComponent();
            productId = id;
            _loggedInUser = user;

            // 🔐 Kiểm tra quyền
            if (_loggedInUser.RoleId != 1)
            {
                MessageBox.Show("Bạn không có quyền truy cập vào chức năng này.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                Close();
                return;
            }

            LoadData();
        }

        private void LoadData()
        {
            if (productId == null)
                return;

            using var context = new NetManagementContext();
            product = context.Products.Find(productId);

            if (product != null)
            {
                txtName.Text = product.ProductName;
                txtPrice.Text = product.Price.ToString("0", CultureInfo.InvariantCulture);
                chkAvailable.IsChecked = product.IsAvailable;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            string name = txtName.Text.Trim();
            bool isPriceValid = decimal.TryParse(txtPrice.Text, out decimal price);

            if (string.IsNullOrEmpty(name) || !isPriceValid)
            {
                MessageBox.Show("Vui lòng nhập tên và giá hợp lệ.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using var context = new NetManagementContext();

            if (productId == null)
            {
                var newProduct = new Product
                {
                    ProductName = name,
                    Price = price,
                    IsAvailable = chkAvailable.IsChecked ?? true
                };
                context.Products.Add(newProduct);
            }
            else
            {
                var existingProduct = context.Products.Find(productId);
                if (existingProduct != null)
                {
                    existingProduct.ProductName = name;
                    existingProduct.Price = price;
                    existingProduct.IsAvailable = chkAvailable.IsChecked ?? true;
                }
            }

            context.SaveChanges();
            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
