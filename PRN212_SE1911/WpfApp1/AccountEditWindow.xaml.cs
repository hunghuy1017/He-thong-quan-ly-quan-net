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
    /// Interaction logic for AccountEditWindow.xaml
    /// </summary>
    public partial class AccountEditWindow : Window
    {
        private readonly int? _userId;             // ID tài khoản đang chỉnh
        private readonly User _currentUser;        // Người đang đăng nhập
        private User? _editingUser;                // Đối tượng cần chỉnh

        public AccountEditWindow(int? userId, User currentUser)
        {
            InitializeComponent();
            _userId = userId;
            _currentUser = currentUser;
            LoadRoles();

            if (_userId.HasValue)
                LoadUser();
            else
                pwdPassword.Visibility = Visibility.Visible; // Hiện khi tạo mới
        }

        private void LoadRoles()
        {
            using var context = new NetManagementContext();
            var roles = context.Roles.ToList();
            cboRole.ItemsSource = roles;
            cboRole.DisplayMemberPath = "RoleName";
            cboRole.SelectedValuePath = "RoleId";
        }

        private void LoadUser()
        {
            using var context = new NetManagementContext();
            _editingUser = context.Users.FirstOrDefault(u => u.UserId == _userId);
            if (_editingUser == null) return;

            txtUserName.Text = _editingUser.UserName;
            txtFullName.Text = _editingUser.FullName;
            chkActive.IsChecked = _editingUser.IsActive ?? true;
            cboRole.SelectedValue = _editingUser.RoleId;

            txtUserName.IsEnabled = false;

            if (_currentUser.UserId == _editingUser.UserId)
            {
                // Chủ tài khoản → được sửa mật khẩu
                pwdPassword.Visibility = Visibility.Visible;
                pwdPassword.Password = _editingUser.PasswordHash;
                pwdPassword.IsEnabled = true;
            }
            else
            {
                // Admin hoặc người khác → ẩn mật khẩu
                pwdPassword.Visibility = Visibility.Collapsed;
                pwdPassword.IsEnabled = false;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            using var context = new NetManagementContext();

            try
            {
                if (string.IsNullOrWhiteSpace(txtUserName.Text) ||
                    (!_userId.HasValue && string.IsNullOrWhiteSpace(pwdPassword.Password)) ||
                    cboRole.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng điền đầy đủ thông tin bắt buộc.", "Thiếu thông tin", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (_userId.HasValue)
                {
                    var user = context.Users.FirstOrDefault(u => u.UserId == _userId);
                    if (user != null)
                    {
                        user.FullName = txtFullName.Text;
                        user.RoleId = (int)cboRole.SelectedValue;
                        user.IsActive = chkActive.IsChecked;

                        if (_currentUser.UserId == user.UserId)
                        {
                            // Tự sửa mình → được đổi mật khẩu
                            user.PasswordHash = pwdPassword.Password;
                        }
                        else
                        {
                            // Người khác sửa → lưu thời gian
                            user.LastModified = DateTime.Now;
                        }
                    }
                }
                else
                {
                    // Tạo mới
                    var exists = context.Users.Any(u => u.UserName == txtUserName.Text);
                    if (exists)
                    {
                        MessageBox.Show("Tên đăng nhập đã tồn tại.", "Trùng tài khoản", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    var newUser = new User
                    {
                        UserName = txtUserName.Text,
                        PasswordHash = pwdPassword.Password,
                        FullName = txtFullName.Text,
                        RoleId = (int)cboRole.SelectedValue,
                        IsActive = chkActive.IsChecked ?? true
                    };

                    context.Users.Add(newUser);
                }

                context.SaveChanges();
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
