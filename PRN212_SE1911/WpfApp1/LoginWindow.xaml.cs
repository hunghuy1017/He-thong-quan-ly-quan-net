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
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private readonly NetManagementContext _context = new NetManagementContext();

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameBox.Text.Trim();
            string password = PasswordBox.Password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập và mật khẩu.", "Thiếu thông tin", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var user = _context.Users
                    .FirstOrDefault(u => u.UserName == username && u.IsActive == true); // phân biệt hoa thường

                if (user == null)
                {
                    MessageBox.Show("Tài khoản không tồn tại hoặc bị khóa.", "Lỗi đăng nhập", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (user.PasswordHash != password || user.UserName != username)
                {
                    MessageBox.Show("Tài khoản hoặc mật khẩu không đúng.", "Lỗi đăng nhập", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // ✅ Gán user vào AppSession
                AppSession.CurrentUser = user;

                MessageBox.Show($"Chào mừng {user.FullName ?? user.UserName}!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                var main = new MainWindow(user);
                main.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hệ thống: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

