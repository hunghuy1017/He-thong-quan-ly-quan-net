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
using Microsoft.EntityFrameworkCore;
using WpfApp1.Models;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for AccountManagerWindow.xaml
    /// </summary>
    public partial class AccountManagerWindow : Window
    {
        private readonly User _loggedInUser;

        public AccountManagerWindow(User loggedInUser)
        {
            InitializeComponent();
            _loggedInUser = loggedInUser;
            LoadUsers();
        }

        private void LoadUsers()
        {
            using var context = new NetManagementContext();
            var users = context.Users
                .Include(u => u.Role)
                .ToList();

            dgUsers.ItemsSource = users;
        }

        private void Reload_Click(object sender, RoutedEventArgs e)
        {
            LoadUsers();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement fe && fe.Tag is User selectedUser)
            {
                var editWindow = new AccountEditWindow(selectedUser.UserId, _loggedInUser);
                editWindow.ShowDialog();
                LoadUsers();
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var editWindow = new AccountEditWindow(null, _loggedInUser); // thêm mới
            editWindow.ShowDialog();
            LoadUsers();
        }
    }
}
