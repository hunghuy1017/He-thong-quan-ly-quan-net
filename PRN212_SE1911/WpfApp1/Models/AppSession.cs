using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Models
{
    public static class AppSession
    {
        // Lưu thông tin người dùng hiện tại sau khi đăng nhập
        public static User? CurrentUser { get; set; }

        // Xóa thông tin người dùng khi đăng xuất
        public static void Clear()
        {
            CurrentUser = null;
        }

        // Kiểm tra có người đăng nhập chưa
        public static bool IsLoggedIn => CurrentUser != null;

        // Kiểm tra có phải Admin không (RoleId = 1)
        public static bool IsAdmin => CurrentUser?.RoleId == 1;
    }

}
