using System.Text;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private User _loggedInUser;

        public MainWindow(User user)
        {
            InitializeComponent();
            _loggedInUser = user;
            WelcomeText.Text = $"Xin chào, {_loggedInUser.FullName ?? _loggedInUser.UserName}";

            // ✅ Phân quyền: nếu không phải Admin (RoleId != 1) thì ẩn chức năng quản lý
            if (_loggedInUser.RoleId != 1)
            {
                Account_ClickBtn.IsEnabled = false;
                Account_ClickBtn.Visibility = Visibility.Collapsed;

                Product_ClickBtn.IsEnabled = false;
                Product_ClickBtn.Visibility = Visibility.Collapsed;

                Report_ClickBtn.IsEnabled = false;
                Report_ClickBtn.Visibility = Visibility.Collapsed;
            }
        }

        private void Computer_Click(object sender, RoutedEventArgs e)
        {
            ComputerWindow window = new ComputerWindow(_loggedInUser); // ✅ truyền user đang đăng nhập
            window.ShowDialog();
        }


        private void Session_Click(object sender, RoutedEventArgs e)
        {
            SessionWindow window = new SessionWindow(_loggedInUser); // ✅ truyền user
            window.ShowDialog();
        }


        private void Product_Click(object sender, RoutedEventArgs e)
        {
            ProductWindow window = new ProductWindow(_loggedInUser);
            window.ShowDialog();
        }


        private void Account_Click(object sender, RoutedEventArgs e)
        {
            var window = new AccountManagerWindow(_loggedInUser);
            window.ShowDialog();
        }

        private void Report_Click(object sender, RoutedEventArgs e)
        {
            var window = new RevenueReportWindow(_loggedInUser); // truyền User vào
            window.ShowDialog();
        }


        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow login = new LoginWindow();
            login.Show();
            this.Close();
        }
        private void History_Click(object sender, RoutedEventArgs e)
        {
            var window = new CustomerHistoryWindow(); // Trang lịch sử đã tạo ở trên
            window.ShowDialog();
        }

    }
}