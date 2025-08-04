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
    /// Interaction logic for SelectProductWindow.xaml
    /// </summary>
    public partial class SelectProductWindow : Window
    {
        public class SelectedItem
        {
            public Product Product { get; set; }
            public int Quantity { get; set; }
            public decimal TotalPrice => Product.Price * Quantity;
        }

        public List<SelectedItem> SelectedItems { get; private set; } = new();

        private List<Product> _availableProducts = new();

        // Hỗ trợ truyền danh sách sản phẩm cũ vào để sửa
        public SelectProductWindow(List<SelectedItem>? existingItems = null)
        {
            InitializeComponent();
            LoadProducts();

            if (existingItems != null)
            {
                SelectedItems = existingItems;
                RefreshGrid();
            }
        }

        private void LoadProducts()
        {
            using var context = new NetManagementContext();
            _availableProducts = context.Products.Where(p => p.IsAvailable == true).ToList();
            cbProducts.ItemsSource = _availableProducts;
        }

        private void cbProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbProducts.SelectedItem is Product product)
            {
                txtPrice.Text = $"{product.Price:N0} VNĐ";
            }
            else
            {
                txtPrice.Text = "";
            }
        }

        private void AddToList_Click(object sender, RoutedEventArgs e)
        {
            if (cbProducts.SelectedItem is Product product &&
                int.TryParse(txtQuantity.Text, out int qty) && qty > 0)
            {
                var existing = SelectedItems.FirstOrDefault(p => p.Product.ProductId == product.ProductId);
                if (existing != null)
                {
                    existing.Quantity += qty;
                }
                else
                {
                    SelectedItems.Add(new SelectedItem
                    {
                        Product = product,
                        Quantity = qty
                    });
                }

                RefreshGrid();
                txtQuantity.Text = "1";
            }
            else
            {
                MessageBox.Show("Vui lòng chọn sản phẩm và nhập số lượng hợp lệ.");
            }
        }

        private void RefreshGrid()
        {
            dgSelectedProducts.ItemsSource = null;
            dgSelectedProducts.ItemsSource = SelectedItems;
        }

        private void EditProduct_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is SelectedItem item)
            {
                string input = Microsoft.VisualBasic.Interaction.InputBox(
                    $"Nhập lại số lượng cho {item.Product.ProductName}:", "Sửa số lượng", item.Quantity.ToString());

                if (int.TryParse(input, out int newQty))
                {
                    if (newQty > 0)
                    {
                        item.Quantity = newQty;
                    }
                    else
                    {
                        SelectedItems.Remove(item);
                    }
                    RefreshGrid();
                }
                else
                {
                    MessageBox.Show("Số lượng không hợp lệ!");
                }
            }
        }

        private void DeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is SelectedItem item)
            {
                if (MessageBox.Show($"Xoá {item.Product.ProductName} khỏi danh sách?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    SelectedItems.Remove(item);
                    RefreshGrid();
                }
            }
        }

        private void Select_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedItems.Count == 0)
            {
                MessageBox.Show("Bạn chưa chọn sản phẩm nào.");
                return;
            }

            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
